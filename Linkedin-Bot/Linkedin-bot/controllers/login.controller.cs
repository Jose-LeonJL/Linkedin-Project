using System;
using System.Collections.Generic;
using System.Text;
using PuppeteerSharp;
using Linkedin_bot.config;
using Linkedin_bot.models;
using PuppeteerSharp.Input;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using Linkedin_bot.db;

namespace Linkedin_bot.controllers
{
    public class login_controller
    {
        public static int cuentassirven = 0;
        public static async Task<results> login(Page page, accounts account)
        {
            results result = new results();
            string pathcookiesfile = Path.Combine(Directory.GetCurrentDirectory(), "cookies", account.account_mail + ".json");
            //verificamos cookies
            if (File.Exists(pathcookiesfile))
            {
                try
                {
                    CookieParam cookieParam = new CookieParam();
                    await page.SetCookieAsync(JsonSerializer.Deserialize<CookieParam[]>(File.ReadAllText(pathcookiesfile)));
                    Console.WriteLine("-> cookies aplicadas");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"-> error aplicando cookies{ex}");
                }
            }
            try
            {
                //ingresamos a Linkedin
                var resgoto = await page.GoToAsync(configuracion.linkedin.url, 120000, new[] { WaitUntilNavigation.DOMContentLoaded });
                await Task.Delay(10000);
                var proxy = accounts_proxys.SelectByIdAccount(database.getdatabase(), account.account_id.ToString());
                proxy.proxy_banned = false;
                proxy.Update(database.getdatabase());
                result.ok = false;
                //return result;
            }
            catch (NavigationException ex)
            {
                Notificaciones.SendNotificationError(account, "Error Ingresando al login, es necesario revisar el proxycredentials").Wait();
                Console.WriteLine("-> Error Ingresando al login, es necesario revisar el proxycredentials");
                Console.WriteLine(ex.Message);
                result.ok = false;
                return result;
            }


            try
            {
                if (page.Url == configuracion.linkedin.urlloginsuccess)
                {
                    result.ok = true;
                    Console.WriteLine("-> Login Success using cookies");
                } else if (page.Url == configuracion.linkedin.url)
                {
                    Console.WriteLine("-> Ejecutando login using mail n user");
                    try
                    {
                        await page.TypeAsync(configuracion.linkedin.login.txtuser, account.account_mail);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("-> Error ingresando usuario");
                        Console.WriteLine(ex.Message);
                        result.ok = false;
                    }
                    await page.TypeAsync(configuracion.linkedin.login.txtpass, account.account_linkedin_pass);
                    await page.ClickAsync(configuracion.linkedin.login.btnlogin, new ClickOptions { Delay = new Random().Next(1, 3) * new Random().Next(10, 20) });
                    await page.WaitForNavigationAsync();
                } else if (page.Url == configuracion.linkedin.urlbanned)
                {
                    result.banned = true;
                    Console.WriteLine("-> Cuenta baneada");
                } else if (page.Url.StartsWith(configuracion.linkedin.verificationcode.url))
                {
                    await Notificaciones.SendNotificationNeedVerification(account);

                    try
                    {
                        Console.WriteLine("-> Si no hay codigo por ingresar o desea cancelar por favor escriba 'salir'");
                        Console.WriteLine("-> Ingrese el codigo de verificacion : ");
                        var codigo = Console.ReadLine();

                        var pin = await page.QuerySelectorAsync(configuracion.linkedin.verificationcode.txtpin);
                        await pin.TypeAsync(codigo);

                        var btn = await page.QuerySelectorAsync("#email-pin-submit-button");
                        await btn.ClickAsync();

                        await page.WaitForNavigationAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("-> Error ingresando el pin");
                    }

                    //await Task.Delay(600000);
                    if (page.Url.StartsWith(configuracion.linkedin.verificationcode.url))
                    {
                        result.pin = true;
                    }
                    else
                    {
                        result.ok = true;
                    }

                }
                else
                {
                    Console.WriteLine("Madres que pedo");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("-> Error verificando el autologin");
                Console.WriteLine(ex.Message);
                result.ok = false;
            }


            try
            {
                if (page.Url == configuracion.linkedin.urlloginsuccess)
                {
                    result.ok = true;
                    Console.WriteLine("-> Login Success using cookies");
                }
                else if (page.Url == configuracion.linkedin.url)
                {
                    Console.WriteLine("-> Ejecutando login using mail n user");
                    try
                    {
                        await page.TypeAsync(configuracion.linkedin.login.txtuser, account.account_mail, new TypeOptions { Delay = new Random().Next(1, 5) * new Random().Next(10, 100) });
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("-> Error ingresando usuario");
                        Console.WriteLine(ex.Message);
                        result.ok = false;
                    }
                    await page.TypeAsync(configuracion.linkedin.login.txtpass, account.account_linkedin_pass);
                    await page.ClickAsync(configuracion.linkedin.login.btnlogin, new ClickOptions { Delay = new Random().Next(1, 3) * new Random().Next(10, 20) });
                    await page.WaitForNavigationAsync();
                }
                else if (page.Url == configuracion.linkedin.urlbanned)
                {
                    result.banned = true;
                    Console.WriteLine("-> Cuenta baneada");
                }
                else if (page.Url.StartsWith(configuracion.linkedin.verificationcode.url))
                {
                    try
                    {
                        var pin = await page.QuerySelectorAsync(configuracion.linkedin.verificationcode.txtpin);
                        if (pin == null) { result.pin = false; result.banned = true; } else { result.pin = true; }

                        await Notificaciones.SendNotificationNeedVerification(account);
                        Console.WriteLine("-> Si no hay codigo por ingresar o desea cancelar por favor escriba 'salir'");
                        Console.WriteLine("-> Ingrese el codigo de verificacion : ");
                        var codigo = Console.ReadLine();
                        await pin.TypeAsync(codigo);

                        var btn = await page.QuerySelectorAsync("#email-pin-submit-button");
                        await btn.ClickAsync();

                        await page.WaitForNavigationAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("-> Error ingresando el pin");
                    }


                    if (page.Url.StartsWith(configuracion.linkedin.verificationcode.url))
                    {
                        result.pin = true;
                        result.ok = false;
                    }
                    else
                    {
                        result.pin = false;
                        result.ok = true;
                    }
                }
                else
                {
                    Console.WriteLine("Madres que pedo");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("-> Error verificando el autologin");
                Console.WriteLine(ex.Message);
                result.ok = false;
            }

            return result;
        }
        public static bool validateaccount(Page page, accounts account)
        {
            bool result = false;
            //verificamos el estado del login
            if (page.Url == configuracion.linkedin.urlloginsuccess)
            {
                Console.WriteLine("-> login shido");
                Console.WriteLine("------------------------------------");
                result = true;
            }
            else if (page.Url == configuracion.linkedin.urlbanned)
            {
                Console.WriteLine("-> cuenta baneada");
                Console.WriteLine("------------------------------------");
            }
            else
            {
                Console.WriteLine("-> necesitamos pin");
                Console.WriteLine("------------------------------------");
            }
            return result;
        }

    }
}
