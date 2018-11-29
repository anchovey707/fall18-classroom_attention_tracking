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
using System.Configuration;

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
        //List of open tabs
        static List<Window> openTabs;
        //Timer if necessary
        static Int32 byteCount;
        Process tobiiEyeTrackerProcess;
        static  JsonObject jSonPointDataObject;
        //Avoid hardcoding screen dimensions - Needed as tobii returns values 0-1 x,y
        Int32 screenWidth = Screen.PrimaryScreen.Bounds.Width;
        Int32 screenHeight = Screen.PrimaryScreen.Bounds.Height;
        int eyeTrackerWaitMax = int.Parse(ConfigurationManager.AppSettings["TrackerWait"].ToString());int eyeTrackerWait=0;

        ServerConnection server;
    
        //Constructor
        public iFocus(ServerConnection server) {
            Debug.Write("iFocus - Started!");
            SetEyeTrackers();
            tracker = eyeTrackers[0];
            this.server = server;
            this.tracker.GazeDataReceived += EyeTracker_GazeDataReceived;
        }

        //get 'all' of the eyetrackers (should only be 1) and puts it into array
        private void SetEyeTrackers()
        {
            eyeTrackers = EyeTrackingOperations.FindAllEyeTrackers();
            if (eyeTrackers.Count == 0)
            {
                Console.WriteLine("Eye tracker failed");
                TobiiEyeTrackerProcess.Close();
                Environment.Exit(-1);
            }
            
        }
        
        //GAze data event handler, gets the XY,app and time, then sends it to the server
        private void EyeTracker_GazeDataReceived(object sender, GazeDataEventArgs e) {
            if (eyeTrackerWait++ > eyeTrackerWaitMax) {
                // Left eye coordinates multiplied by computer width and height
                //Remember to change 1680 based on monitor size
                float x = (e.LeftEye.GazePoint.PositionOnDisplayArea.X) * screenWidth;
                //Remember to change 1050 based on monitor size
                float y = (e.LeftEye.GazePoint.PositionOnDisplayArea.Y) * screenHeight;
                String current = "\"" + x.ToString() + ", " + y.ToString() + "\"";
                Debug.WriteLine("X:" + x.ToString() + ", Y:" + y.ToString() + "\"");
                // check NaN
                if (Double.IsNaN(e.LeftEye.GazePoint.PositionOnDisplayArea.X))
                    return;

                PointF center = new PointF(x, y);
                GetOpenWindows();
                String viewingBrowser = "";
                foreach (Window w in openTabs) {
                    Console.WriteLine(w.GetTabInfo());
                    Rect tempRect = w.GetRectangleInfo();
                    //Check for what is being viewed
                    if (tempRect.Left < x && x < tempRect.Right && tempRect.Top < y && tempRect.Bottom > y) {
                        //Current browser tab being viewed
                        viewingBrowser = w.GetTabInfo();
                       break;
                    }
                    

                }
                String xString = x.ToString();
                String yString = y.ToString();

                //jSonPointDataObject = new JsonObject(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), xString, yString,viewingBrowser);
                //jSonArray.Add(jSonPointDataObject);
                currentData = "#" + Environment.UserName + "#" + xString + "#" + yString
                    + "#" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "#" + viewingBrowser + ";";
                server.SendDataToServer(Encoding.ASCII.GetBytes(currentData));
                eyeTrackerWait = 0;
            }
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



       


    }
}
