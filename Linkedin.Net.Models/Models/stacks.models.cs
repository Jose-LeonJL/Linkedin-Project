using Linkedin.Net.Db;
using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
namespace Linkedin.Net.Models
{
    public class stacks
    {
        public int stack_id { get; set; }
        public int invites { get; set; }
        public int connections { get; set; }
        public int replies { get; set; }
        public int messages_send { get; set; }
        public long fecha { get; set; }
        public static double unixdate
        {
            get
            {
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
                TimeSpan span = (DateTime.Now.ToLocalTime() - epoch);
                return span.TotalSeconds;
            }
        }
        public static double unixdateday
        {
            get
            {
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime();
                DateTime today = DateTime.Now;
                TimeSpan span = (new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, DateTimeKind.Local).ToLocalTime() - epoch);
                return span.TotalSeconds;
            }
        }

        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dateTime;
        }
        public static List<stacks> SelectAll()
        {
            var database = new database();
            using (var db = database.connection)
            {
                var sql = "Select * from stacks;";
                var result = db.Query<stacks>(sql).ToList();
                return result;
            }
        }

        public static bool CreateStacks(database database,stacks stack)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO  stacks(stack_id,invites,connections,replies,messages_send,fecha) VALUES(DEFAULT,@stack_id,@invites,@connections,@replies,@fecha) ;";
                var result = db.Execute(sql, stack);
                return (result > 0);
            }
        }
        public static bool UpdateStacks(database database, stacks stack)
        {
            using (var db = database.connection)
            {
                var sql = "select * from stacks where fecha = @fecha;";
                var data = db.QueryFirstOrDefault<stacks>(sql, stack);
                if (data == null) 
                {
                    CreateStacks(database, stack);
                    return true;
                }
                stack.invites += data.invites;
                stack.connections += data.connections;
                stack.replies += data.replies;
                stack.messages_send += data.messages_send;
                sql = " UPDATE stacks SET invites=@invites,connections=@connections,replies=@replies,messages_send=@messages_send WHERE fecha = @fecha;";
                var result = db.Execute(sql,stack);
                return (result > 0);
            }
        }
        public static List<Tuple<int, int>> SelectStacksTotales()
        {
            var dataBase = database.getdatabase();
            using(var db = dataBase.connection)
            {
                var sql = "SELECT ss.messages_send as conexiones,ss.fecha from stacks as ss;";
                var result = db.Query<int, int, Tuple<int, int>>(sql,Tuple.Create,splitOn:"*").ToList();
                return result;
            }
        }
        public static List<Tuple<int, int>> SelectStacksDiarios()
        {
            var dataBase = database.getdatabase();
            using (var db = dataBase.connection)
            {
                var sql = $"SELECT ss.messages_send as conexiones,ss.fecha from stacks as ss WHERE ss.fecha = {unixdateday}";
                var result = db.Query<int, int, Tuple<int, int>>(sql, Tuple.Create, splitOn: "*").ToList();
                return result;
            }
        }
        public static List<Tuple<string,string,long,string>> SelectStacksTotalesPorCuenta()
        {
            var dataBase = database.getdatabase();
            using(var db = dataBase.connection)
            {
                var sql = "SELECT c.account_name as nombre, c.account_mail as correo, COUNT(cc.account_id) as conexiones, FROM_UNIXTIME(cc.fecha_inicio, '%Y-%m-%d') as fecha FROM client_connections as cc JOIN accounts as c on c.account_id = cc.account_id GROUP BY cc.account_id,fecha; ";
                var result = db.Query<string, string, long, string, Tuple<string, string, long, string>>(sql, Tuple.Create, splitOn: "*").ToList();
                return result;
            }
        }
    }
}
