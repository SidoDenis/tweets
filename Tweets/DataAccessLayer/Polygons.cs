using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using GMap.NET;
using GMap.NET.WindowsForms;
using Tweets.BusinessLayer;

namespace Tweets.DataAccessLayer
{
    class Polygons
    {
        public static Dictionary<string, List<List<List<double>>>> deserialize()
        {
            string jsonString = new StreamReader("C:\\Users\\Denis\\source\\repos\\Tweets\\Tweets\\files\\states.json").ReadToEnd();
            //Console.WriteLine(jsonString);
            Dictionary<string, List<List<List<double>>>> states = 
                JsonConvert.DeserializeObject<Dictionary<string, List<List<List<double>>>>>(jsonString);
            return states;
        }

        public static Dictionary<string, List<CustomPolygon>> GetPolygons(Dictionary<string, List<List<List<double>>>> states)
        {
            Dictionary<string, List<CustomPolygon>> polygons = new Dictionary<string, List<CustomPolygon>>();
            foreach (KeyValuePair<string, List<List<List<double>>>> state in states)
            {

                foreach (List<List<double>> polygon in states[state.Key])
                {

                    List<PointLatLng> points = new List<PointLatLng>();
                    foreach (List<double> point in polygon)
                    {
                        points.Add(new PointLatLng(point[1], point[0]));
                    }
                    CustomPolygon newpolygon = new CustomPolygon(points, state.Key, GetCentroid(points), points.Count());

                    if (!polygons.ContainsKey(state.Key))
                    {
                        List<CustomPolygon> polygonlist = new List<CustomPolygon>();
                        polygonlist.Add(newpolygon);
                        polygons.Add(state.Key, polygonlist);
                    }
                    else
                    {
                        polygons[state.Key].Add(newpolygon);
                    }

                }
            }
            return polygons;
        }

        public static PointLatLng GetCentroid(List<PointLatLng> poly)
        {
            double accumulatedArea = 0.0;
            double centerX = 0.0;
            double centerY = 0.0;

            for (int i = 0, j = poly.Count - 1; i < poly.Count; j = i++)
            {
                double temp = poly[i].Lat * poly[j].Lng - poly[j].Lat * poly[i].Lng;
                accumulatedArea += temp;
                centerX += (poly[i].Lat + poly[j].Lat) * temp;
                centerY += (poly[i].Lng + poly[j].Lng) * temp;
            }

            if (Math.Abs(accumulatedArea) < 1E-7f)
                return PointLatLng.Empty;

            accumulatedArea *= 3f;
            return new PointLatLng(centerX / accumulatedArea, centerY / accumulatedArea);
        }
    }
}
