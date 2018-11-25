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

namespace TobiiForm{

    //Class for connecting to the server
    public class ServerConnection{
        private int count = 0;
        private Socket tcpSocket,udpSocket;
        private byte[] outStream,inStream;
        private bool readyToSend = false;
        private int port = int.Parse(ConfigurationManager.AppSettings["PORT"].ToString());
        private string ip = ConfigurationManager.AppSettings["IP"].ToString();


        //Constructor Handles Connection and Writes initial line to file
        public ServerConnection(){
            InitConnection();
        }
        
        public void InitConnection(){

            tcpSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            udpSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            outStream = new byte[10025];

            Console.WriteLine("trying to connect");
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

            //send my username that I want to stream
            tcpSocket.Send(outStream);

            //starting the TCP thread that listens for commands
            Thread portThread = new Thread(portChanges);
            portThread.Start();
        }

        private void portChanges() {
            //wait for port
            String returndata = "";
            inStream = new byte[0];
            tcpSocket.ReceiveTimeout = 500;
            while (true) {
                try { 
                    returndata = "";
                    while (!returndata.Contains(";")) {
                        inStream = new byte[tcpSocket.Available];
                        tcpSocket.Receive(inStream);
                        returndata += Encoding.ASCII.GetString(inStream);
                    }
                    returndata = returndata.Substring(returndata.IndexOf(":") + 1, returndata.Length - returndata.IndexOf(":") - 2);
                    port = int.Parse(returndata);
                    Console.WriteLine("port=" + port);
                    readyToSend = true;
                }catch(SocketException e) {
                    tcpSocket.Send(new byte[] { 0 });
                }
            }
        }
        
        
        public void SendDataToServer(byte[] data) {
            try {
                if (readyToSend)
                    udpSocket.SendTo(data, new IPEndPoint(IPAddress.Parse(ip), port));
            } catch (IOException e) {
            }
        }
      

    }
}
