using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TeacherGUI
{
    public class databaseController
    {
        //probably need to pull this ip from a external config file thats eaasy to change
        public static String databaseIP = "192.168.0.54";
        public static String databaseUser = "teacher";
        public static String databasePass = "course";
        public static String databaseName = "attentiontracking";
        public static String username = "";
        public static String sqlQuery;
        public static MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
   
        private bool admin = false;
        private string id = "";
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
                MessageBox.Show(ex.Message);
            }
        }
        
        
        //I put methods here to clean up the login form
        public databaseController(){
            dbConnect();}
        
        public bool login(string user,string pass,bool hash) {
            if(hash)
                pass = Hash.passwordHash(pass);
            Console.WriteLine(pass);
            try {
                MySqlDataReader reader = new MySqlCommand("SELECT * FROM teacher WHERE login_id = '" + user + "' and pass = '" + pass + "';", databaseController.conn).ExecuteReader();
                if (reader.HasRows) {
                    reader.Read();
                    admin = reader.GetBoolean("administrator");
                    id = reader.GetString("login_id");
                    reader.Close();
                    username = user;
                    return true;
                } else {
                    reader.Close();
                    return false;
                }
            }catch(Exception e) {
                return false;
            }
        }
        public bool login(string user, string pass){
            return login(user, pass, true);
        }

        public string[] getClasses(){
            List<string> classlist = new List<string>();
            try {
                MySqlDataReader reader = new MySqlCommand("SELECT crn FROM course WHERE teacher_id = '" + id + "';", databaseController.conn).ExecuteReader();
                while (reader.Read()) {
                    classlist.Add(reader.GetString("crn"));
                }
                reader.Close();
                return classlist.ToArray();
            } catch (Exception e) {
                classlist.Add("82325");
            }
            return classlist.ToArray();
        }

        public bool isAdmin(){
            return admin;
        }
    }
}


