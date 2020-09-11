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
    class SentimentList
    {
        public static List<Sentiment> getsentiments()
        {
            List<Sentiment> sentiments = new List<Sentiment>();
            const string Path = @"C:\Users\Denis\source\repos\Tweets\Tweets\files\sentiments.csv";
            using (var stream = File.OpenRead(Path))
            using (var reader = new StreamReader(stream))
            {
                var data = CsvParser.ParseHeadAndTail(reader, ',', '"');  
                var lines = data.Item2;

                foreach (var line in lines)
                {
                    try
                    {
                        for (var i = 0; i < line.Count; i++)
                            if (!string.IsNullOrEmpty(line[i]))
                                continue;
                        string name = line[0];
                        sentiments.Add(new Sentiment(line[0], Convert.ToDouble(line[1], CultureInfo.InvariantCulture)));
                    }
                    catch { continue; }
                }
            }
            return sentiments;
        }
    }
}
