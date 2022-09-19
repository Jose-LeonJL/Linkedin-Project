using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.IO;
using Linkedin.Net.Db;
using Linkedin.Net.Models;

namespace Linkedin_Insert_Data.Models
{
    internal class AccountToModel
    {
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string LinkedinPassword { get; set; }
        public string RecoveryEmail { get; set; }
        public string RecoveryPassword { get; set; }
        public string Link { get; set; }
        public string Proxy { get; set; }

        public static void Insert()
        {
            try
            {
                var dir = Console.ReadLine();
                var config = new CsvConfiguration(CultureInfo.InvariantCulture)
                {
                    Delimiter = ","
                };
                using (var reader = new StreamReader(dir))
                using (var csv = new CsvReader(reader, config))
                {
                    //csv.Context.RegisterClassMap<AccountToModelMap>();
                    var records = csv.GetRecords<accounts>();
                    foreach (var item in records)
                    {
                        try
                        {
                            var cuenta = Linkedin.Net.Models.accounts.CreateAccount(database.getdatabase(), item);
                            Console.WriteLine("Ingresado");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);

            }
            
        }
    }
    internal class AccountToModelMap : ClassMap<AccountToModel>
    {
        public AccountToModelMap()
        {
            Map(x => x.FirtName).Name("FIRST NAME");
            Map(x => x.LastName).Name("LAST NAME");
            Map(x => x.Email).Name("Email ID");
            Map(x => x.Password).Name("Password");
            Map(x => x.LinkedinPassword).Name("Linkedin Pass.");
            Map(x => x.RecoveryEmail).Name("Recovery Email");
            Map(x => x.RecoveryPassword).Name("Password");
            Map(x => x.Link).Name("Link");
            Map(x => x.Proxy).Name("Proxies");
        }
    }
}
