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

namespace TeacherGUI{
    public partial class Form1 : Form{
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
            try{
                //initialize sockets
                tcpSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
                udpSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
                outStream = new byte[10025];

                Console.WriteLine("trying to connect");
                tcpSocket.Connect(ConfigurationManager.AppSettings["IP"], int.Parse(ConfigurationManager.AppSettings["PORT"].ToString()));
                tcpSocket.ReceiveTimeout = 2000;
                String returndata="";
                
                //Wait for the start char, ';'
                while (returndata != ";") {
                    inStream = new byte[tcpSocket.Available];
                    tcpSocket.Receive(inStream);
                    returndata = Encoding.ASCII.GetString(inStream);
                }
                Console.WriteLine("Connected!");
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
                this.Close();
                return;
            }
            

            // Create new memory bitmap the same size as the picture box
            bMap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            // Call CreateIntensityMask, give it the memory bitmap, and use it's output to set the picture box image
            pictureBox1.Image = CreateIntensityMask(bMap, HeatPoints);

            
            listView1.View = View.Details;
            listView1.CheckBoxes = true;
            // Display grid lines.
            listView1.GridLines = true;
            
            // Create columns for the items and subitems.
            // Width of -2 indicates auto-size.
            listView1.Columns.Add("Student Name", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Student Email", -2, HorizontalAlignment.Left);
            listView1.Columns.Add("Current Focused Window", -2, HorizontalAlignment.Left);

        }

        //Update the bitmap in the pictureBox object
        public void UpdateMap() {
            while (true)
            {
                Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["UPDATESPEED"]));
                bMap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
                foreach (Student student in classList) {
                    if (editClassSync)
                        break;
                    HeatPoints.Add(student.StHeatPoint);
                }
                pictureBox1.Image = CreateIntensityMask(bMap, HeatPoints);
                
            }
        }


        //Thread to listen for packets on the UDP socket
        public void ListenForPackets(){
            string packetText;
            string name = "", posX = "", posY = "", time = "", app = "";
            int index = 0;
            while (true)
            {
                //Console.WriteLine("Listening for next packet");
                if (udpSocket.Available > 0){
                    inStream = new byte[udpSocket.Available];
                    udpSocket.Receive(inStream);
                    //Packet is in bytes so decoded it and parse each value out
                    packetText = Encoding.ASCII.GetString(inStream);
                    index = 0;
                    name = packetText.Substring(index + 1, packetText.IndexOf('#', index + 1) - 1);
                    index += name.Length + 1;
                    posX = packetText.Substring(index + 1, packetText.IndexOf('#', index + 1) - 1 - index);
                    index += posX.Length + 1;
                    posY = packetText.Substring(index + 1, packetText.IndexOf('#', index + 1) - 1 - index);
                    index += posY.Length + 1;
                    time = packetText.Substring(index + 1, packetText.IndexOf('#', index + 1) - 1 - index);
                    index += time.Length + 1;
                    //Since the app might not be included, then it has to check if it's there. '#' indeicates another peice of info
                    if (packetText.Substring(index).Contains("#"))
                        try {
                            app = packetText.Substring(index + 1, packetText.Length - index - 2 - packetText.Substring(packetText.IndexOf(';')).Length);
                        }catch(Exception e){
                            app = "";
                        }
                    else
                        app = "";

                    //Check if it's a new student or not. If they're already in the list then just update their app.
                    if (!students.Contains(name)){
                        if (listView1.InvokeRequired) {
                            editClassSync = true;
                            addStudent(name);
                        }
                    } else {
                        updateStudentApp(name,app);
                    }

                    //Since the output(pictureImage) is half of a 1920x1080 screen, 960x540, then divide it by 2
                    //Also if the input is out of bounds then set it back inside of the bounds
                    int tempX=(int)double.Parse(posX) / 2;
                    if (tempX < 0)
                        tempX = 0;
                    else if (tempX > 960)
                        tempX = 960;
                    int tempY= (int)double.Parse(posY) / 2;
                    if (tempY < 0)
                        tempY = 0;
                    else if (tempY > 960)
                        tempY = 960;

                    //create a new heatpoint a set it to the student
                    HeatPoint newHeat = new HeatPoint(tempX, tempY, byte.MaxValue );
                    for(int i= 0; i < classList.Count; i++)
                    {
                        if (classList[i].getName()==name)
                            classList[i].setHP(newHeat);
                        
                    }
                        
                }
            }
        }
        //Add student to the list, it also has to make sure that when the map updates it dosen't access the list while it's being edited
        public void addStudent(string name) {
            if (listView1.InvokeRequired){
                Invoke((MethodInvoker)delegate {  this.addStudent(name); });
            } else { 
                students.Add(name);
                ListViewItem item = new ListViewItem(databaseController.getFullName(name));
                item.SubItems.Add(name);
                item.SubItems.Add("");
                listView1.Items.Add(item);
                classList.Add(new Student(name));
                editClassSync = false;
            }
        }

        //Update the listview to change the currently viewed app for the student
        public void updateStudentApp(string name,string app) {
            if (listView1.InvokeRequired) {
                Invoke((MethodInvoker)delegate { this.updateStudentApp(name,app); });
            } else {
                listView1.Items[students.IndexOf(name)].SubItems[2].Text = app;
                
            }


        }
        //Heartbeat for the server
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

        //close for the heartbeat method, as the heartbeat is on a seperate thread
        private void invokeClose() {
            if (this.InvokeRequired) {
                Invoke((MethodInvoker)delegate { this.invokeClose(); });
            } else {
                this.Close();

            }
        }
        
        //Course selection screen
        private void button1_Click(object sender, EventArgs e){
            this.Close();
        }
        //When the form closes it needs to stop the threads and discconect from the server
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            tcpThread.Abort();
            udpThread.Abort();
            mapThread.Abort();
            tcpSocket.Close();
            udpSocket.Close();
            teacherHome.Show();
            
        }


        //This draws the heatpoints onto the canvas
        //http://dylanvester.com/2015/10/creating-heat-maps-with-net-20-c-sharp/ 
        private List<HeatPoint> HeatPoints = new List<HeatPoint>();
       
        private Bitmap CreateIntensityMask(Bitmap bSurface, List<HeatPoint> aHeatPoints)
        {
            // Create new graphics surface from memory bitmap
            Graphics DrawSurface = Graphics.FromImage(bSurface);
            // Set background color to white so that pixels can be correctly colorized
            DrawSurface.Clear(Color.White);
            // Traverse heat point data and draw masks for each heat point
            try{
                foreach (Student student in classList)
                    DrawHeatPoint(DrawSurface, student.getHP(), 25);
            }
            catch(Exception e){

            }
            return bSurface;
        }

        private void DrawHeatPoint(Graphics Canvas, HeatPoint HeatPoint, int Radius){
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

        private double ConvertDegreesToRadians(double degrees){
            double radians = (Math.PI / 180) * degrees;
            return (radians);
        }
        //End of dylanvester.com code

        
    }
    //Structures for keeping up with specific students and their heatpoints
    public struct HeatPoint{
        public int X;
        public int Y;
        public byte Intensity;
        public HeatPoint(int iX, int iY, byte bIntensity){
            X = iX;
            Y = iY;
            Intensity = bIntensity;
        }
    }
    public class Student{
        public HeatPoint StHeatPoint;
        public String name;
        public Student(String name1){
            this.name = name1;
            StHeatPoint = new HeatPoint();
        }
        public String getName(){
            return this.name;
        }
        public void setHP(HeatPoint heat){
            this.StHeatPoint = heat;
        }
        public HeatPoint getHP(){
            return this.StHeatPoint;
        }
    }

}

