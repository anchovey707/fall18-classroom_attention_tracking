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
        Validator validUserCheck = new Validator();
        //Sobject of stream class
        private FileStream globalFileStream;
        // add to config file 
        public Int64 byteCountOfStream;
        iFocus tracker;
        //delegate void ObjectDelegate(Object source, System.Timers.ElapsedEventArgs e);
        public static String surveyAnswer = "100";

        public Form1()
        {
            Debug.WriteLine("Form1");
            Init();
            InitializeComponent();
            SpawnThread();
        }

        private void Init()
        {
            Debug.WriteLine("Init");
            //adminLogin = validUserCheck.DetermineAdminLogin();//if Admin set adminLogin to true, skip further checks
            adminLogin = true;

            if (!adminLogin) validUserCheck.ValidUserNameCheck();// checks if valid user - will system exit if not
           
            SystemEvents.SessionEnding += new SessionEndingEventHandler(SystemEvents_SessionEnding); //EVENThandler for logout or any type of shutdown

            //store file in local H: Folder, not AppData - For Non-Test scenarios
            String systemFileLocation = "D:\\Users\\" + Environment.UserName + "\\Desktop\\eyes";
            globalFileStream = new FileStream(systemFileLocation, FileMode.OpenOrCreate, FileAccess.ReadWrite);

            Debug.WriteLine("Pre iFocus");
            //Find which windows are open (duh)
            
            tracker = new iFocus(globalFileStream);

            //initialize serverConnector and determines if valid
            //serverConnector = new ServerConnection(globalFileStream);
            //Handles Validation in ServerConnection need this after establish connection to get day/time
            if (!adminLogin) serverConnector.valid.ValidateUserBasedOnDaysTimes(serverConnector.Timestamp,serverConnector);
            
            iFocus.GetOpenWindows();//get initial windows up 
            //UI location top left 
            Point p = new Point(0, 0);
            this.Location = p;

            Debug.WriteLine("Init End");
        }
   

        
       
        // Will eventually move this thread spawn to a new thread class 
        //starts thread which handles tracker writing to file and serverConnector sending data to server
        private void SpawnThread()
        {
            Thread thread = new Thread(new ThreadStart(SampleIFocusAndWriteToServer));
            thread.Start();
        }
        //Actual work being done by iFocus and ServerConnection
        private void SampleIFocusAndWriteToServer()
        {
            while (true)
            {//Add this note to change log
                tracker.GazeData(tracker.Tracker);// want to still collect if server connection not made
                                                  //serverConnector.SendDataToServer(tracker.ByteCount); // next successful server connection we dump data that was not originally sent
                Debug.WriteLine(tracker.ByteCount);
            }
        }
        

        public FileStream GlobalFileStream { get => globalFileStream; set => globalFileStream = value; }
        public bool AdminLogin { get => adminLogin; set => adminLogin = value; }

        //Close everything when session endss (Logout event - SessionEndingEventArgs)
        public void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            try
            {
                logger.Info("User has properly logged off " + DateTime.Now + "\n");
                serverConnector.TerminateProtocol();
            }
            catch
            {
                //Only if connection not made yet, exit anyway
                // or tracker not set up, exit anyway as well
            }
            finally
            {
                Environment.Exit(1);
            }
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

        //Procs whenever the form's state is changed
        private void Form1_Move(object sender, EventArgs e)
        {
            //Only change should be when minimized
            if (this.WindowState == FormWindowState.Minimized)
            {
                formDownTimer.Stop();
                //Most for testing, counter is not needed for final version
                counter = 0;
                //Interval can be directly set to desired minutes, current Interval is every minute
                formDownTimer.Interval = 60000;
                //formDownTimer.Interval = 10000;
                formDownTimer.Elapsed += OnTimedEvent;
                formDownTimer.Start();
                this.Hide();
                //Testing statement, for proof of it working
                /*if (firstTime)
                {
                    firstTime = false;
                    notifyIcon1.Visible = true;
                    notifyIcon1.ShowBalloonTip(1000, "Reminder!", "Don't forget to answer the survey! Click this to answer now.", ToolTipIcon.Info);
                } */
            }

            if (this.WindowState == FormWindowState.Normal)
            {
                formUpTimer.Stop();
                formUpTimer.Interval = 60000;
                formUpTimer.Elapsed += otherOnTimedEvent;
                formUpTimer.Start();

            }
        }

        //When the icon is clicked, show the form again.
        /*private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;

        }*/

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

        private void otherOnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (InvokeRequired)
            {
                this.Invoke((MethodInvoker)delegate { surveyAnswer = "99"; });
                this.Invoke((MethodInvoker)delegate { this.WindowState = FormWindowState.Minimized; });

            }
            else
            {
                surveyAnswer = "99";
                Debug.Write("So rude");
                this.WindowState = FormWindowState.Minimized;

            } 
        }

        //When balloon tip is clicked, bring up the form.
        /*private void notifyIcon1_BalloonTipClicked(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            notifyIcon1.Visible = false;
        }*/


        private void sendButton_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                //Do something, write to a file maybe?
                surveyAnswer = "5";
                //For testing
                Debug.Write("Level 5");
                this.WindowState = FormWindowState.Minimized;
            }
            else if (radioButton2.Checked)
            {
                //Do something, write to a file maybe?
                surveyAnswer = "4";
                //For testing
                Debug.Write("Level 4");
                this.WindowState = FormWindowState.Minimized;
            }
            else if (radioButton3.Checked)
            {
                //Do something, write to a file maybe?
                surveyAnswer = "3";
                //For testing
                Debug.Write("Level 3");
                this.WindowState = FormWindowState.Minimized;
            }
            else if (radioButton4.Checked)
            {
                //Do something, write to a file maybe?
                surveyAnswer = "2";
                //For testing
                Debug.Write("Level 2");
                this.WindowState = FormWindowState.Minimized;
            }
            else if (radioButton5.Checked)
            {
                //Do something, write to a file maybe?
                surveyAnswer = "1";
                //For testing
                Debug.Write("Level 1");
                this.WindowState = FormWindowState.Minimized;
            }
            else if(radioButton6.Checked)
            {
                surveyAnswer = "0";
                Debug.Write("Level 0");
                this.WindowState = FormWindowState.Minimized;
            }
            else 
            {
                //Make the user select something
                //surveyAnswer = "No answer selected";
                //For testing
                Debug.Write("You must construct additional pylons");
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }
        }




    }

}
