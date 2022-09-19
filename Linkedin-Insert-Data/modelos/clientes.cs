using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;
using Linkedin_Insert_Data;
using FluentValidation;

namespace Linkedin_Insert_Data
{
    public class clients
    {
        public int client_id { get; set; }
        public string client_name { get; set; }
        public string client_email { get; set; }
        public string client_title { get; set; }
        public string client_url { get; set; }
        public string client_location { get; set; }
        public string client_company_name { get; set; }
        public string client_company_website { get; set; }
        public string client_company_industry { get; set; }
        public string client_company_linkedin_url { get; set; }
        public string client_description { get; set; }
        public double create_at { get; set; }
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
        public static List<clients> SelectAll(Database database)
        {
            using (var db = database.connection)
            {
                var sql = "select * from clients";
                return db.Query<clients>(sql).ToList();
            }
        }
        public static clients SelectById(Database database, string id)
        {
            using (var db = database.connection)
            {
                var sql = $"select * from clients where client_id=@id";
                return db.QueryFirst<clients>(sql, new { id = id });
            }
        }
        public static bool CreateClients( clients client)
        {
            Database database = Database.getdatabase();
            using (var db = database.connection)
            {
                var sql = "INSERT INTO clients (`client_id`,`client_name`,`client_email`,`client_title`,`client_url`,`client_location`,`client_company_name`,`client_company_website`,`client_company_industry`,`client_company_linkedin_url`,`client_description`,`create_at`) VALUES (DEFAULT,@client_name,@client_email,@client_title,@client_url,@client_location,@client_company_name,@client_company_website,@client_company_industry,@client_company_linkedin_url,@client_description,@create_at);";
                var result = db.Execute(sql, client);
                return (result > 0);
            }
        }
        public static List<clients> SelectClientsAvailable(Database database)
        {
            using (var db = database.connection)
            {
                var sql = $"SELECT * FROM clients as cs WHERE NOT EXISTS(SELECT * FROM client_connections as cc WHERE cs.client_id = cc.client_id);";
                return db.Query<clients>(sql).ToList();
            }
        }
    }
    public class clientsValidatior : AbstractValidator<clients>
    {
        public clientsValidatior()
        {
            RuleFor(x => x.client_id).NotNull().WithMessage("El id no puede ser nulo").Must(Positivo).WithMessage("El id debe ser un numero mayor a 0");
            RuleFor(x => x.client_name).NotNull().WithMessage("El nombre no puede ser nulo").NotEmpty().WithMessage("El nombre no puede estar vacio").MaximumLength(100).WithMessage("El nombre no debe de sobrepasar los 100 caracteres");
            RuleFor(x => x.client_email).MaximumLength(100).WithMessage("El correo no debe de sobrepasar los 100 caracteres");
            RuleFor(x => x.client_title).MaximumLength(250).WithMessage("El Titulo no debe de sobrepasar los 250 caracteres");
            RuleFor(x => x.client_url).NotNull().WithMessage("El url no puede ser nulo").NotEmpty().WithMessage("El url no puede estar vacio").MaximumLength(250).WithMessage("El url no puede ser mayor a 250 caracteres");
            RuleFor(x => x.client_location).MaximumLength(100).WithMessage("La locacion no debe de sobrepasar los 100 caracteres");
            RuleFor(x => x.client_company_name).MaximumLength(100).WithMessage("El nombre de la compañia no debe de sobrepasar los 100 caracteres");
            RuleFor(x => x.client_company_website).MaximumLength(250).WithMessage("El sito de la compañia no debe de sobrepasar los 250 caracteres");
            RuleFor(x => x.client_company_industry).MaximumLength(250).WithMessage("La industia no debe de sobrepasar los 250 caracteres");
            RuleFor(x => x.client_company_linkedin_url).MaximumLength(250).WithMessage("El url de la compañia no debe de sobrepasar los 250 caracteres");
            RuleFor(x => x.client_description).MaximumLength(250).WithMessage("La descripcion no debe de sobrepasar los 250 caracteres");
        }
        private bool Positivo(int id)
        {
            return id > 0;
        }
        
    }
}
