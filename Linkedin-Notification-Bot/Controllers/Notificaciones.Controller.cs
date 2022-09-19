using System;
using MoreLinq;
using System.Text;
using System.Linq;
using Linkedin.Net;
using Telegram.Bot;
using Linkedin.Net.Db;
using System.Threading;
using Telegram.Bot.Types;
using Linkedin.Net.Models;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using System.Collections.Generic;
using Telegram.Bot.Extensions.Polling;

namespace Linkedin_Notification_Bot.Controllers
{
    internal class Notificaciones
    {
        //Variables
        private static ChatId chatid = new ChatId(Environment.GetEnvironmentVariable("chatid"));//"-746540648"
        private static string botid = Environment.GetEnvironmentVariable("token");
        public static async Task SenMessageTest()
        {
            try
            {
                var botclient = new TelegramBotClient(botid);
                using var cts = new CancellationTokenSource();
                Message
                message = await botclient.SendTextMessageAsync(
                    chatId: chatid,
                    text: "Testing...",
                    parseMode: ParseMode.Html);
                Console.WriteLine("-> Test Enviado");
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }
        //Metodos
        public static async Task SendNotificationMessage(accounts account, proxys proxy, string client, string mensaje)
        {
            var botclient = new TelegramBotClient(botid);
            using var cts = new CancellationTokenSource();
            //var stadistic = stacks.UpdateStacks(new database(), new stacks
            //{
            //    invites = 0,
            //    connections = 1,
            //    replies = 0,
            //    messages_send = 0,
            //    fecha = Convert.ToInt64(stacks.unixdateday)
            //});
            var me = await botclient.GetMeAsync();
            Message
            message = await botclient.SendTextMessageAsync(
                chatId: chatid,
                text: "<b>|✳️\t\t\tCliente respondio\t\t\t|</b>\n" +
                      $"<b>|\t\t\tCorreo: {account.account_mail}\t\t\t|</b>\n" +
                      $"<b>|\t\t\tContraseña: {account.account_linkedin_pass}\t\t\t|</b>\n" +
                      $"<b>|\t\t\tProxy : {proxy.proxy_ip}:{proxy.proxy_port}:{proxy.proxy_user}:{proxy.proxy_pass}\t\t\t|</b>\n" +
                      $"<b>|\t\t\tCliente : {client}\t\t\t|</b>\n" +
                      $"<b>|\t\t\tMensaje : {mensaje}\t\t\t|</b>\n",
                parseMode: ParseMode.Html);

        }
        public static async Task SendNotificationNeedVerification(accounts account)
        {
            var botclient = new TelegramBotClient(botid);
            using var cts = new CancellationTokenSource();
            var me = await botclient.GetMeAsync();
            Message
            message = await botclient.SendTextMessageAsync(
                chatId: chatid,
                text: "<b>|⚠\t\t\tEs necesario verificacion manual : \t\t\t|</b>\n" +
                      $"<b>| \t\t\tCorreo : {account.account_mail}\t\t\t|</b>\n" +
                      $"<b>| \t\t\tContraseña : {account.account_pass}\t\t\t|</b>\n" +
                      $"<b>| \t\t\tCorreo recuperacion : {account.account_recovery_mail}\t\t\t|</b>\n" +
                      $"<b>| \t\t\tContraseña recuperacion : {account.account_recovery_pass}\t\t\t|</b>\n" +
                      $"<b>| \t\t\tTiempo restante : 10 min\t\t\t|</b>\n",
                parseMode: ParseMode.Html);

        }
        public static async Task SendNotificationBanned(accounts account, proxys proxy)
        {
            var botclient = new TelegramBotClient(botid);
            using var cts = new CancellationTokenSource();
            var me = await botclient.GetMeAsync();
            Message
            message = await botclient.SendTextMessageAsync(
                chatId: chatid,
                text: "<b>|⚠️\t\t\tCuenta Baneada\t\t\t|</b>\n" +
                      $"<b>|\t\t\tCorreo: {account.account_mail}\t\t\t|</b>\n" +
                      $"<b>|\t\t\tContraseña: {account.account_linkedin_pass}\t\t\t|</b>\n" +
                      $"<b>|\t\t\tProxy : {proxy.proxy_ip}:{proxy.proxy_port}:{proxy.proxy_user}:{proxy.proxy_pass}\t\t\t|</b>\n",
                parseMode: ParseMode.Html);

        }
        public static async Task SendNotificationNoHaveClients()
        {
            var botclient = new TelegramBotClient(botid);
            using var cts = new CancellationTokenSource();
            var me = await botclient.GetMeAsync();
            Message
            message = await botclient.SendTextMessageAsync(
                chatId: chatid,
                text: "<b>|⚠️\t\t\tNo hay mas clientes\t\t\t|</b>\n",
                parseMode: ParseMode.Html);

        }
        public static async Task SendNotificationError(accounts account, string error)
        {
            var proxy = accounts_proxys.SelectByIdAccount(Linkedin.Net.Db.database.getdatabase(), account.account_id.ToString());
            proxy.proxy_banned = true;
            proxy.Update(Linkedin.Net.Db.database.getdatabase());
            var update = proxys.SelectProxysAvailable(Linkedin.Net.Db.database.getdatabase()).FirstOrDefault();
            //Actualizamos registros

            if (update != null)
            {
                var acconupdate = accounts_proxys.Selectaccounts_proxysByIdAccount(Linkedin.Net.Db.database.getdatabase(), account.account_id.ToString());
                acconupdate.proxy_id = update.proxy_id;
                acconupdate.update(Linkedin.Net.Db.database.getdatabase());

            }

            var botclient = new TelegramBotClient(botid);
            using var cts = new CancellationTokenSource();
            var me = await botclient.GetMeAsync();
            Message
            message = await botclient.SendTextMessageAsync(
                chatId: chatid,
                text: $"<b>|⚠️\t\t\t{error}\t\t\t|</b>\n" +
                      $"<b>|\t\t\tCorreo: {account.account_mail}\t\t\t|</b>\n" +
                      $"<b>|\t\t\tContraseña: {account.account_linkedin_pass}\t\t\t|</b>\n",
                parseMode: ParseMode.Html);

        }
        public static async Task MainLoop(database database)
        {

            var cuentassincookies = accounts.AccountsAllHaveCookies(database);
            var cuentas = accounts.SelectAccountsWithCookies(database);
            List<NotificacionesData> clientemensaje = new List<NotificacionesData>();
            Console.WriteLine("-> Bot trabajando!!!");
            while (true)
            {
                if (cuentassincookies != 0)
                {
                    Console.WriteLine("-> Faltan cookies en la base de datos");
                    if (accounts.AccountsAllHaveCookies(database) != cuentassincookies)
                    {
                        cuentas = accounts.SelectAccountsWithCookies(database);
                        Console.WriteLine("-> Actualizando Cuentas");
                    }
                }
                foreach (var cuenta in cuentas)
                {
                    try
                    {
                        var cliente = new Cliente(cuenta.account_cookie);
                        var Mensajes = await cliente.GetConversation();

                        foreach (var Mensaje in Mensajes)
                        {
                            var notexist = (from msm in clientemensaje where msm.Id == Mensaje.Id select msm).FirstOrDefault();
                            if (notexist == null)
                            {
                                await SendNotificationMessage(cuenta, proxys.SelectById(database, cuenta.account_id.ToString()), Mensaje.Cliente, Mensaje.Mensaje);
                                clientemensaje.Add(Mensaje);
                            }
                        }
                        await Task.Delay(new Random().Next(5, 10) * 1000);

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("-> Error main Notificaciones");
                    }
                }
                await Task.Delay(new Random().Next(5, 10) * 10000);
            }
        }
    }
}
