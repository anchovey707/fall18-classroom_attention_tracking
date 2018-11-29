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
        //Initial Connection
        public void InitConnection(){

            tcpSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            tcpSocket.ReceiveTimeout=5000;
            udpSocket = new Socket(SocketType.Dgram, ProtocolType.Udp);
            ////////////////////////////////////////////////////////////////////////////////////////////outStream = new byte[10025];
            while (true) {
                try {
                    Console.WriteLine("Trying to connect");
                    tcpSocket.Connect(ip, port);
                    break;
                }catch(SocketException e) {
                    Console.WriteLine("Inital Connection Timed out");
                }
             
            }

            String returndata = "";

            //Wait for the start char, ';'
            while (returndata != ";") {
                inStream = new byte[tcpSocket.Available];
                tcpSocket.Receive(inStream);
                returndata = Encoding.ASCII.GetString(inStream);
                Console.WriteLine("'" + returndata + "'");
            }
            Console.WriteLine("Got the connection!");
            //Sending username to server
            outStream = Encoding.ASCII.GetBytes("#" + Environment.UserName +";");

            //send my username that I want to stream
            tcpSocket.Send(outStream);
    
            //starting the TCP thread that listens for commands
            Thread portThread = new Thread(portChanges);
            portThread.Start();
        }

        private void portChanges() {
            //wait for which port number to switch to
            String returndata = "";
            inStream = new byte[0];
            tcpSocket.ReceiveTimeout = 500;
            while (true) {
                try {
                    returndata = "";
                    //while getting message, if it conatins ';', then its the end of the message
                    while (!returndata.Contains(";")) {
                        inStream = new byte[tcpSocket.Available];
                        tcpSocket.Receive(inStream);
                        returndata += Encoding.ASCII.GetString(inStream);
                    }
                    //parse the port number out of the 
                    returndata = returndata.Substring(returndata.IndexOf(":") + 1, returndata.Length - returndata.IndexOf(":") - 2);
                    port = int.Parse(returndata);
                    Console.WriteLine("port=" + port);
                    readyToSend = true;
                } catch (SocketException e) {
                    //If it timedout then send the heartbeat
                    try {
                        tcpSocket.Send(new byte[] { 0 });
                        Console.WriteLine("heartbeat");
                    //If it can't even send the heartbeat, then try to reconnect
                    } catch (SocketException ee) {
                        InitConnection();
                    }
                }
            }
        }
        
        //Send the data to the server via the UDP socket
        public void SendDataToServer(byte[] data) {
            try {
                if (readyToSend)
                    udpSocket.SendTo(data, new IPEndPoint(IPAddress.Parse(ip), port));
            } catch (Exception e) {
                //If it did not send then it probably is no longer conencted, so try again
                Console.WriteLine(e.StackTrace);
                InitConnection();
            }
        }
      

    }
}
