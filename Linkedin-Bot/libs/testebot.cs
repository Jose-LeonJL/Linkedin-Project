using System;
using Linkedin_bot.models;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Linkedin_bot.db;
using Telegram.Bot.Types.ReplyMarkups;

namespace Linkedin_bot
{
    class testebot
    {
        public async static void xd()
        {
            var botid = Environment.GetEnvironmentVariable("bottoken");
            //old bot "5108257872:AAGxhAXu3ieSdZ0GgTSbe62_Df2BCOA_Mh8"
            var botClient = new TelegramBotClient(botid);
            using var cts = new CancellationTokenSource();

            // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = { } // receive all update types
            };
            botClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                receiverOptions,
                cancellationToken: cts.Token);
            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            cts.Cancel();
            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                // Only process Message updates: https://core.telegram.org/bots/api#message
                if (update.Type != UpdateType.Message)
                    return;
                // Only process text messages
                if (update.Message!.Type != MessageType.Text)
                    return;

                var chatId = update.Message.Chat.Id;
                var messageText = update.Message.Text;

                

                if (messageText.ToLower() == "/stacks")
                {
                    var stadisticas = stacks.SelectAll();
                    var mensaje = "";
                    foreach (var stack in stadisticas)
                    {
                        mensaje += $"Mensajes enviados : {stack.messages_send}\nFecha : {stacks.UnixTimeStampToDateTime(stack.fecha)}\n";
                        mensaje += "-------------------------------------\n";
                    }
                    mensaje += $"Total mensajes enviados: {(from es in stadisticas select es.messages_send).Sum()}\n";
                   
                    var sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: mensaje,
                        cancellationToken: cancellationToken);
                }else if (messageText.ToLower().Split(" ")[0] == "/proxys")
                {
                    if((messageText.ToLower().Split(" ")[1] == "banned"))
                    {

                        var proxy = proxys.SelectProxysBanned(database.getdatabase());
                        var mensaje = "Lista de proxys baneados : \n";
                        foreach (var px in proxy)
                        {
                            mensaje += $"{px.proxy_ip}:{px.proxy_port}:{px.proxy_user}:{px.proxy_pass}";
                        }
                        var sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: mensaje,
                        cancellationToken: cancellationToken);
                    }
                    else
                    {
                        var sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "El comando no es una funcion de proxy",
                        cancellationToken: cancellationToken);
                    }
                }else if (messageText.ToLower().Split(" ")[0] == "/accounts")
                {
                    if ((messageText.ToLower().Split(" ")[1] == "banned"))
                    {

                        var proxy = accounts.SelectAccountsBanned(database.getdatabase());
                        var mensaje = "Lista de cuentas baneadas : \n";
                        foreach (var px in proxy)
                        {
                            mensaje += $"Nombre{px.account_name} correo : {px.account_mail}";
                        }
                        var sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: mensaje,
                        cancellationToken: cancellationToken);
                    }
                    else
                    {
                        var sentMessage = await botClient.SendTextMessageAsync(
                        chatId: chatId,
                        text: "El comando no es una funcion de accouns",
                        cancellationToken: cancellationToken);
                    }
                }

                // Echo received message text
               
            }
            Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }
    }
}
