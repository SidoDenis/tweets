using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweets.BusinessLayer;

namespace Tweets.DataAccessLayer
{
    class statemood
    {
        public static Dictionary<string, double> GetStatesMood(Dictionary<string, List<Tweet>> sortedTweets, List<Sentiment> sentiments)
        {
            Dictionary<string, double> statesMood = new Dictionary<string, double>();
            foreach (KeyValuePair<string, List<Tweet>> states in sortedTweets)
            {
                double mood = 0; int i = 0;
                if (sortedTweets[states.Key].Count == 0)
                    continue;
                foreach (Tweet tweet in sortedTweets[states.Key])
                {
                    double moodvalue = GetMoodValue(tweet, sentiments);
                    mood += moodvalue;
                    if (moodvalue != 0) i++;
                }
                if (i == 0)
                    continue;
                statesMood.Add(states.Key, mood);
            }
            return statesMood;
        }

        public static double GetMoodValue(Tweet tweet, List<Sentiment> sentiments)
        {
            double moodvalue = 0; int i = 0;
            foreach (Sentiment sentiment in sentiments)
            {
                if (sentiment.Word.Split(' ').Length == 1)
                {
                    foreach(string word in tweet.Words)
                    {
                        if (word.Equals(sentiment))
                        {
                            moodvalue += sentiment.Value;
                            i++;
                        }
                    }
                }
                else 
                {
                    if (tweet.Text.Contains(sentiment.Word))
                    {
                        moodvalue += sentiment.Value;
                        i++;
                    }
                }
            }
            if (i > 0) return moodvalue / i;
            else return 0;
        }
    }
}
