using GMap.NET;
using GMap.NET.WindowsForms;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweets.BusinessLayer;

namespace Tweets.DataAccessLayer
{
    class TweetFunc
    {
        public static List<Tweet> parse(string filename)
        {
            List<Tweet> lines = new List<Tweet>(); 
            string[] tweets = File.ReadAllLines(filename);
            foreach (string tweet in tweets)
            {
                try
                {
                    List<string> words = new List<string>();
                    string[] parts = tweet.Split('_');
                    string cords = parts[0].TrimEnd();
                    cords = cords.Trim(new char[] { '[', ']' });
                    string datetimetext;
                    try { datetimetext = parts[1].TrimStart(); }
                    catch { continue; }
                    string[] oxoy = cords.Split(',');
                    string ox = oxoy[0].Trim();
                    string oy = oxoy[1].Trim();
                    string[] parts2 = datetimetext.Split(' ');
                    string date = parts2[0].Trim();
                    string timeword = parts2[1].Trim();
                    string time = timeword.Split('\t')[0];
                    try { string word0 = timeword.Split('\t')[1]; words.Add(word0); }
                    catch { }

                    for (int i = 2; i < parts2.Length; i++)
                    {
                        string word = parts2[i].Trim();
                        words.Add(word);
                    }
                    string tweettext = "";
                    foreach (string word in words)
                    {
                        tweettext += word + " ";
                    }
                    lines.Add(new Tweet(Convert.ToDouble(ox, CultureInfo.InvariantCulture),
                        Convert.ToDouble(oy, CultureInfo.InvariantCulture), date, time, tweettext, words));
                }
                catch { continue; }
            }
            return lines;
        }

        public static Dictionary<string, List<Tweet>> GetDictionary(List<Tweet> tweets, Dictionary<string, List<List<List<double>>>> states)
        {
            Dictionary<string, List<Tweet>> dictinary = new Dictionary<string, List<Tweet>>();
            string code = null;
            foreach (Tweet tweet in tweets)
            {
                code = GetPostalCode(tweet, states);
                if (code != null)
                {
                    if (!dictinary.ContainsKey(code))
                    {
                        List<Tweet> tweetlist = new List<Tweet>();
                        tweetlist.Add(tweet);
                        dictinary.Add(code, tweetlist);
                    }
                    else
                    {
                        dictinary[code].Add(tweet);
                    }
                }
            }
            return dictinary;
        }

        public static string GetPostalCode(Tweet tweet, Dictionary<string, List<List<List<double>>>> states)
        {
            foreach (KeyValuePair<string, List<List<List<double>>>> state in states)
            {
                foreach (List<List<double>> polygon in states[state.Key])
                {
                    List<PointLatLng> points = new List<PointLatLng>();
                    foreach (List<double> point in polygon)
                    {
                        points.Add(new PointLatLng(point[1], point[0]));
                    }
                    GMapPolygon newpolygon = new GMapPolygon(points, state.Key);
                    if (newpolygon.IsInside(new PointLatLng(tweet.OX, tweet.OY)))
                    {
                        return state.Key;
                    }
                }
            }
            return null;
        }
    }
}
