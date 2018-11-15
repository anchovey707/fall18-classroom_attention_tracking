using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using Tobii.Research;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TobiiForm
{
    class Program
    {
        private static Form1 current;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Debug.WriteLine("Started!");
            //file.AutoFlush = true;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            Debug.WriteLine("current new form");
            current = new Form1{    
                StartPosition = FormStartPosition.Manual
            };
            Debug.WriteLine("Trying to run App");
            Application.Run(current);


        }

        

    }

}
