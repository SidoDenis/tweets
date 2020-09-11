using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweets.BusinessLayer;
using Tweets.DataAccessLayer;
using GMap.NET;
using GMap.NET.WindowsForms;
using System.Drawing;
using GMap.NET.WindowsForms.Markers;
using System.Threading;

namespace Tweets.ServiceLayer
{
    class DataManipulation
    {
        public static List<Sentiment> sentiments = new List<Sentiment>();
        public static List<Tweet> tweets = new List<Tweet>();
        public static Dictionary<string, List<List<List<double>>>> states = new Dictionary<string, List<List<List<double>>>>();
        public static Dictionary<string, List<Tweet>> sortedTweets = new Dictionary<string, List<Tweet>>();
        public static Dictionary<string, double> statesMood = new Dictionary<string, double>();
        public static Dictionary<string, List<CustomPolygon>> polygonsDictionary = new Dictionary<string, List<CustomPolygon>>();

        public static void GetListOfSentiments()
        {
            sentiments = SentimentList.getsentiments();
        }

        public static void GetListOfTweets(string path)
        {
            tweets = TweetFunc.parse(path);
        }

        public static void GetListOfStates()
        {
            states = Polygons.deserialize();
        }

        public static void GetTweetsByStates()
        {
            sortedTweets = TweetFunc.GetDictionary(tweets, states);
        }

        public static void GetMoodByStates()
        {
            statesMood = statemood.GetStatesMood(sortedTweets, sentiments);
        }

        public static void GetListOfPolygons()
        {
            polygonsDictionary = Polygons.GetPolygons(states);
        }

        

        public static string GetPath(RadioButton b1, RadioButton b2, RadioButton b3, RadioButton b4,
            RadioButton b5, RadioButton b6, RadioButton b7, RadioButton b8, RadioButton b9)
        {
            string filename;
            if (b1.Checked) { filename = "cali_tweets2014.txt"; return filename; }
            else if(b2.Checked) { filename = "family_tweets2014.txt"; return filename; }
            else if(b3.Checked) { filename = "football_tweets2014.txt"; return filename; }
            else if(b4.Checked) { filename = "high_school_tweets2014.txt"; return filename; }
            else if(b5.Checked) { filename = "movie_tweets2014.txt"; return filename; }
            else if(b6.Checked) { filename = "shopping_tweets2014.txt"; return filename; }
            else if(b7.Checked) { filename = "snow_tweets2014.txt"; return filename; }
            else if(b8.Checked) { filename = "texas_tweets2014.txt"; return filename; }
            else if(b9.Checked) { filename = "weekend_tweets2014.txt"; return filename; }
            return null;
        }

        
        public static void Layout(GMapControl map)
        {
            
            foreach (KeyValuePair<string, List<CustomPolygon>> polygonlist in polygonsDictionary)
            {
                var polygonslist = new GMapOverlay(polygonlist.Key);
                foreach (CustomPolygon polygon in polygonsDictionary[polygonlist.Key])
                {
                    polygon.Stroke = new Pen(Color.Black, 1);
                    if (!statesMood.ContainsKey(polygonlist.Key)) polygon.Fill = new SolidBrush(Color.Gray);
                    else
                    {
                        double b = statesMood[polygonlist.Key];
                        int red = 0, blue = 0, green = 0;
                        if(statesMood[polygonlist.Key] > 0)
                        {
                            red = 55 + Convert.ToInt32(statesMood[polygonlist.Key] * 5);
                            green = Math.Abs(55 - Convert.ToInt32(statesMood[polygonlist.Key] * 5));
                            if (red > 255) red = 255;
                        }
                        else if (statesMood[polygonlist.Key] < 0)
                        {
                            blue = 55 + Convert.ToInt32(Math.Abs(statesMood[polygonlist.Key])*5);
                            if (blue > 255) blue = 255;
                        }
                        else
                        {
                            red = 255; blue = 255; green = 255; 
                        }
                        polygon.Fill = new SolidBrush(Color.FromArgb(red,green,blue));
                    }
                    polygonslist.Polygons.Add(polygon);
                }
                map.Overlays.Add(polygonslist); 
            }
            
            foreach (KeyValuePair<string, List<CustomPolygon>> polygonlist in polygonsDictionary)
            {
                PointLatLng centre = new PointLatLng(); int max = 0, i = 0, b = 0;
                foreach(CustomPolygon polygon in polygonsDictionary[polygonlist.Key])
                {
                    if (polygon.PointAmount > max)
                    {
                        max = polygon.PointAmount;
                        b = i;
                    }
                    i++;
                }
                centre = polygonsDictionary[polygonlist.Key][b].Centre;               
                var markerOverlay = new GMapOverlay("markers");

                var labelMarker = new GmapMarkerWithLabel(new PointLatLng(centre.Lat, centre.Lng), polygonlist.Key, GMarkerGoogleType.blue);
                markerOverlay.Markers.Add(labelMarker);

                map.Overlays.Add(markerOverlay);
            }

        }

        
    }
}
