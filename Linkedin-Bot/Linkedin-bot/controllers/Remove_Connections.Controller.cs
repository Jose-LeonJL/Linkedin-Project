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
    internal class Remove_Connections
    {
        public async void loopMain(database database)
        {
            await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
            //botenemos las cuentas con proxys
            var accountwithproxys = accounts_proxys.SelectAll(database);
            foreach (var awp in accountwithproxys)
            {
                //proxy
                proxys proxy = proxys.SelectById(database, awp.proxy_id.ToString());
                //account 
                accounts account = accounts.SelectById(database, awp.account_id.ToString());
                if (account.account_banned) { continue; }

                //Launcher the browser
                var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = false, DefaultViewport = new ViewPortOptions { Width = 600, Height = 1200 }, Args = new[] { $"--proxy-server={proxy.proxy_ip}:{proxy.proxy_port}" } });

                //create a new page and go to linkedin
                Page page = await browser.NewPageAsync();
                await page.AuthenticateAsync(new Credentials { Username = proxy.proxy_user, Password = proxy.proxy_pass });

                //iniciamos sesion
                var login = await login_controller.login(page, account);


                if (login.ok)
                {//enviamos mensajes
                    try
                    {
                        //Remove connection
                        await Remove(page);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Error desconocidos");
                        Console.WriteLine(ex.ToString());
                    }
                }
                else if (login.pin)
                {//obtenemos el pin y lo ingresamos
                    Console.WriteLine("-> Cuenta Requiere pin");
                    //await codeverification.messajevalidation(database, account, browser, page);
                }
                else if (login.banned)
                {//reportamos la cuenta beneada
                    Console.WriteLine("-> Cuenta Baneada");
                    await Notificaciones.SendNotificationBanned(account, proxy);
                    Console.WriteLine("-> Notificacion enviada");
                    account.ReportBenned(database);
                    Console.WriteLine("-> Cuenta reportada");

                }

                await browser.CloseAsync();
            }
        }

        public async Task Remove(Page page)
        {
            page.GoToAsync("https://www.linkedin.com/mynetwork/invitation-manager/sent/");
            await page.WaitForNavigationAsync();

            var MainContainer = await page.QuerySelectorAsync("#main");

            try
            {
                await MainContainer.ClickAsync();
                for (int i = 0; i < 6; i++)
                {
                    await MainContainer.PressAsync("Space");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("-> Error en el DOM");
            }

            await Task.Delay(5000);

            var SecondContainer = await MainContainer.QuerySelectorAsync("section.mn-invitation-manager__artdeco-card.artdeco-card.ember-view");
            var ThreeContainer = await SecondContainer.QuerySelectorAsync("div.self-focused.ember-view");
            var InvitationContainer = await ThreeContainer.QuerySelectorAsync("ul.artdeco-list.mn-invitation-list");

            var Conexiones = await InvitationContainer.QuerySelectorAllAsync("li");

            foreach(var conexion in Conexiones)
            {
                //var 
            }
            Console.WriteLine(Conexiones.Length);
            //await Task.Delay(100000);
        }
    }
}
