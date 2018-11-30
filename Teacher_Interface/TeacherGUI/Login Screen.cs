using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace TeacherGUI
{

    public partial class Login_Screen : Form
    {
        public Login_Screen()
        {
            InitializeComponent();
            databaseController.dbConnect();
        }

        //loginButton
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (databaseController.login(usernameTextBox.Text, passwordTextBox.Text)) {
                new TeacherHome(databaseController.isAdmin(), databaseController.getClasses()).Show();
                this.Hide();
            }else{
                MessageBox.Show("Either your username or password was incorrect. Please try again.");
            }
            
            

                
        }

        private void Login_Screen_Load(object sender, EventArgs e)
        {
            //Username textbox
            usernameTextBox.Enter += new EventHandler(username_Enter);
            usernameTextBox.Leave += new EventHandler(username_Leave);
            username_SetText();

            //Password textbox
            passwordTextBox.Enter += new EventHandler(password_Enter);
            passwordTextBox.Leave += new EventHandler(password_Leave);
            password_SetText();

            this.AcceptButton = button1;
        }
        
        //Username
        protected void username_SetText()
        {
            this.usernameTextBox.Text = "Username";
            usernameTextBox.ForeColor = Color.Gray;
        }

        private void username_Enter(object sender, EventArgs e)
        {
            if (usernameTextBox.ForeColor == Color.Black)
                return;
            usernameTextBox.Text = "";
            usernameTextBox.ForeColor = Color.Black;
        }
        private void username_Leave(object sender, EventArgs e)
        {
            if (usernameTextBox.Text.Trim() == "")
                username_SetText();
        }

        //Password
        protected void password_SetText()
        {
            this.passwordTextBox.Text = "Password";
            passwordTextBox.ForeColor = Color.Gray;
        }

        private void password_Enter(object sender, EventArgs e)
        {
            if (passwordTextBox.ForeColor == Color.Black)
                return;
            passwordTextBox.Text = "";
            passwordTextBox.ForeColor = Color.Black;
        }
        private void password_Leave(object sender, EventArgs e)
        {
            if (passwordTextBox.Text.Trim() == "")
                password_SetText();
        }
    }
}
