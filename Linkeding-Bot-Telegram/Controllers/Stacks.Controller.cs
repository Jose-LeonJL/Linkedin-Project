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
    internal class Stacks
    {
        //Metodos y funciones
        public static async void StacksDiario(TelegramBotClient botClient, CallbackQuery e)
        {
            var datos = stacks.SelectStacksDiarios();
            var mensajetxt = "";
            if (datos.Count == 0)
            {
                mensajetxt = "<pre>No hay datos hoy.</pre>";
            }
            else
            {
                mensajetxt ="<pre>"+ datos.ToStringTable(new[] { "Conexiones", "Fecha" }, a => a.Item1, a => $"{stacks.UnixTimeStampToDateTime(a.Item2).Day}/{stacks.UnixTimeStampToDateTime(a.Item2).Month}/{stacks.UnixTimeStampToDateTime(a.Item2).Year}") +"</pre>";
            }

            botClient.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
            await botClient.SendTextMessageAsync(e.Message.Chat.Id, mensajetxt, ParseMode.Html);
        }

        public static async void StacksTotal(TelegramBotClient botClient, CallbackQuery e)
        {
            var datos = stacks.SelectStacksTotales();
            var mensajetxt = "";
            if(datos.Count == 0)
            {
                mensajetxt = "<pre>No hay datos hoy.</pre>";
                botClient.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                await botClient.SendTextMessageAsync(e.Message.Chat.Id, mensajetxt, ParseMode.Html);
            }
            else
            {
                using (var writer = new StreamWriter(Path.Join(Directory.GetCurrentDirectory(), "Stacks-Totales.csv")))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(datos);
                }

                botClient.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                await using Stream stream = System.IO.File.OpenRead(Path.Join(Directory.GetCurrentDirectory(), "Stacks-Totales.csv"));
                await botClient.SendDocumentAsync(
                    chatId:e.Message.Chat.Id,
                    document: new InputOnlineFile(content: stream, fileName: "Stacks-Totales.csv")
                    );
            }
        }

        public static async void StacksTotalPorCuenta(TelegramBotClient botClient, CallbackQuery e)
        {
            var data = stacks.SelectStacksTotalesPorCuenta();
            var mensajetxt = "";
            if (data.Count == 0)
            {
                mensajetxt = "<pre>No hay datos.</pre>";
                botClient.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                await botClient.SendTextMessageAsync(e.Message.Chat.Id, mensajetxt, ParseMode.Html);
            }
            else
            {
                using (var writer = new StreamWriter(Path.Join(Directory.GetCurrentDirectory(), "Stacks-PorCuentas.csv")))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.WriteRecords(data   );
                }

                botClient.DeleteMessageAsync(e.Message.Chat.Id, e.Message.MessageId);
                await botClient.SendDocumentAsync(e.Message.Chat.Id, new InputOnlineFile(Path.Join(Directory.GetCurrentDirectory(), "Stacks-PorCuentas.csv")));
            }
        }

        //Menus de stacks
        public static InlineKeyboardMarkup CreateMenuStacks()
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            buttons.Add(new InlineKeyboardButton() { Text = "Diario", CallbackData = $"StacksDiario" });
            buttons.Add(new InlineKeyboardButton() { Text = "Semanal", CallbackData = $"StacksSemanal" });
            buttons.Add(new InlineKeyboardButton() { Text = "Total", CallbackData = $"StacksTotal" });

            buttons.Add(new InlineKeyboardButton() { Text = "Diario por cuenta", CallbackData = $"StacksDiarioPorCuenta" });
            buttons.Add(new InlineKeyboardButton() { Text = "Total por cuenta", CallbackData = $"StacksTotalPorCuenta" });
            buttons.Add(new InlineKeyboardButton() { Text = "Excel options", CallbackData = $"StacksExcelOptions"});
            
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
        public static InlineKeyboardMarkup CreateMenuExcell()
        {
            List<InlineKeyboardButton> buttons = new List<InlineKeyboardButton>();
            buttons.Add(new InlineKeyboardButton() { Text = "Diario", CallbackData = $"StacksExcelDiario" });
            buttons.Add(new InlineKeyboardButton() { Text = "Semanal", CallbackData = $"StacksExcelSemanal" });
            buttons.Add(new InlineKeyboardButton() { Text = "Total", CallbackData = $"StacksExcelTotal" });
            buttons.Add(new InlineKeyboardButton() { Text = "Diario por cuenta", CallbackData = $"StacksExcelDiarioPorCuenta" });
            buttons.Add(new InlineKeyboardButton() { Text = "Semanal por cuenta", CallbackData = $"StacksExcelSemanalPorCuenta" });
            buttons.Add(new InlineKeyboardButton() { Text = "Regresar", CallbackData = $"StacksExcelRegresar" });
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
    public static class TableParser
    {
        public static string ToStringTable<T>(
          this IEnumerable<T> values,
          string[] columnHeaders,
          params Func<T, object>[] valueSelectors)
        {
            return ToStringTable(values.ToArray(), columnHeaders, valueSelectors);
        }

        public static string ToStringTable<T>(
          this T[] values,
          string[] columnHeaders,
          params Func<T, object>[] valueSelectors)
        {

            var arrValues = new string[values.Length + 1, valueSelectors.Length];

            // Fill headers
            for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
            {
                arrValues[0, colIndex] = columnHeaders[colIndex];
            }

            // Fill table rows
            for (int rowIndex = 1; rowIndex < arrValues.GetLength(0); rowIndex++)
            {
                for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
                {
                    arrValues[rowIndex, colIndex] = valueSelectors[colIndex]
                      .Invoke(values[rowIndex - 1]).ToString();
                }
            }

            return ToStringTable(arrValues);
        }

        public static string ToStringTable(this string[,] arrValues)
        {
            int[] maxColumnsWidth = GetMaxColumnsWidth(arrValues);
            var headerSpliter = new string('-', maxColumnsWidth.Sum(i => i + 3) - 1);

            var sb = new StringBuilder();
            for (int rowIndex = 0; rowIndex < arrValues.GetLength(0); rowIndex++)
            {
                for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
                {
                    // Print cell
                    string cell = arrValues[rowIndex, colIndex];
                    cell = cell.PadRight(maxColumnsWidth[colIndex]);
                    sb.Append(" | ");
                    sb.Append(cell);
                }

                // Print end of line
                sb.Append(" | ");
                sb.AppendLine();

                // Print splitter
                if (rowIndex == 0)
                {
                    sb.AppendFormat(" |{0}| ", headerSpliter);
                    sb.AppendLine();
                }
            }

            return sb.ToString();
        }

        private static int[] GetMaxColumnsWidth(string[,] arrValues)
        {
            var maxColumnsWidth = new int[arrValues.GetLength(1)];
            for (int colIndex = 0; colIndex < arrValues.GetLength(1); colIndex++)
            {
                for (int rowIndex = 0; rowIndex < arrValues.GetLength(0); rowIndex++)
                {
                    int newLength = arrValues[rowIndex, colIndex].Length;
                    int oldLength = maxColumnsWidth[colIndex];

                    if (newLength > oldLength)
                    {
                        maxColumnsWidth[colIndex] = newLength;
                    }
                }
            }

            return maxColumnsWidth;
        }
    }
}
