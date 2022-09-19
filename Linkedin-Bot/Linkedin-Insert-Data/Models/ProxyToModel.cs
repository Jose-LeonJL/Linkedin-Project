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
    internal class ProxyToModel
    {
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
                //csv.Context.RegisterClassMap<AccountToModelMap>();
                var records = csv.GetRecords<proxys>();
                foreach (var item in records)
                {
                    try
                    {
                        var cuenta = Linkedin.Net.Models.proxys.CreateProxy(database.getdatabase(), item);
                        var proxy = Linkedin.Net.Models.proxys.SelectByIp(database.getdatabase(), item.proxy_ip.ToString());
                        Console.WriteLine("Ingresado");
                        var acc = Linkedin.Net.Models.accounts_proxys.SelectAccountWithOutProxy(database.getdatabase());
                        Linkedin.Net.Models.accounts_proxys.CreateAccountProxy(database.getdatabase(), new Linkedin.Net.Models.accounts_proxys
                        {
                            account_id = acc.account_id,
                            proxy_id = proxy.proxy_id
                        });
                        Console.WriteLine("Relacionado");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }

                }
            }
        }
        public static void Complete()
        {
            var proxys = Linkedin.Net.Models.proxys.SelectProxysAvailable(database.getdatabase());
            foreach(var proxy in proxys)
            {
                try
                {
                    var acc = Linkedin.Net.Models.accounts_proxys.SelectAccountWithOutProxy(database.getdatabase());
                    Linkedin.Net.Models.accounts_proxys.CreateAccountProxy(database.getdatabase(), new Linkedin.Net.Models.accounts_proxys
                    {
                        account_id = acc.account_id,
                        proxy_id = proxy.proxy_id
                    });
                    Console.WriteLine("Relacionado");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            
        }
        public static void ReAsignacionProxys()
        {
            var proxys = Linkedin.Net.Models.proxys.SelectProxysWithAccountBanned(database.getdatabase());
            var cuentas = Linkedin.Net.Models.accounts.SelectAccountsWithProxyBanned(database.getdatabase());
            for (int i = 0; i < proxys.Count; i++)
            {
                var proxy = proxys[i];
                var cuenta = cuentas[i];

                var referenciaProxy = Linkedin.Net.Models.accounts_proxys.SelectByProxyId(database.getdatabase(), proxy.proxy_id.ToString());
                referenciaProxy.Delete(database.getdatabase());
                var referenciaCuenta = Linkedin.Net.Models.accounts_proxys.Selectaccounts_proxysByIdAccount(database.getdatabase(), cuenta.account_id.ToString());
                referenciaCuenta.Delete(database.getdatabase());
                
                //Creamos nueva referencia valida para el proxy inutil y la cuenta inutil
                Linkedin.Net.Models.accounts_proxys.CreateAccountProxy(database.getdatabase(), new Linkedin.Net.Models.accounts_proxys
                {
                    account_id = referenciaProxy.account_id,
                    proxy_id = referenciaCuenta.proxy_id
                });

                //Creamos nueva referencia valida para el proxy util y la cuenta util
                Linkedin.Net.Models.accounts_proxys.CreateAccountProxy(database.getdatabase(), new Linkedin.Net.Models.accounts_proxys
                {
                    account_id = referenciaCuenta.account_id,
                    proxy_id = referenciaProxy.proxy_id
                });
                Console.WriteLine("-> Nueva referencia creada!!!");
            }
            Console.WriteLine("-> Terminado!!!");
            
        }
    }
}
