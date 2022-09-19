using System;
using System.Collections.Generic;
using System.Text;
using dotenv.net;
using MySql.Data.MySqlClient;
using System.IO;

namespace Linkedin_bot.db
{
    public class database
    {
        //variables
        private readonly static database _database = new database();
        private MySqlConnection _connetion;

        //propiedades
        public MySqlConnection connection { get { return this._connetion; } }

        public database() 
        {
            DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] {Path.Combine(Directory.GetCurrentDirectory(),".env") }));
            var server = $"Server={Environment.GetEnvironmentVariable("database_host")};";
            var database = $"DATABASE={Environment.GetEnvironmentVariable("database_name")};";
            var user = $"UID={Environment.GetEnvironmentVariable("database_user")};";
            var password = $"PASSWORD={Environment.GetEnvironmentVariable("database_password")};";
            var port = $"PORT={Environment.GetEnvironmentVariable("database_port")};";
            this._connetion = new MySqlConnection($"{server}{database}{user}{password}{port};CharSet=utf8mb4;");

        }

        public bool OpenConnection()
        {
            try
            {
                _connetion.Open();
                return true;
            }
            catch (MySqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        throw new Exception("Cannot connect to server.  Contact administrator");
                    case 1045:
                        throw new Exception("Invalid username/password, please try again");
                }
                return false;
            }
        }

        public static database getdatabase()
        {
            return _database;
        }
    }
}
