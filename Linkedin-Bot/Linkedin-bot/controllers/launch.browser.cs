using System;
using System.IO;
using System.Text;
using Linkedin.Net;
using PuppeteerSharp;
using Linkedin_bot.db;
using System.Text.Json;
using Linkedin_bot.config;
using Linkedin_bot.models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Linkedin_bot.controllers
{
    public class launch_browser
    {
        private Browser browser;
        public launch_browser()
        {
            
        }

        public async void loopMain(database database)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            //botenemos las cuentas con proxys
            var accountwithproxys = accounts_proxys.SelectAll(database);
            foreach (var awp in accountwithproxys)
            {
                //proxy
                proxys proxy = proxys.SelectById(database,awp.proxy_id.ToString());
                //account 
                accounts account = accounts.SelectById(database, awp.account_id.ToString());

                if (account.account_banned) { continue; }

                Console.WriteLine($"-> Cuenta BANNED : {account.account_banned}");
                //if (account.account_mail != "Audrinawise@rediffmail.com")
                //{
                //    continue;
                //}
                if (account.account_banned) 
                { 
                    continue; 
                }
                if (proxy.proxy_banned)
                {
                    continue;
                }
                Console.WriteLine($"-> Proceso...");


                //client 
                var client = clients.SelectClientsAvailable(database);
                if (client.Count == 0) { await Notificaciones.SendNotificationNoHaveClients(); }
                //mensaje
                messages message = message_accounts.SelectMessageByAccountId(database, account.account_id.ToString());


                //Launcher the browser
                this.browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = false,DefaultViewport = new ViewPortOptions { Width=600,Height=1200}, Args = new[] { $"--proxy-server={proxy.proxy_ip}:{proxy.proxy_port}" } });
                
                //create a new page and go to linkedin
                Page page = await browser.NewPageAsync();
                await page.AuthenticateAsync(new Credentials { Username = proxy.proxy_user, Password = proxy.proxy_pass });

                //iniciamos sesion
                var login = await login_controller.login(page, account);

                //verificamos el estado del login
                if (login.ok)
                {//enviamos mensajes
                    try
                    {
                        await sendmessage.sendamessage(page, account, client,message,database);
                    }catch(Exception ex)
                    {
                        Console.WriteLine("errorororo");
                        Console.WriteLine(ex.ToString());
                    }
                }else if (login.pin)
                {//obtenemos el pin y lo ingresamos
                    Console.WriteLine("-> Cuenta Requiere pin");
                    //await codeverification.messajevalidation(database, account, browser, page);
                }else if (login.banned)
                {//reportamos la cuenta beneada
                    Console.WriteLine("-> Cuenta Baneada");
                    await Notificaciones.SendNotificationBanned(account,proxy);
                    Console.WriteLine("-> Notificacion enviada");
                    account.ReportBenned(database);
                    Console.WriteLine("-> Cuenta reportada");

                }
                
                //guardamos cookies
                var cookie =await  page.GetCookiesAsync();
                try
                {
                    File.WriteAllText(Path.Combine("cookies", account.account_mail + ".json"), JsonSerializer.Serialize(cookie));
                    //var cookiesave =accounts.UpdateCookie( database, Linkedin.Net.Cookies.Cookie.CookieHeader(Path.Combine("cookies", account.account_mail + ".json")),account.account_id.ToString());
                    //if (cookiesave){Console.WriteLine("-> Cookie actualizada");}else{Console.WriteLine("-> Error Cookie no actualizada");}
                }
                catch (Exception ex )
                {
                    Console.WriteLine(ex.Message);
                }

                //if (cansendmessage) await sendmessage.sendamessage(page, account);
                await Task.Delay(new Random().Next(15, 20) * 1000);
                await browser.CloseAsync();
            }

        }
    }
}
