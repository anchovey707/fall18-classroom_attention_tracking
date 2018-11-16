﻿using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TobiiForm
{
    /*
    !!!!!!!!!!!!!TEST VERSION COMMENT OUT LINE 49!!!!!!!!!!!! 
    */
    //Class for connecting to the server
    public class ServerConnection
    {

        int ConnectionAttempts = 0;
        TcpClient client = new TcpClient();
        NetworkStream serverStream;
        //Access to data from config
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //Logger
        private static Logger logger = LogManager.GetCurrentClassLogger();
        FileStream Form1FileStreamObject;
        String timestamp;
        public Validator valid = new Validator(); //access to checks
        //Getters/Setters
        public NetworkStream ServerStream { get => serverStream; set => serverStream = value; }
        public TcpClient Client { get => client; set => client = value; }
        public string Timestamp { get => timestamp; set => timestamp = value; }


        //Constructor Handles Connection and Writes initial line to file
        public ServerConnection(FileStream Form1fileStream)
        {
            Form1FileStreamObject = Form1fileStream;
            InitConnection();
            
        }

        public ServerConnection(){} // for Check User

        //Attempts connection a few times logs if unable 
        public void InitConnection()
        {
            ConnectionAttempts = 0;
            while (!client.Connected)
            {
                //Add Terminate if > 5
                if (ConnectionAttempts == 5)
                {
                    logger.Fatal("Unable to connect to server after repeated attemmpts " + DateTime.Now + "\n");
                    Console.Write("unable to connect");
                    Environment.Exit(1);//Multiple failures or just write to file... hmmm
                }
                ConnectionAttempts = ConnectClient(ConnectionAttempts);
            }
            //is connected - assignes stream - writes userName to server so it knows where to write data.
            serverStream = client.GetStream();
            byte[] byteName = Encoding.ASCII.GetBytes(Environment.UserName + "-" + DateTime.Now.ToString("yyyy-MM-dd") + "\n"); //writes to same file 
            serverStream.Write(byteName, 0, byteName.Length);
            GetServerData(); // gets our timestamp and day of the week
        }


        //Sloppy but OK way to attempt connections
        public int ConnectClient(int Count)
        {
            try
            {
                client.Connect(ConfigurationManager.AppSettings["IP"], //App.config Values
                        Int32.Parse(ConfigurationManager.AppSettings["PORT"]));
            }
            catch(Exception e)
            {  
                Count++;
                Random rnd = new Random();
                int ran = rnd.Next(1, (int)Math.Pow(2, Count)) * 1000; //playing with exponential backoff... just cuzz
                Debug.WriteLine("Failed Attempt " + ran + " Seconds"); 
                Thread.Sleep(ran);
            }
            return Count;
        }


        //Reads time data from server stores in global variable
        //https://stackoverflow.com/questions/22465102/c-sharp-read-all-bytes source becase horrible c#
        public void GetServerData()
        {
            Thread.Sleep(500);
            String dataString = "";
            if (serverStream.CanRead)//Best way I could get to read data from the inputstream sent from server
            {
                byte[] data = new byte[32];//only two lines
                byte[] dataArray = new byte[32];
                using (var writer = new MemoryStream())//Disposed after 
                {

                    while (serverStream.DataAvailable)
                    {
                        int bytesRead = serverStream.Read(data, 0, data.Length);
                        writer.Write(data, 0, bytesRead);
                    }
                    dataString += Encoding.UTF8.GetString(writer.ToArray());
                    Debug.Write(dataString);
                }
                
            }
            timestamp = dataString;
        }
        

        //grab file/write to connection
        public void SendDataToServer(Int32 byteCount)
        {

            Byte[] newDataFromFile = new Byte[byteCount];
            //Debug.WriteLine(client.SendBufferSize);
            Form1FileStreamObject.Seek(Form1FileStreamObject.Length - byteCount, SeekOrigin.Begin); //Seek Pointer - reads new data in file
            Form1FileStreamObject.Read(newDataFromFile, 0, byteCount);
            String a = Encoding.UTF8.GetString(newDataFromFile, 0, newDataFromFile.Length);
            try
            {
                serverStream.Write(newDataFromFile, 0, newDataFromFile.Length);
            }
            catch(IOException e)
            {
                ConnectClient(ConnectionAttempts);//lost connection somehow? try to reconnect
            }
            Thread.Sleep(100);

        }
      

        //needs no explaination 
        public void TerminateProtocol()//Kewl Name - Vury Surious
        {
            
            Byte[] disconnectCommand = Encoding.ASCII.GetBytes("Disconnect");
            ServerStream.Write(disconnectCommand, 0, disconnectCommand.Length);
            ServerStream.Flush();
            Client.Dispose();
            Environment.Exit(1);
        }

    }
}