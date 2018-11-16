using System;
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

        public TeacherHome()
        {
            InitializeComponent();
        }
    
        private void CurrentClass_Click(object sender, EventArgs e)
        {
            Form form; 
            try
            {
                form = new Form1(int.Parse(selectedClass.Text));
            }
            catch(Exception ea)
            {
                form = new Form1(1234);
            }
            form.Show();
            Hide();
        }

        private void HistoricData_Click(object sender, EventArgs e)
        {
            new Historic_Data().Show();
            Hide();
        }

        private void TeacherHome_Load(object sender, EventArgs e)
        {

        }
    }
}
