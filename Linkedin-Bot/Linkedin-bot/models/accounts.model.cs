using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Linkedin_bot.db;
using System.Linq;

namespace Linkedin_bot.models
{
    public class accounts
    {
        public int account_id { get; set; }
        public string account_name { get; set; }
        public string account_mail_server { get; set; }
        public string account_mail { get; set; }
        public string account_pass { get; set; }
        public string account_linkedin_pass { get; set; }
        public string account_recovery_mail { get; set; }
        public string account_recovery_pass { get; set; }
        public string account_mobile_number { get; set; }
        public string account_profile_url { get; set; }
        public bool account_banned { get; set; }
        public bool account_salenav { get; set; }
        public bool account_selected { get; set; }
        public string account_cookie { get; set; }
        public double create_at { get; set; }
        public List<proxys> proxys { get; set; }
        public bool ReportBenned(database database)
        {
            using (var db = database.connection)
            {
                var sql = "UPDATE accounts SET account_banned = true WHERE account_id = @account_id;";
                var result = db.Execute(sql, this);
                return (result > 0);
            }
        }
        public static List<accounts> SelectAll(database database)
        {
            using (var db = database.connection)
            {
                var sql = "select * from accounts";
                return db.Query<accounts>(sql).ToList();
            }
        }
        public static accounts SelectById(database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from accounts where account_id=@id";
                return db.QueryFirstOrDefault<accounts>(sql, new { id = id });
            }
        }
        public static bool CreateAccount(database database, accounts account)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO `accounts` (`account_id`,`account_name`,`account_mail`,`account_pass`,`account_linkedin_pass`,`account_recovery_mail`,`account_recovery_pass`,`account_mobile_number`,`account_profile_url`,`account_banned`,`account_salenav`,`account_mail_server`,`create_at`) VALUES (DEFAULT,@account_name,@account_mail,@account_pass,@account_linkedin_pass,@account_recovery_mail,@account_recovery_pass,@account_mobile_number,@account_profile_url,@account_banned,@account_salenav,@account_mail_server,@create_at);";
                var result = db.Execute(sql, new { account_name=account.account_name,account_mail=account.account_mail,account_pass=account.account_pass,account_linkedin_pass= account.account_linkedin_pass,account_recovery_mail= account.account_recovery_mail,account_recovery_pass= account.account_recovery_pass,account_mobile_number= account.account_mobile_number,account_profile_url= account.account_profile_url,account_banned= account.account_banned,account_salenav= account .account_salenav, account_mail_server=account.account_mail_server, create_at=account.create_at });
                return (result > 0);
            }
        }
        public static List<accounts> SelectAccountWithoutMessage(database database,string message_type_id)
        {
            using (var db = database.connection)
            {
                var sql = $"SELECT * FROM accounts as ac WHERE NOT EXISTS(SELECT * FROM message_accounts as ma WHERE ac.account_id = ma.account_id);";
                return db.Query<accounts>(sql).ToList();
            }
        }
        public static bool UpdateCookie(database database, string cookie, string id )
        {
            using (var db = database.connection)
            {
                var sql = "update accounts set account_cookie=@cookie where account_id=@id;";
                var result = db.Execute(sql, new { cookie = cookie, id = id });
                return (result > 0);
            }
        }
        public static int AccountsWithOutCookies(database database)
        {
            using (var db = database.connection)
            {
                var sql = "SELECT COUNT(*) FROM accounts WHERE account_cookie is NULL;";
                var result = db.QueryFirstOrDefault<int>(sql);
                return result;
            }
        }
        public static int AccountsWithCookies(database database)
        {
            using (var db = database.connection)
            {
                var sql = "SELECT COUNT(*) FROM accounts;";
                var result = db.QueryFirstOrDefault<int>(sql);
                return result;
            }
        }
        public static int AccountsAllHaveCookies(database database)
        {
            var sin = AccountsWithOutCookies(database);
            var all = AccountsWithCookies(database);
            return (all - sin);
        }
        public static List<accounts> SelectAccountsWithCookies(database database)
        {
            using (var db = database.connection)
            {
                var sql = "SELECT * FROM accounts where account_cookie Is NOT NULL AND account_banned = false;";
                var result = db.Query<accounts>(sql).ToList();
                return result;
            }
        }
        public static List<accounts> SelectAccountsBanned(database database)
        {
            using (var db = database.connection)
            {
                var sql = "SELECT * FROM accounts where account_banned = true;";
                var result = db.Query<accounts>(sql).ToList();
                return result;
            }
        }
    }
}
