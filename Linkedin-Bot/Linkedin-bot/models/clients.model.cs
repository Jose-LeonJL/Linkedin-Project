using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Linkedin_bot.db;
using System.Linq;

namespace Linkedin_bot.models
{
    public class clients
    {
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string client_email { get; set; }
        public string client_title { get; set; }
        public string client_url { get; set; }
        public string client_location { get; set; }
        public string client_company_name { get; set; }
        public string client_company_website { get; set; }
        public string client_company_industry { get; set; }
        public string client_company_linkedin_url { get; set; }
        public string client_description { get; set; }
        public double create_at { get; set; }

        public static List<clients> SelectAll(database database)
        {
            using (var db = database.connection)
            {
                var sql = "select * from clients";
                return db.Query<clients>(sql).ToList();
            }
        }
        public static clients SelectById(database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from clients where client_id=@id";
                return db.QueryFirstOrDefault<clients>(sql, new { id = id });
            }
        }
        public static bool CreateClients(database database, clients client)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO clients (`client_id`,`client_name`,`client_email`,`client_title`,`client_url`,`client_location`,`client_company_name`,`client_company_website`,`client_company_industry`,`client_company_linkedin_url`,`client_description`,`create_at`) VALUES (DEFAULT,@client_name,@client_email,@client_title,@client_url,@client_location,@client_company_name,@client_company_website,@client_company_industry,@client_company_linkedin_url,@client_description,@create_at);";
                var result = db.Execute(sql, client);
                return (result > 0);
            }
        }
        public static List<clients> SelectClientsAvailable(database database)
        {
            using (var db = database.connection)
            {
                var sql = $"SELECT * FROM clients as cs WHERE NOT EXISTS(SELECT * FROM client_connections as cc WHERE cs.client_id = cc.client_id);";
                return db.Query<clients>(sql).ToList();
            }
        }
    }
}
