using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TobiiForm
{
    public class Validator
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
       
        /*
      Gets timestamp from server and does necessary checking
      Checks Hours-UserHashkey and determines if Application.Exit necessary
      Exit handled by TerminateProtocol function 
      */
        public void ValidateUserBasedOnDaysTimes(String Timestamp, ServerConnection serverConnectionToClose)
        {
            List<String> validConnectionDays = new List<String>(ConfigurationManager.AppSettings["DAYS"].Split(new char[] { ';' })); // gets valid days
            String[] timestampSplit = Timestamp.Split(new char[] { '-' });

            //Gets timespans of begin time and end time
            List<String> validConnectionTime = new List<String>(ConfigurationManager.AppSettings["TIMES"].Split(new char[] { ';' })); // gets times
            String[] startTime = validConnectionTime[0].Split(new char[] { '-' });
            TimeSpan start = new TimeSpan(Convert.ToInt32(startTime[0]), Convert.ToInt32(startTime[1]), 0); //14:00

            String[] endTime = validConnectionTime[1].Split(new char[] { '-' });
            TimeSpan end = new TimeSpan(Convert.ToInt32(endTime[0]), Convert.ToInt32(endTime[1]), 0); // Dont want to collect if student too late??

            //timespan from server 
            TimeSpan current = new TimeSpan(Convert.ToInt32(timestampSplit[1]), Convert.ToInt32(timestampSplit[2]), 0);
            //Get time Difference
            TimeSpan difference = start - current;

            //Disconnect if not proper day or after end time or loged in an hour early
            if (!validConnectionDays.Contains(timestampSplit[0]) || current > end || difference.Hours > 0)
            {
                logger.Info("User has privilege but wrong time : " + DateTime.Now + "\n");
                Process[] TobiiTray = Process.GetProcessesByName("Tobii.EyeX.Tray");
                if (TobiiTray.Length > 0)
                {
                    TobiiTray[0].Close(); // Close tobii tray on logged in user 
                }
                serverConnectionToClose.TerminateProtocol();
            }
            if (current < start)
            {//Can only be minutes now that we did hour check
                Thread.Sleep(difference.Minutes * 60 * 1000); // sleeps X minutes until class start
            }
            logger.Info("Non-Admin-User has privilege and we are now collecting their data " + DateTime.Now + "\n");
            //Wait for sleep time or continue process of Init protocol
        }


        //Checks Admins first, if true skip all further checks
        public Boolean DetermineAdminLogin()
        {
            String username = Environment.UserName;
            List<String> admins = new List<String>(ConfigurationManager.AppSettings["ADMIN"].Split(new char[] { ';' }));//Config file
            if (admins.Contains(username))
            {
                logger.Info(" Admin has logged in " + DateTime.Now + "\n");

                return true;
            }

            return false;
        }

        //Initial username check
        public void ValidUserNameCheck()
        {

            List<String> usersHashCodes = new List<String>(ConfigurationManager.AppSettings["USERS"].Split(new char[] { ';' }));//gets user hashes
            //List<Int32> intPaths = usersHashCodes.Select(int.Parse).ToList(); // converts to ints
            String logedInUsername = Environment.UserName;//store username
            
            if (!usersHashCodes.Contains(ObfuscateStrings(logedInUsername)))
            {
                logger.Info("User Loged in does not have privilege to use tobii " + DateTime.Now + "\n");
                Process[] engine = Process.GetProcessesByName("Tobii.EyeX.Engine");
                Process[] tray = Process.GetProcessesByName("Tobii.EyeX.Tray");
                if (engine.Length > 0)
                {
                    engine[0].Kill(); // Turns off the lazer bheams - needs to be changed to solution from documentation
                }
                if (tray.Length > 0)
                {
                    tray[0].Kill(); // Turns off the lazer bheams - needs to be changed to solution from documentation
                }
                //Environment.Exit(1);
            }
        }


        //Poor excuse of hashing but quick
        static String ObfuscateStrings(String s)
        {
            String fhalf = s.Substring(s.Length / 2);
            String lhalf = s.Substring(0, s.Length / 2);
            return fhalf + lhalf;
        }

    }
}
