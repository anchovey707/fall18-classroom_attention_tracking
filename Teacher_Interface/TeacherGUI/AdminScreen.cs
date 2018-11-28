using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TeacherGUI
{
    public partial class AdminScreen : Form
    {
        public AdminScreen()
        {
            InitializeComponent();
        }

        private void SubmitProfessor_Click(object sender, EventArgs e)
        {
            databaseController.dbConnect();
            databaseController.sqlQuery = "INSERT INTO teacher (login_id, pass, first_name, last_name) " +
                                          "VALUES (@login, @pass, @firstName, @lastName)";
            MySqlCommand cmd = new MySqlCommand(databaseController.sqlQuery, databaseController.conn);

            string passwordHash = Hash.passwordHash(password.Text);

            cmd.Parameters.AddWithValue("@login", Environment.UserName);
            cmd.Parameters.AddWithValue("@pass", passwordHash);
            cmd.Parameters.AddWithValue("@firstName", firstName.Text);
            cmd.Parameters.AddWithValue("@lastName", lastName.Text);

            if (cmd.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Professor Added");
                databaseController.conn.Close();
            }
            else
            {
                MessageBox.Show("Submission failed, please ensure you entered all data and try again.");
                databaseController.conn.Close();
            }
        }

        private void SubmitClass_Click(object sender, EventArgs e)
        {
            
            databaseController.dbConnect();
            databaseController.sqlQuery = "INSERT INTO course (crn, teacher_id, start_time, end_time) " +
                                          "VALUES (@crn, @teacher_id, @startTime, @endTime)";
            MySqlCommand cmd = new MySqlCommand(databaseController.sqlQuery, databaseController.conn);

            cmd.Parameters.AddWithValue("@crn", crn.Text);
            cmd.Parameters.AddWithValue("@teacher_id", teacher_id.Text);
            cmd.Parameters.AddWithValue("@startTime", startTime.Text);
            cmd.Parameters.AddWithValue("@endTime", endTime.Text);

            if (cmd.ExecuteNonQuery() > 0)
            {
                MessageBox.Show("Class Added");
                databaseController.conn.Close();
            }
            else
            {
                MessageBox.Show("Submission failed, please ensure you entered all data and try again.");
                databaseController.conn.Close();
            }

        }

        //on load
        private void AdminScreen_Load(object sender, EventArgs e)
        {

            //populate class dataGridView
            databaseController.dbConnect();
            databaseController.sqlQuery = "SELECT CONCAT(t.first_name, ' ', t.last_name) AS 'Professor', " +
                                                  "c.name 'Class Name', c.startTime 'Start Time' " +
                                                  "FROM course c " +
                                                  "JOIN teacher t ON c.teacher_id = t.login_id;";
            MySqlCommand cmd = new MySqlCommand(databaseController.sqlQuery, databaseController.conn);
            MySqlDataAdapter myAdapter = new MySqlDataAdapter();
            myAdapter.SelectCommand = cmd;
            DataTable dgvDataTable = new DataTable();
            myAdapter.Fill(dgvDataTable);
            dataGridView1.DataSource = dgvDataTable;

            //populate professor dataGridView
            databaseController.sqlQuery = "SELECT CONCAT(t.first_name, ' ', t.last_name) AS 'Professor', " +
                                                  "login_id AS 'Username'" +
                                          "FROM teacher t;";
            cmd = new MySqlCommand(databaseController.sqlQuery, databaseController.conn);
            myAdapter = new MySqlDataAdapter();
            myAdapter.SelectCommand = cmd;
            dgvDataTable = new DataTable();
            myAdapter.Fill(dgvDataTable);
            dataGridView2.DataSource = dgvDataTable;

            //populate professor dropdown
            databaseController.sqlQuery = "SELECT CONCAT(first_name, ' ', last_name) AS 'Professor', login_id AS 'Username'" +
                                          "FROM teacher";
            cmd = new MySqlCommand(databaseController.sqlQuery, databaseController.conn);
            MySqlDataReader reader;

            reader = cmd.ExecuteReader();
            DataTable profDataTable = new DataTable();
            profDataTable.Columns.Add("login_id", typeof(string));
            profDataTable.Columns.Add("Professor", typeof(string));
            profDataTable.Load(reader);

            teacher_id.ValueMember = "login_id";
            teacher_id.DisplayMember = "Professor";
            teacher_id.DataSource = profDataTable;
            teacher_id.DropDownStyle = ComboBoxStyle.DropDownList;

            //Class Name textbox
            courseName.Enter += new EventHandler(textBox1_Enter);
            courseName.Leave += new EventHandler(textBox1_Leave);
            textBox1_SetText();

            //First Name textbox
            firstName.Enter += new EventHandler(firstName_Enter);
            firstName.Leave += new EventHandler(firstName_Leave);
            firstName_SetText();

            //Last Name textbox
            lastName.Enter += new EventHandler(lastName_Enter);
            lastName.Leave += new EventHandler(lastName_Leave);
            lastName_SetText();

            //Password textbox
            password.Enter += new EventHandler(password_Enter);
            password.Leave += new EventHandler(password_Leave);
            password_SetText();

            //crn textbox
            crn.Enter += new EventHandler(crn_Enter);
            crn.Leave += new EventHandler(crn_Leave);
            crn_SetText();


            databaseController.conn.Close();
        }

        //code from https://stackoverflow.com/questions/14544135/how-to-gray-out-default-text-in-textbox
        protected void textBox1_SetText()
        {
            this.courseName.Text = "Class Name";
            courseName.ForeColor = Color.Gray;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (courseName.ForeColor == Color.Black)
                return;
            courseName.Text = "";
            courseName.ForeColor = Color.Black;
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (courseName.Text.Trim() == "")
                textBox1_SetText();
        }

        //teacher firstName
        protected void firstName_SetText()
        {
            this.firstName.Text = "First Name";
            firstName.ForeColor = Color.Gray;
        }

        private void firstName_Enter(object sender, EventArgs e)
        {
            if (firstName.ForeColor == Color.Black)
                return;
            firstName.Text = "";
            firstName.ForeColor = Color.Black;
        }
        private void firstName_Leave(object sender, EventArgs e)
        {
            if (firstName.Text.Trim() == "")
                firstName_SetText();
        }

        //teacher lastName
        protected void lastName_SetText()
        {
            this.lastName.Text = "Last Name";
            lastName.ForeColor = Color.Gray;
        }

        private void lastName_Enter(object sender, EventArgs e)
        {
            if (lastName.ForeColor == Color.Black)
                return;
            lastName.Text = "";
            lastName.ForeColor = Color.Black;
        }
        private void lastName_Leave(object sender, EventArgs e)
        {
            if (lastName.Text.Trim() == "")
                lastName_SetText();
        }

        //teacher password
        protected void password_SetText()
        {
            this.password.Text = "Password";
            password.ForeColor = Color.Gray;
        }

        private void password_Enter(object sender, EventArgs e)
        {
            if (password.ForeColor == Color.Black)
                return;
            password.Text = "";
            password.ForeColor = Color.Black;
        }
        private void password_Leave(object sender, EventArgs e)
        {
            if (password.Text.Trim() == "")
                password_SetText();
        }

        //course crn

        protected void crn_SetText()
        {
            this.crn.Text = "CRN";
            crn.ForeColor = Color.Gray;
        }

        private void crn_Enter(object sender, EventArgs e)
        {
            if (crn.ForeColor == Color.Black)
                return;
            crn.Text = "";
            crn.ForeColor = Color.Black;
        }
        private void crn_Leave(object sender, EventArgs e)
        {
            if (crn.Text.Trim() == "")
                crn_SetText();
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        // public class DayOfWeek
        // {
        //     public string Name { get; set; }
        //     public string Value { get; set; }
        // }
    }
}
