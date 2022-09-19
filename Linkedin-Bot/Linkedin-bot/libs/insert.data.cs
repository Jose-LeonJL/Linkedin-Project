using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Linkedin_bot.models;
using Linkedin_bot.db;
using Microsoft.VisualBasic.CompilerServices;

namespace Linkedin_bot.libs
{
    class insert
    {
        public static void insertdata(database database)
        {
            var datos = File.ReadAllLines(@"E:\Documentos\Trabajo\Trabajos\5cre\Linkdin-bot\Test accounts - Sales Nav.csv");
            int count = 1;
            foreach (var dato in datos)
            {
                if (dato.StartsWith("First  Name")) continue;
                var linea = dato.Split(",");
                var mail = linea[2].Split("@")[1];
                if(mail == "outlook.com")
                {
                    mail = "https://login.live.com/login.srf";
                }else if (mail == "yahoo.com")
                {
                    mail = "https://login.yahoo.com/";
                }else
                {
                    mail = "nulo";
                }
                var ac = accounts.CreateAccount(database, new accounts
                {
                    account_name=linea[0]+" "+linea[1],
                    account_mail=linea[2],
                    account_pass = linea[3],
                    account_linkedin_pass = linea[5],
                    account_recovery_mail = linea[6],
                    account_recovery_pass = linea[7],
                    account_banned=false,
                    account_profile_url="",
                    account_mobile_number="",
                    account_salenav=true,
                    account_mail_server = mail
                });
                if (ac) Console.WriteLine("-> Cuenta agragada exitosamente"); else Console.WriteLine("-> Error agregando cuenta");
                var ps = proxys.CreateProxy(database, new proxys
                {
                    proxy_ip=linea[8].Split(":")[0],
                    proxy_port= linea[8].Split(":")[1],
                    proxy_user= linea[8].Split(":")[2],
                    proxy_pass= linea[8].Split(":")[3]
                });
                if (ps) Console.WriteLine("-> Proxy agragado exitosamente"); else Console.WriteLine("-> Error agregando proxy");
                if (ac & ps)
                {
                    var ap = accounts_proxys.CreateAccountProxy(database, new accounts_proxys { account_id = count, proxy_id = count });
                    if (ap) Console.WriteLine("-> Registro agregado con exito"); else Console.WriteLine("-> Error agregando registro");
                    count += 1;
                }
                else
                {
                    Console.WriteLine("-> error desconocido");
                }

                Console.WriteLine("--------------------------------------------------");
            }

        }
        public static void insertclients(database database)
        {
            var datos = File.ReadAllLines(@"E:\Documentos\Trabajo\Trabajos\5cre\Linkdin-bot\3500 clientes.csv");
            foreach (var dato in datos)
            {
                if (dato.StartsWith("First  Name")) continue;
                var linea = dato.Split("{");
                dynamic mail;
                if (linea[2] == "" || linea[2] == " ")
                {
                    mail = null;
                }
                else
                {
                    mail = linea[2];
                }
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                //Console.WriteLine("------------------------------------------------");
                //Console.WriteLine($"-> client_name : {linea[0] + " " + linea[1]}");
                //Console.WriteLine($"-> client_email : {linea[2]}");
                //Console.WriteLine($"-> client_title : {linea[3]}");
                //Console.WriteLine($"-> client_location : {linea[4]}");
                //Console.WriteLine($"-> client_url : {linea[5]}");
                //Console.WriteLine($"-> client_company_name : {linea[6]}");
                //Console.WriteLine($"-> client_company_website : {linea[7]}");
                //Console.WriteLine($"-> client_company_industry : {linea[8]}");
                //Console.WriteLine($"-> client_company_linkedin_url : {linea[11]}");
                //Console.WriteLine($"-> client_description : {linea[12]}");
                //Console.WriteLine("------------------------------------------------");
                //Console.WriteLine(linea[3].Length);
                //Console.WriteLine(linea[2]);
                var name = Encoding.UTF8.GetBytes(linea[0] + " " + linea[1]);
                var ct = clients.CreateClients(database, new clients
                {
                    client_name = Encoding.UTF8.GetString(name),
                    client_email = mail,
                    client_title = linea[3],
                    client_location = linea[4],
                    client_url = linea[5],
                    client_company_name = linea[6],
                    client_company_website = linea[7],
                    client_company_industry = linea[8],
                    client_company_linkedin_url = linea[11],
                    client_description = linea[12]
                });
                if (ct) Console.WriteLine("-> cliente guardado!!"); else Console.WriteLine("-> Error!!");
            }
        }
        public static void insertmessage(database database)
        {
            var datos = File.ReadAllLines(@"E:\Documentos\Trabajo\Trabajos\5cre\Linkdin-bot\mensajes.csv");
            var account = accounts.SelectAccountWithoutMessage(database,"1");
            foreach (var dato in datos)
            {
                var linea = dato.Split("[");


                var ct = messages.CreateMessages(database, new messages
                {
                    message_type_id=int.Parse(linea[0]),
                    message= linea[1]
                });
                if (ct) Console.WriteLine("-> mensaje guardado!!"); else Console.WriteLine("-> Error!!");
                
            }
            int mensage = 1;
            foreach(var dato in account)
            {
                var ma = message_accounts.Createmessage_accounts(database, new message_accounts
                {
                    account_id=dato.account_id,
                    message_id= mensage
                });
                if (ma) Console.WriteLine("-> relacion guardado!!"); else Console.WriteLine("-> Error!!");
                mensage += 1;
            }
        }
    }
}
