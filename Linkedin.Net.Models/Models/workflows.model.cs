using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Linkedin.Net.Db;
using System.Linq;

namespace Linkedin.Net.Models
{
    public class workflows
    {
        public int workflow_id { get; set; }
        public int work_id { get; set; }
        public int account_id { get; set; }
        public bool started { get; set; }
        public bool finished { get; set; }
        public int fecha { get; set; }

        public static List<workflows> SelectAll(database database)
        {
            using (var db = database.connection)
            {
                var sql = "select * from workflows";
                return db.Query<workflows>(sql).ToList();
            }
        }
        public static accounts SelectById(database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from workflows where workflow_id=@id";
                return db.QueryFirstOrDefault<accounts>(sql, new { id = id });
            }
        }
        public static bool CreateAccount(database database, workflows workflow)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO `workflows` (`workflow_id`,`work_id`,`account_id`,`started`,`finished`,`fecha`) VALUES (DEFAULT,@work_id,@account_id,@started,@finished,@fecha);";
                var result = db.Execute(sql, new { work_id=workflow.work_id,account_id=workflow.account_id,started=workflow.started,finished=workflow.finished,fecha=workflow.fecha});
                return (result > 0);
            }
        }
    }
}
