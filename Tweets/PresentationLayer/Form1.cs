using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweets.DataAccessLayer;
using Tweets.ServiceLayer;
using GMap.NET;
using GMap.NET.WindowsForms;
using GMap.NET.MapProviders;

namespace Tweets
{
    public partial class Form1 : Form
    {
        Thread[] threads;
        
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Map();
            DataManipulation.GetListOfStates();
            DataManipulation.GetListOfSentiments();
            update();
            label1.Text = "Ready!";
        }

        private void Map()
        {
            map.ShowCenter = false;
            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
            map.MinZoom = 3;
            map.MaxZoom = 18;
            map.Zoom = 5;
            map.Position = new PointLatLng(38.851469, -78.996008);
        }
        
        private void update()
        { 
            string filename = DataManipulation.GetPath(radioButton1,radioButton2,radioButton3,radioButton4,
                radioButton5,radioButton6,radioButton7,radioButton8,radioButton9);
            DataManipulation.GetListOfTweets(@"C:\Users\Denis\source\repos\Tweets\Tweets\files\"+filename);
            DataManipulation.GetTweetsByStates();
            DataManipulation.GetMoodByStates();
            DataManipulation.GetListOfPolygons();
            DataManipulation.Layout(map);
        }

        private void loading()
        {
            label1.Text = "Wait...";
        }

        private void thread()
        {
            threads = new Thread[1];
            threads[0] = new Thread(loading);
            threads[0].Start();
            threads[0].Join();
            threads[0].Abort();
            update();
            label1.Text = "Ready!";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            thread();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            thread();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            thread();
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            thread();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            thread();
        }

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            thread();
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            thread();
        }

        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            thread();
        }

        private void radioButton9_CheckedChanged(object sender, EventArgs e)
        {
            thread();
        }
    }
}
