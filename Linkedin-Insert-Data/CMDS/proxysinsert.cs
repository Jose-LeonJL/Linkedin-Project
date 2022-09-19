using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using CsvHelper;
using System.Globalization;
using System.Linq;
using MoreLinq;

namespace Linkedin_Insert_Data
{
    class proxysinsert
    {
        public static void insertProxys()
        {
            var proxys = JsonSerializer.Deserialize<List<proxys_model>>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "data", "Accounts - Modelo-Proxys.json")));
            Console.WriteLine($"-> Ingresando Proxys...");
            Console.WriteLine($"-> Proxys totales {proxys.Count}");
            //proxys = proxys.DistinctBy(x => x.proxy_ip).ToList();

            foreach(var proxy in proxys)
            {
                Console.WriteLine($"{proxy.proxy_ip}");
            }
            foreach (var proxy in proxys)
            {
                try
                {
                    proxys_model.CreateProxyWithId(Database.getdatabase(),proxy);
                    accounts_proxys.CreateAccountProxy(Database.getdatabase(), new accounts_proxys
                    { 
                        account_id = proxy.account_id,
                        proxy_id = proxy.proxy_id,
                        account_proxy_id = proxy.proxy_id
                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine("-> Error ingresando Proxys!!!");
                    Console.WriteLine($"-> Error {ex.Message}");
                    Console.WriteLine($"<-------------------------------------------->");
                }
            }

            Console.WriteLine($"-> Proxys ingresados");
            Console.WriteLine($"<-------------------------------------------->");
        }
    }
}
