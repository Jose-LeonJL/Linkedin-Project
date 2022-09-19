using System;
using Linkeding_Bot_Telegram.Telegram_Engine;
using Linkedin.Net.Db;

namespace Linkeding_Bot_Telegram
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("-> Iniciando bot...");
            Console.WriteLine("-> Iniciando base de datos...");
            var db = new database(false);

            Console.WriteLine("-> Cargando Motor de consultas...");
            var motor = new Engine();
            motor.Load();
        }
    }
}
