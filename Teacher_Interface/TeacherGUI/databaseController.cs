using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace TeacherGUI
{
    public class databaseController
    {
        public static String databaseIP = "10.40.51.151";
        public static String databaseUser = "admin";
        public static String databasePass = "aannt707";
        public static String databaseName = "attentionTracking";
        public static String sqlQuery;
        public static MySqlConnection conn = new MySql.Data.MySqlClient.MySqlConnection();
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
    }
}


