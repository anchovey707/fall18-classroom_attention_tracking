using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using System.Threading;
using System.Configuration;
using NLog;

namespace TobiiForm
{
    public partial class Form1 : Form
    {
        //For testing 
        static int counter;
        //Logger
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //Also for testing
        //static bool firstTime = true;
        bool adminLogin = false;
        //Timer object for survey
        static System.Timers.Timer formDownTimer = new System.Timers.Timer();
        static System.Timers.Timer formUpTimer = new System.Timers.Timer();
        ServerConnection serverConnector;
        // add to config file 
        public Int64 byteCountOfStream;
        iFocus tracker;

        public Form1()
        {
            Debug.WriteLine("Form1 init");
            Init();
            InitializeComponent();
            //Hide the form as we no longer need the grading scale from the previous project
            this.Hide();
        }
   
        private void Init() {
            serverConnector = new ServerConnection();
            tracker = new iFocus(serverConnector);
            iFocus.GetOpenWindows();
        }
        

        //UI BELOW
        //SYSTEM TRAY AND OTHER NEW ADDITIONS

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Show();
            Debug.WriteLine("Form loaded");
            //this.WindowState = FormWindowState.Minimized;
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        { }
        

        //Activates on every interval, counter is only for testing
        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {


            counter++;

            if (counter % 5 == 0)
            {
                //this.notifyIcon1.Visible = true;
                //this.notifyIcon1.ShowBalloonTip(1000, "Reminder!", "Don't forget to answer the survey! Click this to answer now.", ToolTipIcon.Info);
                if(InvokeRequired)
                {
                    this.Invoke((MethodInvoker)delegate { this.Show(); });
                    this.Invoke((MethodInvoker)delegate { this.WindowState = FormWindowState.Normal; });
                }
                else
                {
                    this.Show();
                    this.WindowState = FormWindowState.Normal;

                }

                
                //notifyIcon1.Visible = false;
            }
            
            //For testing
            Debug.WriteLine("Counter: " + counter);

        }

       
    }

}
