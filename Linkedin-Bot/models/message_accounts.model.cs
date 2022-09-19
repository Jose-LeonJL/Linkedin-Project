using Linkedin_bot.db;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;

namespace Linkedin_bot.models
{
    public class message_accounts
    {
        public int message_account_id { get; set; }
        public int account_id { get; set; }
        public int message_id { get; set; }

        public static bool Createmessage_accounts(database database, message_accounts message_Accounts)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO `message_accounts` (`message_account_id`,`account_id`,`message_id`) VALUES (DEFAULT,@account_id,@message_id);";
                var result = db.Execute(sql, message_Accounts);
                return (result > 0);
            }
        }
        public static messages SelectMessageByAccountId(database database, string account_id)
        {
            using (var db = database.connection)
            {
                var sql = "select * from message_accounts where account_id=@account_id;";
                var data = db.QueryFirstOrDefault<message_accounts>(sql, new { account_id = account_id});
                sql = "select * from messages where message_id=@message_id;";
                var result = db.QueryFirstOrDefault<messages>(sql, data);
                return result;
            }
        }
    }
}
