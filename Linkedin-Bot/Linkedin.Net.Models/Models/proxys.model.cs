using System;
using System.Text;
using Dapper;
using System.Collections.Generic;
using Linkedin.Net.Db;
using System.Linq;

namespace Linkedin.Net.Models
{
    public class proxys
    {
        public int proxy_id { get; set; }
        public string proxy_ip { get; set; }
        public string proxy_port { get; set; }
        public string proxy_user { get; set; }
        public string proxy_pass { get; set; }
        public bool proxy_banned { get; set; }
        public List<accounts> accounts { get; set; }

        public static List<proxys> SelectAll(database database)
        {
            using(var db = database.connection)
            {
                var sql = "select * from proxys";
                return db.Query<proxys>(sql).ToList();
            }
        }
        public static proxys SelectById(database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from proxys where proxy_id=@id";
                return db.QueryFirstOrDefault<proxys>(sql,new { id=id});
            }
        }
        public static List<proxys> SelectProxysAvailable(database database)
        {
            using (var db = database.connection)
            {
                var sql = $"SELECT * FROM proxys as px WHERE NOT EXISTS(SELECT proxy_id FROM accounts_proxys as ap WHERE px.proxy_id = ap.proxy_id);";
                return db.Query<proxys>(sql).ToList();
            }
        }
        public static bool CreateProxy(database database,proxys proxy)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO proxys (`proxy_id`,`proxy_ip`,`proxy_port`,`proxy_user`,`proxy_pass`) VALUES (DEFAULT,@proxy_ip,@proxy_port,@proxy_user,@proxy_pass);";
                var result = db.Execute(sql, new { proxy_ip = proxy.proxy_ip, proxy_port = proxy.proxy_port, proxy_user = proxy.proxy_user, proxy_pass = proxy.proxy_pass });
                return (result > 0);
            }
        }
        public bool Update(database database)
        {
            using (var db = database.connection)
            {
                var sql = $"UPDATE proxys SET proxy_ip = @proxy_ip, proxy_port= @proxy_port, proxy_user= @proxy_user, proxy_pass=@proxy_pass,proxy_banned= @proxy_banned WHERE proxy_id = @proxy_id;";
                var result = db.Execute(sql,this);
                return (result > 0);
            }
        }
        public static proxys SelectByIp(database database, string ip)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from proxys where proxy_ip=@ip";
                return db.QueryFirstOrDefault<proxys>(sql, new { ip = ip });
            }
        }
        public static List<proxys> SelectProxysBanned(database database)
        {
            using (var db = database.connection)
            {
                var sql = $"SELECT * FROM proxys where proxy_banned = true; ";
                return db.Query<proxys>(sql).ToList();
            }
        }
        public static List<proxys> SelectProxysWithAccountBanned(database database)
        {
            using (var db = database.connection)
            {
                var sql = $"SELECT px.proxy_id,px.proxy_ip,px.proxy_port,px.proxy_user,px.proxy_pass,px.proxy_banned " +
                    $"FROM accounts as ac join accounts_proxys as ap on ap.account_id = ac.account_id JOIN proxys as px on px.proxy_id = ap.proxy_id " +
                    $"WHERE px.proxy_banned = false and ac.account_banned = true; ";
                return db.Query<proxys>(sql).ToList();
            }
        }
        
    }
}
