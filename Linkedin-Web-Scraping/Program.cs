using System;
using CsvHelper;
using System.Linq;
using PuppeteerSharp;
using Linkedin.Net.Models;
using System.Threading.Tasks;
using CsvHelper.Configuration;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

//Verificacion de usuario
//Console.Write("Ingrese usuario : ");
//var usuariosys = Console.ReadLine();
//if (usuariosys != "JoseLeonJL")
//{
//    Console.WriteLine("-> Error, usuario incorecto!!!");
//    return;
//}

//Console.Write("Ingrese la contraseña : ");
//var passwordsys = Console.ReadLine();
//if (passwordsys != "1234567890.")
//{
//    Console.WriteLine("-> Error, password incorecto!!!");
//    return;
//}
//Console.Clear();
Console.WriteLine("Cargando sistema...");

//Creamos el navegador
await new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);


var launcher = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = false, DefaultViewport = new ViewPortOptions { Width = 1900, Height = 960 } });
var url = "https://www.linkedin.com/uas/login?session_redirect=/sales&fromSignIn=true&trk=navigator";

var page = await launcher.NewPageAsync();
await page.GoToAsync(url);

//Login 
var correo = "nigel@nsnyre.com";
var password = "nigelsonigel";

try
{
    Console.WriteLine("-> Ingresando al login...");
    var txtuser = await page.QuerySelectorAsync("#username");
    await txtuser.TypeAsync(correo);
    var txtPass = await page.QuerySelectorAsync("#password");
    await txtPass.TypeAsync(password);
    await txtPass .PressAsync("Enter");
    Console.WriteLine("-> Ingreso exitoso!!!");
}
catch(Exception ex)
{
    Console.WriteLine("-> Error ingresando credenciales");
    return;
}

//Verificar la completacion de criterios de busquedas
Console.WriteLine("-> Realice la busqueda de los caracteres deseados");
Console.WriteLine("-> Ingrese la letra y|Y cuando termine :");
var respuesta = Console.ReadLine();
if(respuesta.ToLower() != "y")
{
    Console.WriteLine("-> Error, tecla no reconocida");
    return;
}

//Ingreso de cantidad maxima de datos
Console.WriteLine("-> Ingrese la cantidad de clientes maxima posible : ");
var numerodeclientes = int.Parse(Console.ReadLine());


var contenedor = await page.QuerySelectorAsync("#search-results-container");
var contenedorSiguiente = await contenedor.QuerySelectorAsync("div.mv4.justify-center");
var listaclientes = await contenedor.QuerySelectorAllAsync(".artdeco-list__item.pl3.pv3");
var button = await contenedorSiguiente.QuerySelectorAsync("button[aria-label='Siguiente']");

//Recuperacion de datos
Console.WriteLine("-> Capturando clientes...\n");
var Clientes = new List<clients>();
var count = 0;

try
{
    while (count < numerodeclientes)
    {
        try
        {
            await page.WaitForSelectorAsync("#search-results-container");
            contenedor = await page.QuerySelectorAsync("#search-results-container");
            await contenedor.ClickAsync();
            for (int i = 0; i < 6; i++)
            {
                await contenedor.PressAsync("Space");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("-> Error en el DOM");
        }
        
        await Task.Delay(8000);

        contenedorSiguiente = await contenedor.QuerySelectorAsync("div.mv4.justify-center");
        await page.WaitForSelectorAsync(".artdeco-list__item.pl3.pv3");
        listaclientes = await contenedor.QuerySelectorAllAsync(".artdeco-list__item.pl3.pv3");
        button = await contenedorSiguiente.QuerySelectorAsync("button[aria-label='Siguiente']");

        Console.WriteLine($"-> Clientes en pagina : {listaclientes.Length}");
        foreach (var cliente in listaclientes)
        {
            try
            {
                var data = await cliente.QuerySelectorAsync("a[data-control-name='view_profile_via_result_name'].ember-view");
                if (data == null)
                {
                    Console.WriteLine("-> Error, data es nulo");
                }
                var nombre = (await data.GetPropertyAsync("innerText")).ToString().Substring(9);
                var link = (await data.GetPropertyAsync("href")).ToString().Substring(9).Split(",")[0].Split("/")[5];
                link = $"https://www.linkedin.com/in/{link}";


                Clientes.Add(new clients { client_url = link, client_name = nombre, client_email = "" });
                Console.WriteLine("-> Capturado!!!");



                //await Task.Delay(1000);
                count += 1;
            }
            catch(Exception ex)
            {

            }
        }

        var result = (await button.GetPropertyAsync("disabled")).ToString().Substring(9);
        if (bool.Parse(result))
        {
            Console.WriteLine("-> El boton esta deshabilitado");
            break;
        }

        //Seguimos con el scraping y esperamos la carga de datos
        await button.ClickAsync();
        
        Console.Clear();
    }
    
}
catch (Exception ex)
{
    Console.WriteLine($"-> Error {ex}");
}

//Creamos y guardamos los datos
//Console.Clear();
Console.WriteLine("-> Guardando registros...");
using (var writer = new StreamWriter(Path.Combine(Directory.GetCurrentDirectory(), $"{count}-Clientes-{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}.csv")))
using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
{
    csv.WriteRecords(Clientes);
}
Console.WriteLine("-> Guardado!!!");

//cerramos el sistema
Console.WriteLine("-> Cerrando...");
await launcher.CloseAsync();
