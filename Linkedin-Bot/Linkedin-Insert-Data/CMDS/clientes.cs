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
    public static class clientesS
    {
        public static void insertclientes()
        {
            var clientes = JsonSerializer.Deserialize<List<clients>>(File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "data", "3500 clientes - Modelo-Clientes.json")));
            Console.WriteLine($"-> Ingresando clientes...");
            Console.WriteLine($"-> Clientes totales {clientes.Count}");
            //clientes = clientes.DistinctBy(x=>  x.client_url).ToList();
            
            foreach(var cliente in clientes)
            {
                try
                {
                    cliente.create_at = clients.unixdateday;
                    clients.CreateClients(cliente);
                }
                catch (Exception EX)
                {
                    Console.WriteLine("-> Error ingresando cliente!!!");
                    Console.WriteLine($"-> Error {EX.Message}");
                    Console.WriteLine($"<-------------------------------------------->");
                }
            }

            Console.WriteLine($"-> Clientes ingresados");
            Console.WriteLine($"<-------------------------------------------->");
        }
    }
}
