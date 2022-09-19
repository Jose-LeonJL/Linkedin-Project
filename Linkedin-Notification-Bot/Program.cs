using System;
using Linkedin.Net.Db;
using Linkedin_Notification_Bot.Controllers;

Console.WriteLine("-> Iniciando Bot...");

Console.WriteLine($"-> Chat :{Environment.GetEnvironmentVariable("chatid")}");
Console.WriteLine($"-> database_host :{Environment.GetEnvironmentVariable("database_host")}");
Console.WriteLine($"-> database_port :{Environment.GetEnvironmentVariable("database_port")}");
Console.WriteLine($"-> database_name :{Environment.GetEnvironmentVariable("database_name")}");
Console.WriteLine($"-> database_user :{Environment.GetEnvironmentVariable("database_user")}");
Console.WriteLine($"-> database_password :{Environment.GetEnvironmentVariable("database_password")}");
Console.WriteLine($"-> token :{Environment.GetEnvironmentVariable("token")}");

Console.WriteLine("-> Enviando Test...");
await Notificaciones.SenMessageTest();

Console.WriteLine("-> Creando Instancia de la base de datos...");
database database = new database(false);

Console.WriteLine("-> Cargando motor de mensajeria...");
Notificaciones.MainLoop(database).Wait();