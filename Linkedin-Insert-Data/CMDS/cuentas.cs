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
    public static class cuentasinsert
    {
        public static void Insertcuentas()
        {
            var cuentas = JsonSerializer.Deserialize<List< cuentass>> (File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "data", "Accounts - Modelo-Cuentas.json")));
            Console.WriteLine($"-> Ingresando cuentas...");
            Console.WriteLine($"-> cuentas totales {cuentas.Count}");
            //cuentas = cuentas.DistinctBy(x => x.account_mail).ToList();

            foreach (var cuenta in cuentas)
            {
                try
                {
                    var mail = cuenta.account_mail.Split("@")[1];
                    if (mail == "outlook.com")
                    {
                        mail = "https://login.live.com/login.srf";
                    }
                    else if (mail == "yahoo.com")
                    {
                        mail = "https://login.yahoo.com/";
                    }
                    else
                    {
                        mail = "nulo";
                    }
                    cuenta.account_mail_server = mail;
                    cuenta.create_at = clients.unixdateday;
                    cuentass.CreateAccountWithId(cuenta);
                }
                catch (Exception EX)
                {
                    Console.WriteLine("-> Error ingresando cuentas!!!");
                    Console.WriteLine($"-> Error {EX.Message}");
                    Console.WriteLine($"<-------------------------------------------->");
                }
            }

            Console.WriteLine($"-> Cuentas ingresados");
            Console.WriteLine($"<-------------------------------------------->");
        }
    }
}
