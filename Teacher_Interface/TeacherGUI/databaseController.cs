using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TeacherGUI
{
    public class databaseController
    {
        public static String databaseIP = ConfigurationManager.AppSettings["IP"].ToString();
        public static String databaseUser = "teacher";
        public static String databasePass = "course";
        public static String databaseName = "attentiontracking";
        public static String username = "";
        public static String sqlQuery;
        public static MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
   
        private static bool admin = false;
        private static string id = "";
        public static void dbConnect()
        {
            string myConnectionString;
            myConnectionString = "server=" + databaseIP + ";uid=" + databaseUser + ";" +
                "pwd=" + databasePass + ";database=" + databaseName;
            try
            {
                conn.ConnectionString = myConnectionString;
                conn.Open();
            }
            catch (MySql.Data.MySqlClient.MySqlException ex)
            {
                MessageBox.Show(ex.StackTrace+"  //Could not access database, check the connection values");
            }
        }
        
        
        
        public databaseController(){
            dbConnect();}

        //I put methods here to clean up the forms
        public static bool login(string user,string pass,bool hash) {
            if(hash)
                pass = Hash.passwordHash(pass);
            try {
                MySqlDataReader reader = new MySqlCommand("SELECT * FROM teacher WHERE login_id = '" + user + "' and pass = '" + pass + "';",conn).ExecuteReader();
                if (reader.HasRows) {
                    reader.Read();
                    admin = reader.GetBoolean("administrator");
                    id = reader.GetString("login_id");
                    reader.Close();
                    username = user;
                    return true;
                } else {
                    reader.Close();
                }
            }catch(Exception e) {
                Console.WriteLine(e.StackTrace);
            }
            return false;
        }
        
        //Login with default hashing
        public static bool login(string user, string pass){
            return login(user, pass, true);
        }

        //Get the teachers list of classes/courses
        public static string[] getClasses(){
            List<string> classlist = new List<string>();
            try {
                MySqlDataReader reader = new MySqlCommand("SELECT crn FROM course WHERE teacher_id = '" + id + "';", conn).ExecuteReader();
                while (reader.Read()) {
                    classlist.Add(reader.GetString("crn"));
                }
                reader.Close();
            } catch (Exception e) {
                classlist.Add("82325");
            }
            return classlist.ToArray();
        }


        public static bool isAdmin(){
            return admin;
        }

        //Get the student's full name
        public static string getFullName(string username) {
            List<string> classlist = new List<string>();
            string name = "~N/A";
            try {
                Console.WriteLine("SELECT CONCAT(first_name,' ',last_name) FROM student WHERE login_id = '" + username + "';");
                MySqlDataReader reader = new MySqlCommand("SELECT CONCAT(first_name,' ',last_name) as name FROM student WHERE login_id = '" + username + "';",conn).ExecuteReader();
                while (reader.Read()) {
                   name=reader.GetString("name");
                }
                reader.Close();
            } catch (Exception e) {
                Console.WriteLine(e.StackTrace);
            }
            return name;
        }

    }
}


