using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweets.BusinessLayer
{
    class Sentiment
    {
        public  string Word { get; set; }
        public  double Value { get; set; }

        public Sentiment(string word, double value)
        {
            Word = word;
            Value = value;
        }
    }
}
