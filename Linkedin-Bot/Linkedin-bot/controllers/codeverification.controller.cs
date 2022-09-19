using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Linkedin_bot.db;
using Linkedin_bot.models;
using PuppeteerSharp;
using PuppeteerSharp.Input;

namespace Linkedin_bot.controllers
{
    public class codeverification
    {
        public async static Task messajevalidation(database database,accounts account,Browser browser, Page LoginPage)
        {
            //Creacion de nueva pagina
            var paginaCorreo = await browser.NewPageAsync();
            
            //Ir al servicio de correo
            var resgoto = await paginaCorreo.GoToAsync(account.account_mail_server, 120000, new[] { WaitUntilNavigation.DOMContentLoaded });

            //if()
            var Pin = await Hotmail(account,paginaCorreo);
            
        }
        private async static Task<string> Hotmail(accounts account, Page paginaCorreo)
        {
            string InputCorreo = "input[type='email']";
            string InputPassword = "input[type='password']";
            string InputSubmit = "input[type='submit']";

            //Esperamos a que carge el input del correo 
            await paginaCorreo.WaitForSelectorAsync(InputCorreo);
            await paginaCorreo.TypeAsync(InputCorreo, account.account_mail, new TypeOptions { Delay = new Random().Next(1, 5) * new Random().Next(10, 15) });

            //Precionamos el boton enviar
            await paginaCorreo.ClickAsync(InputSubmit);
            await Task.Delay(500);

            // Esperamos a que aparezca el input contraseña
            await Task.Delay(500);
            await paginaCorreo.TypeAsync(InputPassword, account.account_pass, new TypeOptions { Delay = new Random().Next(1, 5) * new Random().Next(10, 15) });

            //Precionamos el boton enviar
            await Task.Delay(500);
            await paginaCorreo.ClickAsync(InputSubmit);
            await Task.Delay(1500);

            //Verificamos la opcion de inicio de sesion
            ElementHandle btnsubmit = null;
            try
            {
                 btnsubmit= await paginaCorreo.QuerySelectorAsync(InputSubmit);
            } catch(Exception ex)
            {
                Console.WriteLine("-> Error, boton no encontrado");
            }

            if (btnsubmit == null)
            {
                Console.WriteLine("-> Boton no encontrado, verificando pendientes");
            }
            else
            {
                Console.WriteLine("-> Boton encontrado, verificando pendientes");
                await paginaCorreo.ClickAsync(InputSubmit);

                //Ir al servicio de correo
                //Esperar el cambio de pagina
                await paginaCorreo.WaitForNavigationAsync();
                var resgoto = await paginaCorreo.GoToAsync("https://outlook.live.com/mail/0/", 120000, new[] { WaitUntilNavigation.DOMContentLoaded });

                //Esperamos el boton de otros mensajes
                var btnothers = "button#Pivot25-Tab0";
                await paginaCorreo.WaitForSelectorAsync(btnothers);
                await paginaCorreo.ClickAsync(btnothers);

                //seleccionamos el contenedor de los mensajes 
                var contenedormensajes = await paginaCorreo.QuerySelectorAsync("div.s0OAmtxWyjwp1q8ElgO7");
                var mensajes = await contenedormensajes.QuerySelectorAllAsync("div[tabindex='-1'].ZtMcNhhoIIOO6raJ3mUG");
                Console.WriteLine($"-> Mensajes encontrados {mensajes.Length}");
            }
            return null;
        }
    }
}
