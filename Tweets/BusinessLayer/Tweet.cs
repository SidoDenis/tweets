using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweets.BusinessLayer
{
    class Tweet
    {
        public  double OX { get; set; }
        public  double OY { get; set; }
        public  string Date { get; set; }
        public  string Time { get; set; }
        public  string Text { get; set; }
        public  List<string> Words { get; set; }

        public Tweet(double ox, double oy, string date, string time, string text, List<string> words)
        {
            OX = ox;
            OY = oy;
            Date = date;
            Time = time;
            Text = text;
            Words = words;
        }
    }
}
