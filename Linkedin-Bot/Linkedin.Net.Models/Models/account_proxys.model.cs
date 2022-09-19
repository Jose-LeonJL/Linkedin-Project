using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Linkedin.Net.Db;
using System.Linq;

namespace Linkedin.Net.Models
{
    public class accounts_proxys
    {
        public int account_proxy_id { get; set; }
        public int proxy_id  { get; set; }
        public int account_id { get; set; }

        public static List<accounts_proxys> SelectAll(database database)
        {
            using (var db = database.connection)
            {
                var sql = "select * from accounts_proxys;";
                return db.Query<accounts_proxys>(sql).ToList();
            }
        }       
        public static accounts_proxys SelectByProxyId(database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select ap.account_proxy_id, ap.proxy_id, ap.account_id from accounts_proxys as ap JOIN proxys as px ON px.proxy_id = ap.proxy_id where ap.proxy_id = @id;";
                return db.QueryFirstOrDefault<accounts_proxys>(sql, new { id = id });
            }
        }
        public static proxys SelectById(database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from accounts_proxys where account_proxy_id=@id";
                return db.QueryFirstOrDefault<proxys>(sql, new { id = id });
            }
        }
        public static bool CreateAccountProxy(database database, accounts_proxys accounts_Proxys)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO accounts_proxys (`account_proxy_id`,`proxy_id`,`account_id`) VALUES (DEFAULT,@proxy_id,@account_id);";
                var result = db.Execute(sql, new { proxy_id = accounts_Proxys.proxy_id, account_id = accounts_Proxys.account_id,});
                return (result > 0);
            }
        }
        public static proxys SelectByIdAccount(database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select px.proxy_id, px.proxy_ip, px.proxy_port, px.proxy_user, px.proxy_pass, px.proxy_banned from accounts_proxys as ap JOIN proxys as px ON px.proxy_id = ap.proxy_id where account_id = @id;";
                return db.QueryFirstOrDefault<proxys>(sql, new { id = id });
            }
        }
       
        public static accounts_proxys Selectaccounts_proxysByIdAccount(database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from accounts_proxys where account_id = @id;";
                return db.QueryFirstOrDefault<accounts_proxys>(sql, new { id = id });
            }
        }
        public static accounts SelectAccountWithOutProxy(database database)
        {
            using (var db = database.connection)
            {
                var sql = $"SELECT * FROM accounts as ac WHERE NOT EXISTS(SELECT account_id FROM accounts_proxys as ap WHERE ac.account_id = ap.account_id);";
                return db.QueryFirstOrDefault<accounts>(sql);
            }
        }
        public bool update(database database)
        {
            using (var db = database.connection)
            {
                var sql = $"UPDATE accounts_proxys SET proxy_id = @proxy_id, account_id = @account_id WHERE account_proxy_id = @account_proxy_id;";
                var result = db.Execute(sql, this);
                return (result > 0);
            }
        }

        public bool Delete(database database)
        {
            using (var db = database.connection)
            {
                var sql = $"DELETE FROM accounts_proxys where account_proxy_id = @account_proxy_id;";
                var result = db.Execute(sql, this);
                return (result > 0);
            }
        }
    }
    
}
