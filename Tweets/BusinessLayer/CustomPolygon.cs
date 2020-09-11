using GMap.NET.WindowsForms;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tweets.BusinessLayer
{
    class CustomPolygon : GMapPolygon
    {
        public  PointLatLng Centre { get; set; }
        public  int PointAmount { get; set; }

        public CustomPolygon(List<PointLatLng> points, string state, PointLatLng centre, int pointamount) : base(points, state)
        {
            Centre = centre;
            PointAmount = pointamount;
        }
    }
}
