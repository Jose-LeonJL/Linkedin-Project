using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Linkedin_bot.db;
using System.Linq;
namespace Linkedin_bot.models
{
    public class works
    {
        public int work_id { get; set; }
        public int work_priority { get; set; }
        public string work_name { get; set; }
        public string work_description { get; set; }
        public static List<works> SelectAll(database database)
        {
            using (var db = database.connection)
            {
                var sql = "select * from works";
                return db.Query<works>(sql).ToList();
            }
        }
        public static works SelectById(database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from works where work_id=@id";
                return db.QueryFirstOrDefault<works>(sql, new { id = id });
            }
        }
        public static bool CreateProxy(database database, works work)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO works (`work_id`,`work_priority`,`work_name`,`work_description`) VALUES (DEFAULT,@work_id,@work_priority,@work_name,@work_description);";
                var result = db.Execute(sql, new {  work_id=work.work_id, work_priority = work.work_priority, work_name= work.work_name, work_description=work.work_description });
                return (result > 0);
            }
        }
    }
}
