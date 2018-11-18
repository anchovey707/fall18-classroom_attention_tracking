using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tobii.Research;
using HWND = System.IntPtr;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using System.Collections;
using NLog;

namespace TobiiForm
{
    class iFocus
    {
        //Instantiate array of trackers
        static EyeTrackerCollection eyeTrackers;
        //Actual tracker object
        IEyeTracker tracker;
        //Data to be written to file
        static String currentData;
        //Logger
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //List of open tabs
        static List<Window> openTabs;
        //Timer if necessary
        static Int32 byteCount;
        static FileStream Form1FileStreamObject;
        Process tobiiEyeTrackerProcess;
        static  JsonObject jSonPointDataObject;
        //Avoid hardcoding screen dimensions - Needed as tobii returns values 0-1 x,y
        Int32 screenWidth = Screen.PrimaryScreen.Bounds.Width;
        Int32 screenHeight = Screen.PrimaryScreen.Bounds.Height;
        String surveyAnswers;
        String previousAnswer = "100";
    
        //Constructor
        public iFocus(FileStream fs)
        {
            Debug.Write("iFocus - Started!");
            SetEyeTrackers();
            Form1FileStreamObject = fs;
            tracker = eyeTrackers[0];
        }

        //get 'all' of the eyetrackers (should only be 1) and puts it into array
        private void SetEyeTrackers()
        {
            eyeTrackers = EyeTrackingOperations.FindAllEyeTrackers();
            if (eyeTrackers.Count == 0)
            {
                logger.Fatal( this.GetType().Name + " Class, Eye Tracker not found " + DateTime.Now + "\n");
                TobiiEyeTrackerProcess.Close();
                //Environment.Exit(-1);
            }
            
        }
        
        List<JsonObject> jSonArray;
        //Receives the gaze data
        public void GazeData(IEyeTracker eyeTracker)
        {
            GetOpenWindows();
            ByteCount = 0;
            currentData = "";
            jSonArray = new List<JsonObject>();
            // Start listening to gaze data.
            System.Threading.Thread.Sleep(1000);
            eyeTracker.GazeDataReceived += EyeTracker_GazeDataReceived;
            // Wait for some data to be received.
            System.Threading.Thread.Sleep(250);
            // Stop listening to gaze data.
            eyeTracker.GazeDataReceived -= EyeTracker_GazeDataReceived;
            //Debug.WriteLine(ByteCount);
            currentData = JsonConvert.SerializeObject(jSonArray.ToArray(), Formatting.Indented);
            PrintToFile(currentData);
        }

        

        private void EyeTracker_GazeDataReceived(object sender, GazeDataEventArgs e)
        {
            // Left eye coordinates multiplied by computer width and height
            //Remember to change 1680 based on monitor size
            float x = (e.LeftEye.GazePoint.PositionOnDisplayArea.X) * screenWidth;
            //Remember to change 1050 based on monitor size
            float y = (e.LeftEye.GazePoint.PositionOnDisplayArea.Y) * screenHeight;
            String current = "\"" + x.ToString() + ", " + y.ToString() + "\"";
            Debug.WriteLine("X:" + x.ToString() + ", Y:" + y.ToString() + "\"");
            // check NaN
            if (Double.IsNaN(e.LeftEye.GazePoint.PositionOnDisplayArea.X))
            {
                //X and Y coordinates
                jSonPointDataObject = new JsonObject(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
                jSonArray.Add(jSonPointDataObject);

                return;
            }

            //REMOVE FOR FINAL - ONLY NEED FOR DOUBLE MONITOR
            //x += 1680;

            PointF center = new PointF(x, y);

            String viewingBrowser = "";
            //this.updateBox(center);
            foreach (Window w in openTabs)
            {

                Rect tempRect = w.GetRectangleInfo();
                //Check for what is being viewed
                if (tempRect.Left < x && x < tempRect.Right && tempRect.Top < y && tempRect.Bottom > y)
                {
                    //Current browser tab being viewed
                    viewingBrowser = ", " + w.GetTabInfo();
                    break;
                }

            }
            //currentData += DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + ", " + current + viewingBrowser + "\n";
            String xString = x.ToString();
            String yString = y.ToString();
            surveyAnswers = Form1.surveyAnswer;

            jSonPointDataObject = new JsonObject(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), xString, yString, 
                viewingBrowser, surveyAnswers);
            jSonArray.Add(jSonPointDataObject);
            previousAnswer = surveyAnswers;
            if(!previousAnswer.Equals("100"))
            {
                Form1.surveyAnswer = "100";
            }
        }

        //Print to file
        // Seek - So always appends to end, did not immediately find append to end so went with it
        private void PrintToFile(String Data)
        {
            byte[] tempOutputBytes = Encoding.ASCII.GetBytes(Data);
            Form1FileStreamObject.Seek(0, SeekOrigin.End);
            Form1FileStreamObject.Write(tempOutputBytes, 0, tempOutputBytes.Length);
            ByteCount += tempOutputBytes.Length;
        }


        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        public static IDictionary<HWND, string> GetOpenWindows()
        {
            HWND shellWindow = GetShellWindow();
            Dictionary<HWND, string> windows = new Dictionary<HWND, string>();
            Rect CurrentWindow = new Rect();
            List<Window> tempWindowList = new List<Window>();
            EnumWindows(delegate (HWND hWnd, int lParam)
            {
                GetWindowRect(hWnd, ref CurrentWindow);
                if (hWnd == shellWindow)
                    return true;
                if (!IsWindowVisible(hWnd))
                    return true;

                int length = GetWindowTextLength(hWnd);
                if (length == 0)
                    return true;

                StringBuilder builder = new StringBuilder(length);

                GetWindowText(hWnd, builder, length + 1);
                windows[hWnd] = builder.ToString();
                // Extra Conditions for screens that still read visable though are in fact, not....lol.
                if (IsWindowVisible(hWnd) && (CurrentWindow.Left > -10 && CurrentWindow.Bottom > 0))
                {
                    //Remove further annoyances
                    if (!builder.ToString().Equals("Microsoft Store") && !builder.ToString().Equals("Photos"))//&& !builder.ToString().Equals("Windows Shell Experience Host"))

                        //THIS RETURNS IN ORDER - Z-Score Redundant??
                        //not sure if best way to instantiate object in c#
                        tempWindowList.Add(new Window(builder.ToString(), CurrentWindow));
                }

                return true;

            }, 0);
            openTabs = tempWindowList;
            return windows;
        }

        static List<String> names = new List<String>();
        //Getters Setters
        public IEyeTracker Tracker { get => tracker; set => tracker = value; }
        public Int32 ByteCount { get => byteCount; set => byteCount = value; }
        public Process TobiiEyeTrackerProcess { get => tobiiEyeTrackerProcess; set => tobiiEyeTrackerProcess = value; }

        private delegate bool EnumWindowsProc(HWND hWnd, int lParam);
        [DllImport("USER32.DLL")]
        private static extern bool EnumWindows(EnumWindowsProc enumFunc, int lParam);
        [DllImport("USER32.DLL")]
        private static extern int GetWindowText(HWND hWnd, StringBuilder lpString, int nMaxCount);
        [DllImport("USER32.DLL")]
        private static extern int GetWindowTextLength(HWND hWnd);
        [DllImport("USER32.DLL")]
        private static extern bool IsWindowVisible(HWND hWnd);
        [DllImport("USER32.DLL")]
        private static extern IntPtr GetShellWindow();
        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(String s, String app);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr wnd, ref Rect rectangle);
        [DllImport("User32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);
        [DllImport("User32.dll")]
        public static extern void ReleaseDC(IntPtr hwnd, IntPtr dc);
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);



        //Start 4c Software - Removed
        // public void RunTobiiSoftware()
        // {
        //Run tobii software as it will be off and we must toggle it on - proper path is important here!
        //    try
        //    {
        //        Process[] TobiiTray = Process.GetProcessesByName("Tobii.EyeX.Tray");
        //        if(TobiiTray.Length > 0) {
        //            TobiiEyeTrackerProcess = TobiiTray[0]; // Assign to tobiiprocess so it can be closed at log-off according to IT guys wishes
        //            return; // Tobii Already Running should not be the case
        //         }
        //        string executablePath = Path.GetFullPath("C:\\Program Files (x86)\\Tobii\\Tobii EyeX Interaction\\Tobii.EyeX.Tray.exe");
        //        TobiiEyeTrackerProcess = new Process();
        //        TobiiEyeTrackerProcess.StartInfo.FileName = executablePath;
        //        TobiiEyeTrackerProcess.Start();
        //        System.Threading.Thread.Sleep(15000);
        //    }
        //    catch(Exception e)
        //    {
        //        logger.Fatal("Unable to start Tobii4c Process" + DateTime.Now + "\n");
        //        Environment.Exit(-1);
        //    }
        //}


    }
}
