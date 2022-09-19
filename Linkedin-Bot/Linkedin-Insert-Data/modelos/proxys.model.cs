using System;
using System.Text;
using Dapper;
using System.Collections.Generic;
using System.Linq;

namespace Linkedin_Insert_Data
{
    public class proxys_model
    {
        public int proxy_id { get; set; }
        public int account_id { get; set; }
        public string proxy_ip { get; set; }
        public int proxy_port { get; set; }
        public string proxy_user { get; set; }
        public string proxy_pass { get; set; }
        public static List<proxys_model> SelectAll(Database database)
        {
            using (var db = database.connection)
            {
                var sql = "select * from proxys";
                return db.Query<proxys_model>(sql).ToList();
            }
        }
        public static proxys_model SelectById(Database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from proxys where proxy_id=@id";
                return db.QueryFirst<proxys_model>(sql, new { id = id });
            }
        }
        public static List<proxys_model> SelectProxysAvailable(Database database)
        {
            using (var db = database.connection)
            {
                var sql = $"SELECT * FROM proxys as px WHERE NOT EXISTS(SELECT proxy_id FROM accounts_proxys as ap WHERE px.proxy_id = ap.proxy_id)";
                return db.Query<proxys_model>(sql).ToList();
            }
        }
        public static bool CreateProxy(Database database, proxys_model   proxy)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO proxys (`proxy_id`,`proxy_ip`,`proxy_port`,`proxy_user`,`proxy_pass`) VALUES (DEFAULT,@proxy_ip,@proxy_port,@proxy_user,@proxy_pass);";
                var result = db.Execute(sql, new { proxy_ip = proxy.proxy_ip, proxy_port = proxy.proxy_port, proxy_user = proxy.proxy_user, proxy_pass = proxy.proxy_pass });
                return (result > 0);
            }
        }
        public static bool CreateProxyWithId(Database database, proxys_model proxy)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO proxys (`proxy_id`,`proxy_ip`,`proxy_port`,`proxy_user`,`proxy_pass`) VALUES (@proxy_id, @proxy_ip,@proxy_port,@proxy_user,@proxy_pass);";
                var result = db.Execute(sql, new { proxy_id= proxy.proxy_id, proxy_ip = proxy.proxy_ip, proxy_port = proxy.proxy_port, proxy_user = proxy.proxy_user, proxy_pass = proxy.proxy_pass });
                return (result > 0);
            }
        }
    }
    public class accounts_proxys
    {
        public int account_proxy_id { get; set; }
        public int proxy_id { get; set; }
        public int account_id { get; set; }

        public static List<accounts_proxys> SelectAll(Database database)
        {
            using (var db = database.connection)
            {
                var sql = "select * from accounts_proxys;";
                return db.Query<accounts_proxys>(sql).ToList();
            }
        }
        public static proxys_model SelectById(Database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from accounts_proxys where account_proxy_id=@id";
                return db.QueryFirst<proxys_model>(sql, new { id = id });
            }
        }
        public static bool CreateAccountProxy(Database database, accounts_proxys accounts_Proxys)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO accounts_proxys (`account_proxy_id`,`proxy_id`,`account_id`) VALUES (DEFAULT,@proxy_id,@account_id);";
                var result = db.Execute(sql, new { proxy_id = accounts_Proxys.proxy_id, account_id = accounts_Proxys.account_id, });
                return (result > 0);
            }
        }
    }
}
