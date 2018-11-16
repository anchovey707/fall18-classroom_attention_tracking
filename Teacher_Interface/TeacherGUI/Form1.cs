using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace TeacherGUI
{
    public partial class Form1 : Form
    {
        //static TcpClient clientSocket = new TcpClient();
        Socket clientSocket;
        byte[] outStream;
        byte[] inStream;
        UdpClient listener;
        public Socket udpsock;

        public Form1(int course)
        {
            InitializeComponent();
            try
            {
                clientSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                udpsock = new Socket(SocketType.Dgram, ProtocolType.Udp);

                outStream = new byte[10025];

                Console.WriteLine("trying to connect");
                clientSocket.Connect("10.40.45.58", 61600);

               Console.WriteLine("waiting for starting char");
                String returndata="";


                //Wait for the start char, ';'
                while (returndata != ";") {
                    inStream=new byte[clientSocket.Available];
                    clientSocket.Receive(inStream);
                    returndata = Encoding.ASCII.GetString(inStream);
                    Console.WriteLine("'"+returndata+"'");
                }
                Console.WriteLine("got start char");
                outStream = Encoding.ASCII.GetBytes("#" + Environment.UserName + "#" + course + ";");
                //send my name and corse that I want to stream
                clientSocket.Send(outStream);
                Console.WriteLine("waiting for port info");
                //wait for port
                inStream = new byte[0];
                returndata ="";
                while (!returndata.Contains(";")) {
                    inStream = new byte[clientSocket.Available];
                    clientSocket.Receive(inStream);
                    returndata += Encoding.ASCII.GetString(inStream);
                }
                Console.WriteLine("got data="+returndata);
                Console.WriteLine(returndata.IndexOf(":"));
                Console.WriteLine(returndata.Length);

                Console.WriteLine("port=" + returndata.Substring(returndata.IndexOf(":") + 1,returndata.Length -returndata.IndexOf(":")-2));
                returndata = returndata.Substring(returndata.IndexOf(":") + 1, returndata.Length - returndata.IndexOf(":") - 2);


                udpsock.Bind(new IPEndPoint(IPAddress.Any,int.Parse(returndata)));
                Console.WriteLine("Starting UDP");
                ThreadStart childref = new ThreadStart(ListenForPackets);
                Thread childThread = new Thread(childref);
                childThread.Start();
                Console.WriteLine("Thread should have started");
            }
            catch (Exception e){
                please.Text = e.StackTrace;
                Console.WriteLine(e.StackTrace);
            }




            // Set the view to show details.


            // Create new memory bitmap the same size as the picture box
            Bitmap bMap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            // Initialize random number generator
            Random rRand = new Random();
            // Loop variables
            int iX;
            int iY;
            byte iIntense;
            // Lets loop 500 times and create a random point each iteration
            for (int i = 0; i < 500; i++)
            {
                // Pick random locations and intensity
                iX = rRand.Next(0, 200);
                iY = rRand.Next(0, 200);
                iIntense = (byte)rRand.Next(0, 120);
                // Add heat point to heat points list
                HeatPoints.Add(new HeatPoint(iX, iY, iIntense));
            }
            // Call CreateIntensityMask, give it the memory bitmap, and use it's output to set the picture box image
            pictureBox1.Image = CreateIntensityMask(bMap, HeatPoints);



            listView1.View = View.Details;
            listView1.CheckBoxes = true;
            // Display grid lines.
            listView1.GridLines = true;
           // listView2.View = View.Details;

           // listView2.CheckBoxes = true;

            // Display grid lines.
           // listView2.GridLines = true;

            // Create three items and three sets of subitems for each item.
            ListViewItem item1 = new ListViewItem("Austin Lambeth",0);
            item1.SubItems.Add("al05661");
            item1.SubItems.Add("Current Slides");
            item1.SubItems.Add("3");
            ListViewItem item2 = new ListViewItem("Dylan Albrecht", 1);
            item2.SubItems.Add("fagot.net");
            item2.SubItems.Add("something he shouldn't have open");
            item2.SubItems.Add("6");
            ListViewItem item3 = new ListViewItem("Phillip", 1);
            item3.SubItems.Add("cool guy philip");
            item3.SubItems.Add("Current Slides");
            item3.SubItems.Add("9");

            // Create columns for the items and subitems.
            // Width of -2 indicates auto-size.
            listView1.Columns.Add("Student Name", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Student Email", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Current Focused Window", -2, HorizontalAlignment.Left);
            ListViewItem chrome = new ListViewItem("Chrome", 0);
            chrome.SubItems.Add("1");
            chrome.SubItems.Add("2");
            chrome.SubItems.Add("3");
            ListViewItem mozilla = new ListViewItem("mozilla", 0);
            //listView2.Columns.Add("Open Windows", -2, HorizontalAlignment.Left);
            //Add the items to the ListView.
           listView1.Items.AddRange(new ListViewItem[] { item1, item2, item3 });
            //listView2.Items.AddRange(new ListViewItem[] { chrome,mozilla });


        }

        public void ListenForPackets()
        {
            while (true)
            {
                //Console.WriteLine("Im in boss");
                if (udpsock.Available > 0)
                {
                    inStream = new byte[udpsock.Available];

                    udpsock.Receive(inStream);
                    Console.WriteLine("Packet=" + Encoding.ASCII.GetString(inStream));
                }
                
            }
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            new TeacherHome(false).Show();
            Hide();
        }

        private List<HeatPoint> HeatPoints = new List<HeatPoint>();
        
        private void button2_Click(object sender, EventArgs e)
        {


















            /*
            // Create new memory bitmap the same size as the picture box
            Bitmap bMap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            // Initialize random number generator
            Random rRand = new Random();
            // Loop variables
            int iX;
            int iY;
            byte iIntense;
            // Lets loop 500 times and create a random point each iteration
            for (int i = 0; i < 500; i++)
            {
                // Pick random locations and intensity
                iX = rRand.Next(0, 200);
                iY = rRand.Next(0, 200);
                iIntense = (byte)rRand.Next(0, 120);
                // Add heat point to heat points list
                HeatPoints.Add(new HeatPoint(iX, iY, iIntense));
            }
            // Call CreateIntensityMask, give it the memory bitmap, and use it's output to set the picture box image
            pictureBox1.Image = CreateIntensityMask(bMap, HeatPoints);
            */
        }
        private Bitmap CreateIntensityMask(Bitmap bSurface, List<HeatPoint> aHeatPoints)
        {
            // Create new graphics surface from memory bitmap
            Graphics DrawSurface = Graphics.FromImage(bSurface);
            // Set background color to white so that pixels can be correctly colorized
            DrawSurface.Clear(Color.White);
            // Traverse heat point data and draw masks for each heat point
            foreach (HeatPoint DataPoint in aHeatPoints)
            {
                // Render current heat point on draw surface
                DrawHeatPoint(DrawSurface, DataPoint, 15);
            }
            return bSurface;
        }
        private void DrawHeatPoint(Graphics Canvas, HeatPoint HeatPoint, int Radius)
        {
            // Create points generic list of points to hold circumference points
            List<Point> CircumferencePointsList = new List<Point>();
            // Create an empty point to predefine the point struct used in the circumference loop
            Point CircumferencePoint;
            // Create an empty array that will be populated with points from the generic list
            Point[] CircumferencePointsArray;
            // Calculate ratio to scale byte intensity range from 0-255 to 0-1
            float fRatio = 1F / Byte.MaxValue;
            // Precalulate half of byte max value
            byte bHalf = Byte.MaxValue / 2;
            // Flip intensity on it's center value from low-high to high-low
            int iIntensity = (byte)(HeatPoint.Intensity - ((HeatPoint.Intensity - bHalf) * 2));
            // Store scaled and flipped intensity value for use with gradient center location
            float fIntensity = iIntensity * fRatio;
            // Loop through all angles of a circle
            // Define loop variable as a double to prevent casting in each iteration
            // Iterate through loop on 10 degree deltas, this can change to improve performance
            for (double i = 0; i <= 360; i += 10)
            {
                // Replace last iteration point with new empty point struct
                CircumferencePoint = new Point();
                // Plot new point on the circumference of a circle of the defined radius
                // Using the point coordinates, radius, and angle
                // Calculate the position of this iterations point on the circle
                CircumferencePoint.X = Convert.ToInt32(HeatPoint.X + Radius * Math.Cos(ConvertDegreesToRadians(i)));
                CircumferencePoint.Y = Convert.ToInt32(HeatPoint.Y + Radius * Math.Sin(ConvertDegreesToRadians(i)));
                // Add newly plotted circumference point to generic point list
                CircumferencePointsList.Add(CircumferencePoint);
            }
            // Populate empty points system array from generic points array list
            // Do this to satisfy the datatype of the PathGradientBrush and FillPolygon methods
            CircumferencePointsArray = CircumferencePointsList.ToArray();
            // Create new PathGradientBrush to create a radial gradient using the circumference points
            PathGradientBrush GradientShaper = new PathGradientBrush(CircumferencePointsArray);
            // Create new color blend to tell the PathGradientBrush what colors to use and where to put them
            ColorBlend GradientSpecifications = new ColorBlend(3);
            // Define positions of gradient colors, use intesity to adjust the middle color to
            // show more mask or less mask
            GradientSpecifications.Positions = new float[3] { 0, fIntensity, 1 };
            // Define gradient colors and their alpha values, adjust alpha of gradient colors to match intensity
            GradientSpecifications.Colors = new Color[3]
            {
            Color.FromArgb(0, Color.White),
            Color.FromArgb(HeatPoint.Intensity, Color.Black),
            Color.FromArgb(HeatPoint.Intensity, Color.Black)
            };
            // Pass off color blend to PathGradientBrush to instruct it how to generate the gradient
            GradientShaper.InterpolationColors = GradientSpecifications;
            // Draw polygon (circle) using our point array and gradient brush
            Canvas.FillPolygon(GradientShaper, CircumferencePointsArray);
        }
        private double ConvertDegreesToRadians(double degrees)
        {
            double radians = (Math.PI / 180) * degrees;
            return (radians);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

        }
    }
    public struct HeatPoint
    {
        public int X;
        public int Y;
        public byte Intensity;
        public HeatPoint(int iX, int iY, byte bIntensity)
        {
            X = iX;
            Y = iY;
            Intensity = bIntensity;
        }
    }
}

