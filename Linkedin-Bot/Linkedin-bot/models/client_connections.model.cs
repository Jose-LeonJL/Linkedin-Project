using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Linkedin_bot.db;
using System.Linq;

namespace Linkedin_bot.models
{
    public class client_connections
    {
        public int client_connection_id { get; set; }
        public int client_id { get; set; }
        public int account_id { get; set; }
        public long fecha_inicio { get; set; }
        public long fecha_finalizacion { get; set; }
        public bool respuesta { get; set; }

        public static List<accounts> SelectAll(database database)
        {
            using (var db = database.connection)
            {
                var sql = "select * from client_connections";
                return db.Query<accounts>(sql).ToList();
            }
        }
        public static accounts SelectById(database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from client_connections where client_connection_id=@id";
                return db.QueryFirstOrDefault<accounts>(sql, new { id = id });
            }
        }
        public static bool CreateClient_Connections(database database, client_connections client_Connection)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO `client_connections` (`client_connection_id`,`client_id`,`account_id`,`fecha_inicio`,`fecha_finalizacion`,`respuesta`) VALUES (DEFAULT,@client_id,@account_id,@fecha_inicio,@fecha_finalizacion,@respuesta);";
                var result = db.Execute(sql, new { client_id=client_Connection.client_id, account_id=client_Connection.account_id, fecha_inicio=client_Connection.fecha_finalizacion, fecha_finalizacion=client_Connection.fecha_finalizacion, respuesta=client_Connection.respuesta });
                return (result > 0);
            }
        }
    }
}
