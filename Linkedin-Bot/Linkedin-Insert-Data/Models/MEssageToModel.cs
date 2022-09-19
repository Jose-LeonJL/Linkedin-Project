using CsvHelper;
using CsvHelper.Configuration;
using Linkedin.Net.Db;
using Linkedin.Net.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Linq;

namespace Linkedin_Insert_Data.Models
{
    internal class MessageToModel
    {
        public string Tipo { get; set; }
        public string Mensaje { get; set; }
        public static void Insert()
        {
            var dir = Console.ReadLine();
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ","
            };
            using (var reader = new StreamReader(dir))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<MessageToModelMap>();
                var records = csv.GetRecords<MessageToModel>();
                foreach (var item in records)
                {
                    try
                    {
                        var mensaje = Linkedin.Net.Models.messages.CreateMessages(database.getdatabase(), new Linkedin.Net.Models.messages
                        {
                            message_type_id = int.Parse(item.Tipo),
                            message = item.Mensaje
                        });

                        if (mensaje)
                        {
                            var ac = Linkedin.Net.Models.accounts.SelectAccountWithoutMessage(database.getdatabase(), item.Tipo).FirstOrDefault();
                            if (ac == null)
                            {
                                break;
                            }
                            var msj = Linkedin.Net.Models.messages.SelectByMessage(database.getdatabase(), item.Mensaje);
                            Linkedin.Net.Models.message_accounts.Createmessage_accounts(database.getdatabase(), new Linkedin.Net.Models.message_accounts
                            {
                                account_id = ac.account_id,
                                message_id = msj.message_id
                            });
                        }

                        Console.WriteLine("-> Ingresado");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
        }
    }
    internal class MessageToModelMap : ClassMap<MessageToModel>
    { 
        public MessageToModelMap()
        {
            Map(x => x.Tipo).Name("Tipo");
            Map(x => x.Mensaje).Name("Mensaje");
        }
    }
}
