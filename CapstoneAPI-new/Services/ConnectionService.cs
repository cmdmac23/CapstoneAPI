using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneAPI_new.Services
{
    public class ConnectionService
    {
        public static string server = "mysql8001.site4now.net";
        public static string dbName = "db_a84892_cmac23";
        public static string user = "a84892_cmac23";
        public static string password = "";
        public static string port = "3306";


        public static MySqlConnection connection = null;

        public static void OpenConnection()
        {
            string connectionString = "SERVER=" + server + ";DATABASE=" + dbName + ";PORT=" + port + ";USER ID=" + user + ";PASSWORD=" + password;
            connection = new MySqlConnection(connectionString);

            connection.Open();
        }

        public static void CloseConnection()
        {
            connection.Close();
        }
    }
}
