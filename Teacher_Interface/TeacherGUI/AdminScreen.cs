using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TeacherGUI
{
    public partial class AdminScreen : Form{
        public TeacherHome parentForm;
        public AdminScreen(Form home)
        {
            parentForm=(TeacherHome)home;
            InitializeComponent();
            EndTimePicker.Format=DateTimePickerFormat.Time;
            StartTimePicker.Format = DateTimePickerFormat.Time;
            //databaseController.dbConnect();
        }

        //Add teacher to database
        private void SubmitProfessor_Click(object sender, EventArgs e) {
            if (!(LoginTxtBox.Text == "Username" || password.Text == "Password" ||firstName.Text == "First Name" || lastName.Text == "Last Name")) { 
                databaseController.sqlQuery = "INSERT INTO teacher (login_id, pass, first_name, last_name) " +
                                              "VALUES (@login, @pass, @firstName, @lastName)";
                MySqlCommand cmd = new MySqlCommand(databaseController.sqlQuery, databaseController.conn);

                string passwordHash = Hash.passwordHash(password.Text);

                cmd.Parameters.AddWithValue("@login", LoginTxtBox.Text);
                cmd.Parameters.AddWithValue("@pass", passwordHash);
                cmd.Parameters.AddWithValue("@firstName", firstName.Text);
                cmd.Parameters.AddWithValue("@lastName", lastName.Text);

                try {
                    if (cmd.ExecuteNonQuery() > 0) {
                        MessageBox.Show("Professor Added");
                        updateTeachers();
                    } else {
                        MessageBox.Show("Submission failed, please ensure you entered all data and try again,\nor that it dosen't already exsit.");
                    }
                } catch (Exception ee) {
                    MessageBox.Show("Submission failed, please ensure you entered all data and try again,\nor that it dosen't already exsit.");

                }
            } else
                MessageBox.Show("Submission failed, please ensure you entered all data and try again,\nor that it dosen't already exsit.");
        }

        //Add calss to database
        private void SubmitClass_Click(object sender, EventArgs e)
        {
            if (!(CRNTxtBox.Text == "CRN" || CourseNameTextBox.Text == "Name")) {
                //databaseController.dbConnect();
                databaseController.sqlQuery = "INSERT INTO course (crn, teacher_id, name, startTime, endTime) " +
                                              "VALUES (@crn, @teacher_id, @name, @startTime, @endTime);";
                MySqlCommand cmd = new MySqlCommand(databaseController.sqlQuery, databaseController.conn);
                string starttime = StartTimePicker.Value.Hour + ":" + StartTimePicker.Value.Minute + ":" + StartTimePicker.Value.Second;
                string endtime = EndTimePicker.Value.Hour + ":" + EndTimePicker.Value.Minute + ":" + EndTimePicker.Value.Second;
                if (StartTimePicker.Value.Hour < 10)
                    starttime = "0" + starttime;
                if (EndTimePicker.Value.Hour < 10)
                    endtime = "0" + endtime;
                cmd.Parameters.AddWithValue("@crn", CRNTxtBox.Text);
                cmd.Parameters.AddWithValue("@teacher_id", TeacherListBox.SelectedValue);
                cmd.Parameters.AddWithValue("@name", CourseNameTextBox.Text);
                cmd.Parameters.AddWithValue("@startTime", starttime);
                cmd.Parameters.AddWithValue("@endTime", endtime);

                cmd.CommandText = "INSERT INTO course (crn, teacher_id, name, startTime, endTime) " +
                                              "VALUES (" + CRNTxtBox.Text + ", '" + TeacherListBox.SelectedValue + "',' "
                                              + CourseNameTextBox.Text + "','" + starttime + "', '" + endtime + "');";
                Console.WriteLine(cmd.CommandText);
                if (cmd.ExecuteNonQuery() > 0) {
                    MessageBox.Show("Class Added");
                    updateClasses();
                } else {
                    MessageBox.Show("Submission failed, please ensure you entered all data and try again,\nor that it dosen't already exsit.");
                }
            } else
                MessageBox.Show("Submission failed, please ensure you entered all data and try again,\nor that it dosen't already exsit.");
        }

        //on load
        private void AdminScreen_Load(object sender, EventArgs e)
        {

            updateClasses();

            updateTeachers();

            //Class Name textbox
            CourseNameTextBox.Enter += new EventHandler(textBox1_Enter);
            CourseNameTextBox.Leave += new EventHandler(textBox1_Leave);
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

            LoginTxtBox.Enter += new EventHandler(LoginTxtBox_Enter);
            LoginTxtBox.Leave += new EventHandler(LoginTxtBox_Leave);
            LoginTxtBox_SetText();

            CRNTxtBox.Enter += new EventHandler(CRNTxtBox_Enter);
            CRNTxtBox.Leave += new EventHandler(CRNTxtBox_Leave);
            CRNTxtBox_SetText();
            
        }

        public void updateTeachers() {

            //populate professor dataGridView
            databaseController.sqlQuery = "SELECT CONCAT(t.first_name, ' ', t.last_name) AS 'Professor', " +
                                                  "login_id" +
                                          " FROM teacher t;";
            MySqlCommand cmd = new MySqlCommand(databaseController.sqlQuery, databaseController.conn);
            MySqlDataAdapter myAdapter = new MySqlDataAdapter();
            myAdapter.SelectCommand = cmd;
            DataTable dgvDataTable = new DataTable();
            myAdapter.Fill(dgvDataTable);
            dataGridView2.DataSource = dgvDataTable;

            //populate professor dropdown
            databaseController.sqlQuery = "SELECT CONCAT(first_name, ' ', last_name) AS 'Professor', login_id " +
                                          "FROM teacher";
            cmd = new MySqlCommand(databaseController.sqlQuery, databaseController.conn);
            MySqlDataReader reader;

            reader = cmd.ExecuteReader();
            DataTable profDataTable = new DataTable();
            profDataTable.Columns.Add("login_id", typeof(string));
            profDataTable.Columns.Add("Professor", typeof(string));
            profDataTable.Load(reader);

            TeacherListBox.ValueMember = "login_id";
            TeacherListBox.DisplayMember = "Professor";
            TeacherListBox.DataSource = profDataTable;
            TeacherListBox.DropDownStyle = ComboBoxStyle.DropDownList;
            
        }

        public void updateClasses() {
            //populate class dataGridView
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
        }


        //code from https://stackoverflow.com/questions/14544135/how-to-gray-out-default-text-in-textbox
        protected void textBox1_SetText()
        {
            this.CourseNameTextBox.Text = "Class Name";
            CourseNameTextBox.ForeColor = Color.Gray;
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            if (CourseNameTextBox.ForeColor == Color.Black)
                return;
            CourseNameTextBox.Text = "";
            CourseNameTextBox.ForeColor = Color.Black;
        }
        private void textBox1_Leave(object sender, EventArgs e)
        {
            if (CourseNameTextBox.Text.Trim() == "")
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

        //Login text box
        protected void LoginTxtBox_SetText() {
            this.LoginTxtBox.Text = "Login id";
            LoginTxtBox.ForeColor = Color.Gray;
        }

        private void LoginTxtBox_Enter(object sender, EventArgs e) {
            if (LoginTxtBox.ForeColor == Color.Black)
                return;
            LoginTxtBox.Text = "";
            LoginTxtBox.ForeColor = Color.Black;
        }
        private void LoginTxtBox_Leave(object sender, EventArgs e) {
            if (LoginTxtBox.Text.Trim() == "")
                LoginTxtBox_SetText();
        }

        //CRN text box
        protected void CRNTxtBox_SetText() {
            this.CRNTxtBox.Text = "CRN";
            CRNTxtBox.ForeColor = Color.Gray;
        }

        private void CRNTxtBox_Enter(object sender, EventArgs e) {
            if (CRNTxtBox.ForeColor == Color.Black)
                return;
            CRNTxtBox.Text = "";
            CRNTxtBox.ForeColor = Color.Black;
        }
        private void CRNTxtBox_Leave(object sender, EventArgs e) {
            if (CRNTxtBox.Text.Trim() == "")
                CRNTxtBox_SetText();
        }

        private void AdminScreen_FormClosing(object sender, FormClosingEventArgs e) {
            parentForm.Show();
        }
    }
}
