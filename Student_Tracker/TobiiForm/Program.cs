using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Tobii.Research;
using System.Drawing;
using System.Runtime.InteropServices;
using System.IO;

namespace TobiiForm
{
    class Program
    {
        private static Form1 current;


        


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(){
            //file.AutoFlush = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            current = new Form1();

            current.Show();


            Debug.WriteLine("Trying to run App");
            Application.Run(current);
        }

        

    }

}
