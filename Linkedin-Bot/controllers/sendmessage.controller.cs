using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;
using Linkedin_bot.models;
using Linkedin_bot.config;
using PuppeteerSharp.Input;
using System.Linq;
using Linkedin_bot.db;
using System.Net.Http;

namespace Linkedin_bot.controllers
{
    public class sendmessage
    {
        private static List<mensajes> mensajes = new List<mensajes>
        {
            new mensajes()
            {
                diainicio=0,
                diaFin=6,
                min =22,
                max=25
            },
            new mensajes()
            {
                diainicio=6,
                diaFin=11,
                min =16,
                max=27
            },
             new mensajes()
            {
                diainicio=11,
                diaFin=14,
                min =21,
                max=38
            },
             new mensajes()
            {
                diainicio=14,
                diaFin=21,
                min =34,
                max=39
            },
             new mensajes()
            {
                diainicio=21,
                diaFin=25,
                min =29,
                max=49
            },
             new mensajes()
            {
                diainicio=25,
                diaFin=30,
                min =42,
                max=60
            }
        };
        public static mensajes  RatioMensajes(int dia)
        {
            if (dia <= 0) dia = 1;
            if (dia >= 30) dia = 29;
            var x = (from cosa in mensajes where (dia >= cosa.diainicio && dia < cosa.diaFin) orderby cosa.diainicio select cosa).ToList().First();
            return x;
        }
        public static async Task sendamessage(Page page, accounts account, List<clients> clients,messages message,database database)
        {
            TimeSpan spanTime = (stacks.UnixTimeStampToDateTime(stacks.unixdate) - stacks.UnixTimeStampToDateTime(account.create_at).ToLocalTime());
            Console.WriteLine($"-> Dias Totales {Convert.ToInt32(spanTime.TotalDays)}");
            mensajes mensajes = RatioMensajes(1);
            int min;
            int max;
            if (mensajes == null)
            {
                max = 20;
                min = 15;
            }
            else
            {
                min = mensajes.min;
                max = mensajes.max;
            }
            if (min > clients.Count)
            {
                if (clients.Count > 0)
                {
                    min = 1;
                    max = clients.Count;
                }
                else
                {
                    min = 0;
                    max = 0;
                }
            }
            int totalmensajes = new Random().Next(min, max);
            for (int i =0; i < totalmensajes; i++)
            {
                bool btnconninmain = false, btnconninsec =false;
                //obtenemos datos del cliente
                var random = new Random().Next(clients.Count);
                clients client = clients[random];
                var pageresult = await page.GoToAsync(client.client_url, 120000, new[] { WaitUntilNavigation.DOMContentLoaded });

                if (page.Url == "https://www.linkedin.com/404/")
                {
                    Console.WriteLine("-> El cliente no existe");
                    client_connections.CreateClient_Connections(database, new client_connections
                    {
                        account_id = account.account_id,
                        client_id = client.client_id,
                        fecha_inicio = 0,
                        fecha_finalizacion = 0,
                        respuesta = true,
                    });
                    continue;
                }
                //abrimos el contenedor de opciones
                await page.WaitForSelectorAsync("div.pvs-profile-actions");
                var contenedorbotones = await page.QuerySelectorAllAsync("div.pvs-profile-actions");
                await page.WaitForSelectorAsync("button[type='button'][aria-label='More actions']");
                JSHandle info = null;

                //verificamos si el boton conectar esta en el contenedor principal
                foreach (var content in contenedorbotones)
                {
                    try
                    {
                        await page.WaitForSelectorAsync("button");
                        var btns = await content.QuerySelectorAllAsync("button");
                        foreach (var btn in btns)
                        {
                            await Task.Delay(1000);
                            await page.WaitForSelectorAsync("span.artdeco-button__text");
                            var span = await btn.QuerySelectorAsync("span.artdeco-button__text");
                            var info2 = await span.GetPropertyAsync("innerText");
                            Console.WriteLine($"-> Contenido : {info2.ToString().Replace("JSHandle:", "")}");
                            if(info2.ToString().Replace("JSHandle:", "") == "Connect")
                            {
                                await btn.ClickAsync();
                                Console.WriteLine($"-> Boton encontrado");
                                btnconninmain = true;
                                break;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"-> Error buscando boton conectar en contenedor principal {ex.Message}");
                    }
                }

                //Verificamos si esta en el contenedor secundario
                if (!btnconninmain)
                {
                    //Precionamos el boton
                    foreach (var content in contenedorbotones)
                    {
                        try
                        {
                            var botonmasopciones = await content.QuerySelectorAsync("button[type='button'][aria-label='More actions']");
                            await botonmasopciones.ClickAsync();
                            info = await botonmasopciones.GetPropertyAsync("id");
                            Console.WriteLine("-> Menu desplegado");
                            break;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"-> Error desplegando el menu : {ex.Message}");
                        }
                    }

                    //Esperamos el despliegue del menu
                    await page.WaitForSelectorAsync($"div#ember{int.Parse(info.ToString().Replace("JSHandle:", "").Remove(0, 5)) + 1}");
                    var contenedoropciones = await page.QuerySelectorAsync($"div#ember{int.Parse(info.ToString().Replace("JSHandle:", "").Remove(0, 5))+1}");
                    
                    var xd = await contenedoropciones.QuerySelectorAllAsync("div[role='button'][tabindex='0']");
                    //Test generalizacion de mensajes
                    Console.WriteLine($"-> BOTONES {xd.Length}");
                    foreach (var opcion in xd)
                    {
                        bool verificacion = false;
                        await Task.Delay(500);
                        await page.WaitForSelectorAsync("span");
                        var span = await opcion.QuerySelectorAsync("span");
                        var info2 = await span.GetPropertyAsync("innerText");
                        Console.WriteLine($"-> Contenido : {info2.ToString().Replace("JSHandle:", "")}");
                        if (info2.ToString().Replace("JSHandle:", "") == "Connect")
                        {
                            Console.WriteLine($"-> Boton encontrado");
                            verificacion = true;
                        }
                        if (verificacion)
                        {
                            btnconninsec = true;
                            await opcion.ClickAsync();
                            break;
                        }
                        else
                        {
                            Console.WriteLine("-> Id erroneo no se encontro el boton");
                        }
                    }

                    await page.WaitForSelectorAsync($"div#ember{int.Parse(info.ToString().Replace("JSHandle:", "").Remove(0, 5)) + 5}");
                    var botoncontactar = await contenedoropciones.QuerySelectorAsync($"div#ember{int.Parse(info.ToString().Replace("JSHandle:", "").Remove(0, 5)) + 5}");

                    
                    //Console.WriteLine($"-> id opciones ember {int.Parse(info.ToString().Replace("JSHandle:", "").Remove(0, 5)) + 1}");
                    //Console.WriteLine($"-> id contentmessage ember {int.Parse(info.ToString().Replace("JSHandle:", "").Remove(0, 5)) + 5}");

                    //verificamos el boton
                   

                }
                
                //Buscamos el boton para enviar mensajes
                if(btnconninmain | btnconninsec)
                {
                    try
                    {
                        var contenedorMain = await page.QuerySelectorAsync("div#artdeco-modal-outlet");
                        var contenedor = await contenedorMain.QuerySelectorAsync("div[aria-hidden='false'].artdeco-modal-overlay");
                        var idcon = await contenedor.GetPropertyAsync("id");
                        var contenedoracciones = await contenedor.QuerySelectorAsync($"div#ember{int.Parse(idcon.ToString().Replace("JSHandle:ember", "")) + 4}");
                        var botones = await contenedoracciones.QuerySelectorAllAsync("button");
                        //Console.WriteLine($"-> Id : button#ember{int.Parse(idcon.ToString().Replace("JSHandle:ember", "")) + 5}");
                        await page.WaitForSelectorAsync($"button#ember{int.Parse(idcon.ToString().Replace("JSHandle:ember", "")) + 5}");
                        var boton = await contenedor.QuerySelectorAsync($"button#ember{int.Parse(idcon.ToString().Replace("JSHandle:ember", "")) + 5}");
                        await boton.ClickAsync();

                        if (botones.Length == 2)
                        {
                            Console.WriteLine("-> Escribiendo mensaje");
                            var txt = await page.WaitForSelectorAsync("textarea#custom-message");
                            //Limpiando Mensaje Clinete name
                        
                            var msg = message.message.Replace("(Account_First_Name)", account.account_name.Split(" ")[0]).Replace("(First Name)", client.client_name.Split(" ")[0]).Replace("(First_name)", client.client_name.Split(" ")[0]).Replace("{first_name}", client.client_name.Split(" ")[0]).Replace("(First_Name)", client.client_name.Split(" ")[0]).Replace("{First_Name}", client.client_name.Split(" ")[0]).Replace("(Account_First_Name)", account.account_name).Replace("(Account_First Name)", account.account_name).Replace(@"\n", "\n");
                            await txt.TypeAsync(msg);

                            var botonSUBMIT = await contenedor.QuerySelectorAsync($"button#ember{int.Parse(idcon.ToString().Replace("JSHandle:ember", "")) + 6}");
                            await botonSUBMIT.ClickAsync();
                            Console.WriteLine("-> Mensaje enviado");
                        }
                        else
                        {
                            var contenedores = await contenedor.QuerySelectorAllAsync("div.ember-view");
                            contenedoracciones = contenedores[2];
                            idcon = await contenedoracciones.GetPropertyAsync("id");
                            boton = await contenedor.QuerySelectorAsync($"button#ember{int.Parse(idcon.ToString().Replace("JSHandle:ember", "")) + 1}");
                            await boton.ClickAsync();

                            var txt = await page.WaitForSelectorAsync("textarea#custom-message");
                            var msg = message.message.Replace("(Account_First_Name)", account.account_name.Split(" ")[0]).Replace("(First Name)", client.client_name.Split(" ")[0]).Replace("(First_name)", client.client_name.Split(" ")[0]).Replace("{first_name}", client.client_name.Split(" ")[0]).Replace("(First_Name)", client.client_name.Split(" ")[0]).Replace("{First_Name}", client.client_name.Split(" ")[0]).Replace("(Account_First_Name)", account.account_name).Replace("(Account_First Name)", account.account_name).Replace(@"\n", "\n");
                            await txt.TypeAsync(msg);

                            var botonSUBMIT = await contenedor.QuerySelectorAsync($"button[aria-label='Send now']");
                            await botonSUBMIT.ClickAsync();
                            //idcon =await botonSUBMIT.GetPropertyAsync("id");
                            //Console.WriteLine(idcon);
                            Console.WriteLine("-> Mensaje enviado");
                        }

                        var insert = client_connections.CreateClient_Connections(database, new client_connections
                        {
                            client_id = client.client_id,
                            account_id = account.account_id,
                            fecha_inicio = Convert.ToInt64(stacks.unixdate),
                            fecha_finalizacion = (Convert.ToInt64(stacks.unixdate) + (28 * 86400))
                        });
                        if (insert) Console.WriteLine("-> registro guardado");
                        var stadistic = stacks.UpdateStacks(database, new stacks
                        {
                            invites = 0,
                            connections = 0,
                            replies = 0,
                            messages_send = 1,
                            fecha = Convert.ToInt64(stacks.unixdateday)
                        });
                        if (stadistic) Console.WriteLine("-> Estdistica guardada");
                    }
                    catch (WaitTaskTimeoutException ex)
                    {
                        Console.WriteLine("-> Objeto no reconocido");
                        try
                        {
                            var contenedorMain = await page.QuerySelectorAsync("div#artdeco-modal-outlet");
                            var H2 = await contenedorMain.QuerySelectorAsync("h2#send-invite-modal");
                            if(H2 != null)
                            {
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        catch (Exception e )
                        {

                        }
                    }
                    catch (NullReferenceException ex)
                    {
                        Console.WriteLine("-> Objeto no reconocido");
                        Console.WriteLine($"-> Error {ex.Message}");
                        var insert = client_connections.CreateClient_Connections(database, new client_connections
                        {
                            client_id = client.client_id,
                            account_id = account.account_id,
                            fecha_inicio = Convert.ToInt64(stacks.unixdate),
                            fecha_finalizacion = (Convert.ToInt64(stacks.unixdate) + ( 86400 * 28))
                        });
                        if (insert) Console.WriteLine("-> registro guardado");
                        var stadistic = stacks.UpdateStacks(database, new stacks
                        {
                            invites = 0,
                            connections = 0,
                            replies = 0,
                            messages_send = 1,
                            fecha = Convert.ToInt64(stacks.unixdateday)
                        });
                        if (stadistic) Console.WriteLine("-> Estdistica guardada");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("-> Excepcion no reconocida");
                        Console.WriteLine(ex.ToString());
                        if(account == null)
                        {
                            Console.WriteLine($"-> cuenta nula");

                        }else if(client == null)
                        {
                            Console.WriteLine($"-> cliente nula");
                        }else if (message == null)
                        {
                            Console.WriteLine($"-> mensaje nula");
                        }
                        else if (database == null)
                        {
                            Console.WriteLine($"-> database nula");
                        }
                    }

                }
                Console.WriteLine($"-------------------------------------------------------------------------");

                //delay de tarea entre cliente
                //var delay = new Random().Next(5000, 12000);
                //await Task.Delay(delay);
            }
        }
        
    }
}
