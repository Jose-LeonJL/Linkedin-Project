using System;
using MoreLinq;
using System.IO;
using CsvHelper;
using System.Text;
using System.Linq;
using Telegram.Bot;
using Linkedin.Net.Db;
using System.Threading;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Linkedin.Net.Models;
using System.Globalization;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using System.Collections.Generic;
using Telegram.Bot.Types.ReplyMarkups;
using Linkeding_Bot_Telegram.Controllers;

namespace Linkeding_Bot_Telegram.Controllers
{
    internal class Accounts
    {
        //Metodos
        public static async void AccountsBanned(TelegramBotClient botClient, CallbackQuery e)
        {
            var datos = accounts.SelectAccountsBanned(database.getdatabase());
            var mensajetxt = "";
            if (datos.Count == 0)
            {
                mensajetxt = "<pre>No hay datos.</pre>";
                botClient.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                await botClient.SendTextMessageAsync(e.Message.Chat.Id,mensajetxt,ParseMode.Html);
            }
            else
            {
                using (var writer = new StreamWriter(Path.Join(Directory.GetCurrentDirectory(),"cuentas-baneadas.csv")))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(datos);
                }
                
                botClient.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                await botClient.SendDocumentAsync(e.Message.Chat.Id, new InputOnlineFile(Path.Join(Directory.GetCurrentDirectory(), "cuentas-baneadas.csv")));
            } 
        }

        public static async void AccountsWorking(TelegramBotClient botClient, CallbackQuery e)
        {
            var datos = accounts.SelectAccountsAviable(database.getdatabase());
            var mensajetxt = "";
            if (datos.Count == 0)
            {
                mensajetxt = "<pre>No hay datos.</pre>";
                botClient.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                await botClient.SendTextMessageAsync(e.Message.Chat.Id, mensajetxt, ParseMode.Html);
            }
            else
            {
                using (var writer = new StreamWriter(Path.Join(Directory.GetCurrentDirectory(), "cuentas-Trabajando.csv")))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(datos);
                }

                botClient.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                await botClient.SendDocumentAsync(e.Message.Chat.Id, new InputOnlineFile(Path.Join(Directory.GetCurrentDirectory(), "cuentas-Trabajando.csv")));
            }
        }

        //Sub menus
        public static InlineKeyboardMarkup CreateMenuAccounts()
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            buttons.Add(new InlineKeyboardButton() { Text = "Banned", CallbackData = $"AccountsBanned" });
            buttons.Add(new InlineKeyboardButton() { Text = "Working", CallbackData = $"AccountsWorking" });
            buttons.Add(new InlineKeyboardButton() { Text = "Agregar", CallbackData = $"AccountsAgregar" });

            var menuDosColumnas = new List<InlineKeyboardButton[]>();
            for (var i = 0; i < buttons.Count; i++)
            {
                if (buttons.Count - 1 == i)
                {
                    menuDosColumnas.Add(new[] { buttons[i] });
                }
                else
                    menuDosColumnas.Add(new[] { buttons[i], buttons[i + 1] });
                i++;
            }

            //Creamos el KeyBoard 
            var menu = new InlineKeyboardMarkup(menuDosColumnas.ToArray());
            return menu;
        }
    }
}
