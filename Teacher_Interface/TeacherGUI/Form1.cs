using System;
using System.Collections.Generic;
using System.Configuration;
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
        //static TcpClient tcpSocket = new TcpClient();
        public Socket tcpSocket,udpSocket;
        byte[] outStream;
        byte[] inStream;
        List<Student> classList=new List<Student>();
        Form teacherHome;
        Thread tcpThread,udpThread,mapThread;
        Bitmap bMap;

        List<String> students = new List<String>();
        bool editClassSync = false;
        public Form1(Form f,int course)
        {
            teacherHome = f;
            InitializeComponent();
            try
            {
                tcpSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                udpSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);

                outStream = new byte[10025];

                Console.WriteLine("trying to connect");
                tcpSocket.Connect(databaseController.databaseIP, int.Parse(ConfigurationManager.AppSettings["PORT"].ToString()));
                tcpSocket.ReceiveTimeout = 2000;
                Console.WriteLine("waiting for starting char");
                String returndata="";
                
                //Wait for the start char, ';'
                while (returndata != ";") {
                    inStream = new byte[tcpSocket.Available];
                    tcpSocket.Receive(inStream);
                    returndata = Encoding.ASCII.GetString(inStream);
                }
                Console.WriteLine("got start char");
                outStream = Encoding.ASCII.GetBytes("#" + databaseController.username + "#" + course + ";");
                
                //send my name and course that I want to stream
                tcpSocket.Send(outStream);
                Console.WriteLine("waiting for port info");
                
                //wait for port number
                inStream = new byte[0];
                returndata ="";
                while (!returndata.Contains(";")) {
                    inStream = new byte[tcpSocket.Available];
                    tcpSocket.Receive(inStream);
                    returndata += Encoding.ASCII.GetString(inStream);
                }

                returndata = returndata.Substring(returndata.IndexOf(":") + 1, returndata.Length - returndata.IndexOf(":") - 2);
                
                udpSocket.Bind(new IPEndPoint(IPAddress.Any,int.Parse(returndata)));
                udpThread = new Thread(ListenForPackets);udpThread.Start();
                tcpThread = new Thread(heartBeat);tcpThread.Start();
                mapThread = new Thread(UpdateMap);mapThread.Start();
            }
            catch (Exception e){
                
                Console.WriteLine(e.StackTrace);
                this.Close();
                return;
            }




            // Set the view to show details.


            // Create new memory bitmap the same size as the picture box
            bMap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
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
            //listView1.Items.AddRange(new ListViewItem[] { item1, item2, item3 });
            //listView2.Items.AddRange(new ListViewItem[] { chrome,mozilla });


        }

        public void UpdateMap() {
            while (true)
            {
                Thread.Sleep(200);
                Console.WriteLine("Map");
                //HeatPoints.Clear();
                bMap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                foreach (Student student in classList) {
                    if (editClassSync)
                        break;
                    HeatPoints.Add(student.StHeatPoint);
                }
                pictureBox1.Image = CreateIntensityMask(bMap, HeatPoints);
                
            }
        }



        public void ListenForPackets()
        {
            string packetText;
            string name = "", posX = "", posY = "", time = "", app = "";
            int index = 0;
            while (true)
            {
                //Console.WriteLine("Listening for next packet");
                if (udpSocket.Available > 0)
                {
                    inStream = new byte[udpSocket.Available];

                    udpSocket.Receive(inStream);
                    //Packet is in bytes so decoded it
                    packetText = Encoding.ASCII.GetString(inStream);
                    Console.WriteLine("Packet=" + packetText);
                    index = 0;
                    name = packetText.Substring(index + 1, packetText.IndexOf('#', index + 1) - 1);
                    index += name.Length + 1;
                    posX = packetText.Substring(index + 1, packetText.IndexOf('#', index + 1) - 1 - index);
                    index += posX.Length + 1;
                    posY = packetText.Substring(index + 1, packetText.IndexOf('#', index + 1) - 1 - index);
                    index += posY.Length + 1;
                    time = packetText.Substring(index + 1, packetText.IndexOf('#', index + 1) - 1 - index);
                    index += time.Length + 1;
                    if (packetText.Substring(index).Contains("#"))
                        try {
                            app = packetText.Substring(index + 1, packetText.Length - index - 2 - packetText.Substring(packetText.IndexOf(';')).Length);
                        }catch(Exception e){
                            app = "";
                        }
                    else
                        app = "";
                    if (!students.Contains(name)){
                        if (listView1.InvokeRequired) {
                            editClassSync = true;
                            addStudent(name);
                        }
                    } else {
                        updateStudentApp(name,app);
                    }
                    HeatPoint newHeat = new HeatPoint(int.Parse(posX), int.Parse(posY), byte.MaxValue );
                    Console.WriteLine(newHeat.X + " ;" + newHeat.Y);
                    for(int i= 0; i < classList.Count; i++)
                    {
                        if (classList[i].getName()==name)
                        {
                            classList[i].setHP(newHeat);
                            Console.WriteLine(newHeat.X);
                            Console.WriteLine(classList[i].getName());
                            Console.WriteLine(classList[0].getName());

                            Console.WriteLine(classList[0].getHP().X);
                            Console.WriteLine(classList[i].getHP().X);
                            Console.WriteLine(classList.ToString());
                        }
                    }
                        
                }
            }
        }

        public void addStudent(string name) {
            Console.WriteLine(listView1.InvokeRequired);
            if (listView1.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { Console.WriteLine("Invoke Required!!!!!!!!!!!!!!!!!!!!!!!!!1"); this.addStudent(name); });
            }
            else
            {
                students.Add(name);
                Console.WriteLine("Added " + name + " to the class");
                ListViewItem item = new ListViewItem(databaseController.getFullName(name));
                item.SubItems.Add(name);
                item.SubItems.Add("");
                listView1.Items.Add(item);
                classList.Add(new Student(name));
                editClassSync = false;
            }
        }
        
        public void addStudentOld(string name)
        {
            Console.WriteLine(listView1.InvokeRequired);
            if (listView1.InvokeRequired)
            {
                Invoke((MethodInvoker)delegate { Console.WriteLine("Invoke Required!!!!!!!!!!!!!!!!!!!!!!!!!1"); this.addStudent(name); });
            }
            else
            {
                students.Add(name);
                Console.WriteLine("Added " + name + " to the class");
                ListViewItem item = new ListViewItem(databaseController.getFullName(name));
                item.SubItems.Add(name);
                item.SubItems.Add("");
                listView1.Items.Add(item);
                classList.Add(new Student(name));
            }
        }
        public void updateStudentApp(string name,string app) {
            if (listView1.InvokeRequired) {
                Invoke((MethodInvoker)delegate { this.updateStudentApp(name,app); });
            } else {
                listView1.Items[students.IndexOf(name)].SubItems[2].Text = app;
                
            }


        }

        public void heartBeat() {
            while (true) {
                Thread.Sleep(1000);
                try {
                    tcpSocket.Send(new byte[] { 0 });
                }catch(Exception e) {
                    invokeClose();
                }
            }
        }


        private void invokeClose() {
            if (this.InvokeRequired) {
                Invoke((MethodInvoker)delegate { this.invokeClose(); });
            } else {
                this.Close();

            }
        }
        

        private void button1_Click(object sender, EventArgs e){
            this.Close();
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            //need to stop thread as well
            tcpThread.Abort();
            udpThread.Abort();
            mapThread.Abort();
            tcpSocket.Close();
            udpSocket.Close();
            teacherHome.Show();
            
        }
        private List<HeatPoint> HeatPoints = new List<HeatPoint>();
       
        private Bitmap CreateIntensityMask(Bitmap bSurface, List<HeatPoint> aHeatPoints)
        {
            // Create new graphics surface from memory bitmap
            Graphics DrawSurface = Graphics.FromImage(bSurface);
            // Set background color to white so that pixels can be correctly colorized
            DrawSurface.Clear(Color.White);
            // Traverse heat point data and draw masks for each heat point
            try
            {
                foreach (Student student in classList)
                {
                    // Render current heat point on draw surface
                    Console.WriteLine("Im in the create map");
                    Console.WriteLine(student.getHP().X);
                    Console.WriteLine(student.getHP().Y);
                    DrawHeatPoint(DrawSurface, student.getHP(), 25);
                }
            }
            catch(Exception e)
            {

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
    public class Student
    {
        public HeatPoint StHeatPoint;
        public String name;
        public Student(String name1)
        {
            this.name = name1;
            StHeatPoint = new HeatPoint();
        }
        public String getName()
        {
            return this.name;
        }
        public void setHP(HeatPoint heat)
        {
            Console.WriteLine(heat.X);
            this.StHeatPoint = heat;
            Console.WriteLine(this.StHeatPoint.X);
        }
        public HeatPoint getHP()
        {
            return this.StHeatPoint;
        }
    }

}

