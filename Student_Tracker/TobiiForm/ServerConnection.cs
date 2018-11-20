using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TobiiForm
{


    

    //Class for connecting to the server
    public class ServerConnection
    {
        private int count = 0;
        private Socket tcpSocket,udpSocket;
        private byte[] outStream,inStream;
        private bool readyToSend = false;
        private int port = 61600;
        private string ip = "192.168.0.54";
        /*int ConnectionAttempts = 0;
        TcpClient client = new TcpClient();
        NetworkStream serverStream;*/
        //Access to data from config
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        //Logger
        //private static Logger logger = LogManager.GetCurrentClassLogger();
        FileStream Form1FileStreamObject;
        String timestamp;
        /*public Validator valid = new Validator(); //access to checks
        //Getters/Setters
        public NetworkStream ServerStream { get => serverStream; set => serverStream = value; }
        public TcpClient Client { get => client; set => client = value; }*/
        public string Timestamp { get => timestamp; set => timestamp = value; }


        //Constructor Handles Connection and Writes initial line to file
        public ServerConnection(FileStream Form1fileStream){
            Form1FileStreamObject = Form1fileStream;
            InitConnection();
        }
        
        public void InitConnection()
        {

            tcpSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            udpSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            outStream = new byte[10025];

            Console.WriteLine("trying to connect");
            ///////////////////need a way to grab an IP from a config file///////////////////
            tcpSocket.Connect(ip, port);

            Console.WriteLine("waiting for starting char");
            String returndata = "";

            //Wait for the start char, ';'
            while (returndata != ";") {
                inStream = new byte[tcpSocket.Available];
                tcpSocket.Receive(inStream);
                returndata = Encoding.ASCII.GetString(inStream);
                Console.WriteLine("'" + returndata + "'");
            }
            Console.WriteLine("got start char");
            outStream = Encoding.ASCII.GetBytes("#" + Environment.UserName +";");
            //send my name and corse that I want to stream
            tcpSocket.Send(outStream);
            Console.WriteLine("waiting for port info");
            
            Thread portThread = new Thread(portChanges);
            portThread.Start();





            /*ConnectionAttempts = 0;
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
            GetServerData(); // gets our timestamp and day of the week*/
        }

        private void portChanges() {
            //wait for port
            String returndata = "";
            inStream = new byte[0];
            tcpSocket.ReceiveTimeout = 1000;
            while (true) {
                try { 
                    returndata = "";
                    while (!returndata.Contains(";")) {
                        inStream = new byte[tcpSocket.Available];
                        tcpSocket.Receive(inStream);
                        returndata += Encoding.ASCII.GetString(inStream);
                    }
                    Console.WriteLine("got data=" + returndata);
                    returndata = returndata.Substring(returndata.IndexOf(":") + 1, returndata.Length - returndata.IndexOf(":") - 2);
                    port = int.Parse(returndata);
                    Console.WriteLine("port=" + port);
                    readyToSend = true;
                }catch(SocketException e) {
                    tcpSocket.Send(new byte[] { 0 });
                    Console.WriteLine("timeout,sending heartbeat");
                }
            }
        }


        /*Sloppy but OK way to attempt connections
        public int ConnectClient(int Count)
        {
            try {
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
        }*/


        /*Reads time data from server stores in global variable
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
        }*/


        public void SendDataToServer(byte[] data) {
            try {
                if (readyToSend)
                    udpSocket.SendTo(data, new IPEndPoint(IPAddress.Parse(ip), port));
            } catch (IOException e) {
            }
        }

        //grab file/write to connection
        public void SendDataToServer(Int32 byteCount)
        {

            Byte[] newDataFromFile = new Byte[byteCount];
            //Debug.WriteLine(client.SendBufferSize);
            Form1FileStreamObject.Seek(Form1FileStreamObject.Length - byteCount, SeekOrigin.Begin); //Seek Pointer - reads new data in file
            Form1FileStreamObject.Read(newDataFromFile, 0, byteCount);
            String a = Encoding.UTF8.GetString(newDataFromFile, 0, newDataFromFile.Length);
            Console.WriteLine("a.length="+a.Length);
            try
            {
                // serverStream.Write(newDataFromFile, 0, newDataFromFile.Length);
                byte[] bytes = Encoding.ASCII.GetBytes((++count).ToString());
                Debug.WriteLine("count="+count);
                if (readyToSend)
                    udpSocket.SendTo(newDataFromFile, new IPEndPoint(IPAddress.Parse(ip),port));
            }
            catch(IOException e)
            {
                //ConnectClient(ConnectionAttempts);//lost connection somehow? try to reconnect
            }
        }
      

        /*needs no explaination 
        public void TerminateProtocol()//Kewl Name - Vury Surious
        {
            
            Byte[] disconnectCommand = Encoding.ASCII.GetBytes("Disconnect");
            ServerStream.Write(disconnectCommand, 0, disconnectCommand.Length);
            ServerStream.Flush();
            Client.Dispose();
            Environment.Exit(1);
        }*/

    }
}
