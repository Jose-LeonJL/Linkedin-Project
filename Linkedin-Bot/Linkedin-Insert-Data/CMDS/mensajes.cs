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
    public class message
    {
        public static void insertMnesajes()
        {
            var mensajes = JsonSerializer.Deserialize<List<messages>>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "data", "Accounts - Modelo-Mensajes.json")));
            Console.WriteLine($"-> Ingresando Mensajes...");
            Console.WriteLine($"-> Mensajes totales {mensajes.Count}");
            //mensajes = mensajes.DistinctBy(x => x.message).ToList();

            foreach (var mensaje in mensajes)
            {
                try
                {
                    var cuentas = cuentass.SelectAccountWithoutMessage(Database.getdatabase(), mensaje.message_type_id.ToString());
                    messages.CreateMessagesWithId(mensaje);
                    message_accounts.Createmessage_accounts(Database.getdatabase(), new message_accounts
                    { 
                        account_id = mensaje.message_id,
                        message_id = mensaje.message_id
                    });
                }catch(Exception ex)
                {
                    Console.WriteLine("-> Error ingresando mensaje!!!");
                    Console.WriteLine($"-> Error {ex.Message}");
                    Console.WriteLine($"<-------------------------------------------->");
                }
            }

            Console.WriteLine($"-> Mensajes ingresados");
            Console.WriteLine($"<-------------------------------------------->");
        }
        public static void ModelateMessage()
        {
            var mensajes = JsonSerializer.Deserialize<List<messages>>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "data", "Accounts - Modelo-Mensajes.json")));
            Console.WriteLine($"-> modelando Mensajes...");
            Console.WriteLine($"-> Mensajes totales {mensajes.Count}");
            //mensajes = mensajes.DistinctBy(x => x.message).ToList();

            for (int i = 1; i < mensajes.Count; i++)
            {
                mensajes[i-1].message_id = i;
                Console.WriteLine(mensajes[i].message_id);
            }

            File.WriteAllText(Path.Combine(Directory.GetCurrentDirectory(), "data", "Accounts - Modelo-Mensajes.json"), JsonSerializer.Serialize(mensajes));
            Console.WriteLine($"-> Mensajes ingresados");
            Console.WriteLine($"<-------------------------------------------->");
        }
    }
}
