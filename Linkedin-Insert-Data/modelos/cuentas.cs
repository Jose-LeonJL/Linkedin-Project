using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using FluentValidation;

namespace Linkedin_Insert_Data
{
    public class cuentass
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

        public static bool CreateAccount( cuentass account)
        {
            Database database = Database.getdatabase();
            using (var db = database.connection)
            {
                var sql = "INSERT INTO `accounts` (`account_id`,`account_name`,`account_mail`,`account_pass`,`account_linkedin_pass`,`account_recovery_mail`,`account_recovery_pass`,`account_mobile_number`,`account_profile_url`,`account_banned`,`account_salenav`,`account_mail_server`,`create_at`) VALUES (DEFAULT,@account_name,@account_mail,@account_pass,@account_linkedin_pass,@account_recovery_mail,@account_recovery_pass,@account_mobile_number,@account_profile_url,@account_banned,@account_salenav,@account_mail_server,@create_at);";
                var result = db.Execute(sql, new { account_name = account.account_name, account_mail = account.account_mail, account_pass = account.account_pass, account_linkedin_pass = account.account_linkedin_pass, account_recovery_mail = account.account_recovery_mail, account_recovery_pass = account.account_recovery_pass, account_mobile_number = account.account_mobile_number, account_profile_url = account.account_profile_url, account_banned = account.account_banned, account_salenav = account.account_salenav, account_mail_server = account.account_mail_server, create_at = account.create_at });
                return (result > 0);
            }
        }
        public static bool CreateAccountWithId(cuentass account)
        {
            Database database = Database.getdatabase();
            using (var db = database.connection)
            {
                var sql = "INSERT INTO `accounts` (`account_id`,`account_name`,`account_mail`,`account_pass`,`account_linkedin_pass`,`account_recovery_mail`,`account_recovery_pass`,`account_mobile_number`,`account_profile_url`,`account_banned`,`account_salenav`,`account_mail_server`,`create_at`) VALUES (@account_id,@account_name,@account_mail,@account_pass,@account_linkedin_pass,@account_recovery_mail,@account_recovery_pass,@account_mobile_number,@account_profile_url,@account_banned,@account_salenav,@account_mail_server,@create_at);";
                var result = db.Execute(sql, account);
                return (result > 0);
            }
        }
        public static List<cuentass> SelectAccountWithoutMessage(Database database, string message_type_id)
        {
            using (var db = database.connection)
            {
                var sql = $"SELECT * FROM accounts as ac WHERE NOT EXISTS(SELECT * FROM message_accounts as ma WHERE ac.account_id = ma.account_id);";
                return db.Query<cuentass>(sql).ToList();
            }
        }
    }

    public class cuentasvalidator : AbstractValidator<cuentass>
    {
        public cuentasvalidator()
        {
            RuleFor(x => x.account_id).NotNull().WithMessage("El id no puede ser nulo");
            RuleFor(x => x.account_mail_server).NotNull().WithMessage("El servermail no puede ser nulo").MaximumLength(50).WithMessage("El servermail no puede ser mayor a 50 caracteres");
            RuleFor(x => x.account_name).NotNull().WithMessage("El nombre no puede ser nulo");
        }
    }

    
    public class messages
        {
            public int message_id { get; set; }
            public int message_type_id { get; set; }
            public string message { get; set; }

            public static List<messages> SelectAll(Database database)
            {
                using (var db = database.connection)
                {
                    var sql = "select * from messages";
                    return db.Query<messages>(sql).ToList();
                }
            }
            public static messages SelectById(Database database, string id)
            {
                using (var db = database.connection)
                {
                    var sql = $"select * from messages where message_id=@id";
                    return db.QueryFirst<messages>(sql, new { id = id });
                }
            }
            public static bool CreateMessages(Database database, messages messages)
            {
                using (var db = database.connection)
                {
                    var sql = "INSERT INTO messages (`message_id`,`message_type_id`,`message`) VALUES (DEFAULT,@message_type_id,@message);";
                    var result = db.Execute(sql, messages);
                    return (result > 0);
                }
            }
        public static bool CreateMessagesWithId( messages messages)
        {
            Database database = Database.getdatabase();
            using (var db = database.connection)
            {
                var sql = "INSERT INTO messages (`message_id`,`message_type_id`,`message`) VALUES (@message_id,@message_type_id,@message);";
                var result = db.Execute(sql, messages);
                return (result > 0);
            }
        }
        public static List<messages> SelectMessagesAvailable(Database database)
            {
                using (var db = database.connection)
                {
                    var sql = $"SELECT * FROM messages as ms WHERE NOT EXISTS(SELECT * FROM message_accounts as ma WHERE ms.message_id = ma.message_id);";
                    return db.Query<messages>(sql).ToList();
                }
            }
        }
    public class message_accounts
    {
        public int message_account_id { get; set; }
        public int account_id { get; set; }
        public int message_id { get; set; }

        public static bool Createmessage_accounts(Database database, message_accounts message_Accounts)
        {
            using (var db = database.connection)
            {
                var sql = "INSERT INTO `message_accounts` (`message_account_id`,`account_id`,`message_id`) VALUES (DEFAULT,@account_id,@message_id);";
                var result = db.Execute(sql, message_Accounts);
                return (result > 0);
            }
        }
        public static messages SelectMessageByAccountId(Database database, string account_id)
        {
            using (var db = database.connection)
            {
                var sql = "select * from message_accounts where account_id=@account_id;";
                var data = db.QueryFirst<message_accounts>(sql, new { account_id = account_id });
                sql = "select * from messages where message_id=@message_id;";
                var result = db.QueryFirst<messages>(sql, data);
                return result;
            }
        }
    }

}
