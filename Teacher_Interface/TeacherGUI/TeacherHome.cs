﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TeacherGUI
{

    

    public partial class TeacherHome : Form
    {
        private bool admin = false;
        public int adminForm = 0;
        public TeacherHome(bool a, string[] classes)
        {
            InitializeComponent();
            
            admin = a;
            for(int i = 0; i < classes.Length; i++) {
                Console.WriteLine(classes[i]);
            }
            selectedClass.Items.AddRange(classes);
            if(classes.Length>0)
               selectedClass.Text = classes[0];
            if (!admin)
                adminbtn.Hide();
        }
        

        //Redesign so that the teacher selects the class they currently want to stream and then start said streaming on click
        //use this to send the course that the teacher selects to the Server and wait to receive the port number that the udp stream will come in on
        /*g.outStream = Encoding.ASCII.GetBytes(usernameTextBox.Text + "#" + passwordTextBox.Text);
            g.serverStream.Write(g.outStream, 0, g.outStream.Length);
            g.serverStream.Flush();
        */
        //Change udpListener port to the current class port as received on the TCP connection
        private void CurrentClass_Click(object sender, EventArgs e)
        {
            Form form;
            System.Threading.Thread.Sleep(4000);
            try
            {
                form= new Form1(this,int.Parse(selectedClass.Text));
            }
            catch(Exception ea)
            {
                form = new Form1(this,12345);
            }
            try {
                form.Show();
                Hide();
            } catch (Exception eb) {
            }
        }

        private void HistoricData_Click(object sender, EventArgs e)
        {
            new Historic_Data().Show();
            Hide();
        }
        
        

        private void TeacherHome_FormClosing(object sender, FormClosingEventArgs e) {
            Application.Exit();
        }

        private void adminbtn_Click(object sender, EventArgs e) {
            Console.WriteLine(adminForm);
            if (adminForm++ == 0) { 
                new AdminScreen(this).Show();
                Console.WriteLine("Creating admin page");
                Hide();
            }
        }
    }
}
