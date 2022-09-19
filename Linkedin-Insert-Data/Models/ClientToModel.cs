using CsvHelper;
using CsvHelper.Configuration;
using Linkedin.Net.Db;
using Linkedin.Net.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Linkedin_Insert_Data.Models
{
    internal class ClientToModel
    {
        public string FirtName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string URL { get; set; }

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
                //csv.Context.RegisterClassMap<ClientToModelMap>();
                var records = csv.GetRecords<Linkedin.Net.Models.clients>();
                foreach (var item in records)
                {
                    try
                    {
                        Linkedin.Net.Models.clients.CreateClients(database.getdatabase(), new Linkedin.Net.Models.clients
                        {
                            client_email = item.client_email,
                            client_url = item.client_url,
                            client_name = item.client_name,
                            client_title = "Desconocido",
                            client_description = "Desconocida",
                            create_at = stacks.unixdate
                        });

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
        }

        public static void AutoComplete()
        {
            var clientes = Linkedin.Net.Models.clients.SelectClientsAvailable(database.getdatabase());
            var accounts = Linkedin.Net.Models.accounts.SelectAccountsAviable(database.getdatabase());
            foreach (var cliente in clientes)
            {
                var account = accounts[new Random().Next(accounts.Count)];
                var insert = client_connections.CreateClient_Connections(database.getdatabase(), new client_connections
                {
                    client_id = cliente.client_id,
                    account_id = account.account_id,
                    fecha_inicio = Convert.ToInt64(stacks.unixdate),
                    fecha_finalizacion = (Convert.ToInt64(stacks.unixdate) + (86400 * 28))
                });
                if (insert)
                {
                    Console.WriteLine("Ingresado");
                }
            }
        }
    }
    internal class ClientToModelMap : ClassMap<ClientToModel>
    {
        public ClientToModelMap()
        {
            Map(x => x.FirtName).Name("First name");
            Map(x => x.LastName).Name("Last name");
            Map(x => x.Email).Name("Email");
            Map(x => x.URL).Name("Linkedin");
        }
    }
}
