using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Linkedin_bot.db;
using System.Linq;

namespace Linkedin_bot.models
{
    public class messages
    {
        public int message_id { get; set; }
        public int message_type_id { get; set; }
        public string message { get; set; }

        public static List<messages> SelectAll(database database)
        {
            using (var db = database.connection)
            {
                var sql = "select * from messages";
                return db.Query<messages>(sql).ToList();
            }
        }
        public static messages SelectById(database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from messages where message_id=@id";
                return db.QueryFirstOrDefault<messages>(sql, new { id = id });
            }
        }
        public static bool CreateMessages(database database, messages messages)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO messages (`message_id`,`message_type_id`,`message`) VALUES (DEFAULT,@message_type_id,@message);";
                var result = db.Execute(sql, messages);
                return (result > 0);
            }
        }
        public static List<messages> SelectMessagesAvailable(database database)
        {
            using (var db = database.connection)
            {
                var sql = $"SELECT * FROM messages as ms WHERE NOT EXISTS(SELECT * FROM message_accounts as ma WHERE ms.message_id = ma.message_id);";
                return db.Query<messages>(sql).ToList();
            }
        }
    }
}
