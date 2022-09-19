using System;
using System.IO;
using dotenv.net;
using System.Linq;
using Linkedin.Net;
using Linkedin_bot.db;
using System.Threading;
using Linkedin_bot.libs;
using Linkedin_bot.models;
using Linkedin_bot.config;
using dotenv.net.Utilities;
using Linkedin_bot.controllers;
using System.Collections.Generic;

namespace Linkedin_bot
{
    class index
    {
        static database database = database.getdatabase();
        //static database database1 = new database();
        //static Thread hiloNotificaciones;

         static  void  Main(string[] args)
        {
           
            DotEnv.Load(options: new DotEnvOptions(envFilePaths: new[] { Path.Combine(Directory.GetCurrentDirectory(), ".env") }));

            Console.WriteLine("Chat Id : "+Environment.GetEnvironmentVariable("chatid"));
            Console.WriteLine("Bot Token : " + Environment.GetEnvironmentVariable("bottoken"));
            //Notificaciones.SendNotificationBanned(new accounts(), new proxys()).Wait();
            //testebot.xd();

            //hiloNotificaciones = new Thread(() => { Notificaciones.MainLoop(database1).Wait(); });
            //hiloNotificaciones.Start();

            //Cliente cliente = new Cliente(accounts.SelectById(database,"1").account_cookie);
            //cliente.GetConversation();
            //sendmessage.Envio(40);
            //Notificaciones.SendNotification(accounts.SelectById(database,"1"),proxys.SelectById(database,"1"),clients.SelectById(database,"1")).Wait();
            //Notificaciones.getchats().Wait();
            //insert.insertdata(database);
            //insert.insertclients(database);
            //insert.insertmessage(database);

            //var testremove = new Remove_Connections();
            //testremove.loopMain(database);

            var l = new launch_browser();
            while (true)
            {
                l.loopMain(database);

                Thread.Sleep(new Random().Next(86300000, 86400000));
            }

            //Console.WriteLine(configuracion.linkedin.login.txtpass);
            Console.ReadLine();
        }
    }
   
}
