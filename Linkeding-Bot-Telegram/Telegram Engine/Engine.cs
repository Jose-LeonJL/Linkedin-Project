using System;
using MoreLinq;
using System.Text;
using System.Linq;
using Telegram.Bot;
using System.Threading;
using Telegram.Bot.Types;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using Linkeding_Bot_Telegram.Controllers;
using Telegram.Bot.Args;

namespace Linkeding_Bot_Telegram.Telegram_Engine
{
    internal class Engine
    {
        //Variables
        private string BotId;
        private TelegramBotClient BotClient;
        private List<BotCommand> Commands;
        private User My;
        //Constructor
        public Engine()
        {
            BotId = Environment.GetEnvironmentVariable("token");//"5340226338:AAG84jlSIQ53Kk2ow-Q80fHlqvh4VIYJo1M";;
            BotClient = new TelegramBotClient(BotId);
            BotClient.OnMessage += OnMessage;
            BotClient.OnMessageEdited += OnMessageEdited;
            BotClient.OnCallbackQuery += OnCallbackQuery;

            Commands = new List<BotCommand>();
            Commands.Add(new BotCommand { Command = "start", Description = "Inicia el bot" });   
        }

        private async void OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            try
            {
                var query = e.CallbackQuery;
                switch (query.Data)
                {
                    case "StacksDiario":
                        Stacks.StacksDiario(BotClient, query);
                        break;
                    case "StacksSemanal":
                        await BotClient.EditMessageTextAsync(query.From.Id, query.Message.MessageId, "La opcion no esta disponible en este momento.");
                        break;
                    case "StacksTotal":
                        Stacks.StacksTotal(BotClient, query);
                        break;
                    case "StacksDiarioPorCuenta":
                        await BotClient.EditMessageTextAsync(query.From.Id, query.Message.MessageId, "La opcion no esta disponible en este momento.");
                        break;
                    case "StacksTotalPorCuenta":
                        Stacks.StacksTotalPorCuenta(BotClient, query);
                        break;
                    case "StacksExcelOptions":
                        await BotClient.EditMessageTextAsync(query.From.Id, query.Message.MessageId, "La opcion no esta disponible en este momento.");
                        break;
                    case "ProxysBanned":
                        Proxys.ProxyBanned(BotClient, query);
                        break;
                    case "ProxysAgregar":
                        await BotClient.EditMessageTextAsync(query.From.Id, query.Message.MessageId, "La opcion no esta disponible en este momento.");
                        break;
                    case "AccountsBanned":
                        Accounts.AccountsBanned(BotClient, query);
                        break;
                    case "AccountsAgregar":
                        await BotClient.EditMessageTextAsync(query.From.Id, query.Message.MessageId, "La opcion no esta disponible en este momento.");
                        break;
                    case "AccountsWorking":
                        Accounts.AccountsWorking(BotClient, query);
                        break;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine($"-> Error OnCallbackQuery {ex}");
            }
            
        }

        private async void OnMessageEdited(object sender, MessageEventArgs e)
        {
            try
            {

            }catch(Exception ex)
            {
                Console.WriteLine($"-> Error OnMessageEdited {ex}");
            }
        }

        private async void OnMessage(object sender, MessageEventArgs e)
        {
            try
            {
                var mensaje = e.Message;
            

                Console.WriteLine(mensaje.Text);

                if (mensaje.Text.ToLower() == $"/start@{My.FirstName}")
                {
                    await BotClient.SendTextMessageAsync(mensaje.Chat.Id, "Menu Principal", replyMarkup: CreateMainMenu());
                }

                switch (mensaje.Text.ToLower())
                {
                    case "stacks":
                        await BotClient.SendChatActionAsync(mensaje.Chat.Id, ChatAction.Typing);
                        await BotClient.SendTextMessageAsync(mensaje.Chat.Id, "Menu Stacks", replyMarkup: Stacks.CreateMenuStacks());
                        break;
                    case "proxys":
                        await BotClient.SendChatActionAsync(mensaje.Chat.Id, ChatAction.Typing);
                        await BotClient.SendTextMessageAsync(mensaje.Chat.Id, "Menu Proxys", replyMarkup: Proxys.CreateMenuProxys());
                        break;
                    case "accounts":
                        await BotClient.SendChatActionAsync(mensaje.Chat.Id, ChatAction.Typing);
                        await BotClient.SendTextMessageAsync(mensaje.Chat.Id, "Menu Accounts", replyMarkup: Accounts.CreateMenuAccounts());
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"-> Error {ex}");
            }  
        }

        //Metodos
        public void Load()
        {  
            //Cargamos commandos
            BotClient.SetMyCommandsAsync(Commands).Wait();
            My = BotClient.GetMeAsync().GetAwaiter().GetResult();
            BotClient.StartReceiving();
            Console.ReadLine();
        }

        
        private ReplyKeyboardMarkup CreateMainMenu()
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new[]
            {
                new[]{"Stacks","Proxys","Accounts"}
            };

            
            replyKeyboardMarkup.ResizeKeyboard = true;
            replyKeyboardMarkup.OneTimeKeyboard = true;
            return replyKeyboardMarkup;
        }
       
    }
}
