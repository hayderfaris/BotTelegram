using ExampleTelegramBot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace WindwosBotTimeCurrency
{
    public class Bot
    {
        static ITelegramBotClient botClient;
        static long ChatId = 136657377;
        public string _BotToken;
        static string TimeApi = "http://worldtimeapi.org/api/timezone";
        static string CurrencyApi = "http://data.fixer.io/api/latest?access_key=9a2b8a5214a7c05a1bdfba20d385095c";

        public Bot()
        {

        }
        public Bot(string botToken)
        {
            _BotToken = botToken;
        }

        public void Start()
        {
            try
            {
                botClient = new TelegramBotClient(_BotToken);

                botClient.SetMyCommandsAsync(new List<BotCommand>
                {
                new BotCommand { Command = "start", Description = "start bot"},
                new BotCommand { Command = "currency", Description = "$$$"},
                new BotCommand { Command = "time", Description = "time"},
                });

                botClient.OnMessage += Bot_OnMessage;
                botClient.OnCallbackQuery += Bot_OnCallbackQuery;

                botClient.StartReceiving();
            }
            catch(Exception e) 
            { 
                MessageBox.Show(e.Message); 
            }           

        }

        public void Stop()
        {
            try
            {
                botClient.StopReceiving();
            }
            catch { }
        }



        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            ChatId = e.Message.Chat.Id;
            if (e.Message.Text == "/start") { StartBot(); return; }
            if (e.Message.Text == "/currency") { CurrencyBot(); return; }
            if (e.Message.Text == "/time") { TimeBot(TimeApi); return; }

            if (e.Message.Text.Contains("/c ") && e.Message.Text.Split(' ').Length > 1)
            { CurrencyFunction(e.Message.Text); return; }


            await botClient.SendTextMessageAsync(
                chatId: ChatId,
                text: @"please type valid command .. start with /start",
                parseMode: ParseMode.Markdown,
                disableNotification: true
                );

        }

        static async void Bot_OnCallbackQuery(object sender, CallbackQueryEventArgs e)
        {
            if (e.CallbackQuery.Data == "currency") { CurrencyBot(); return; }
            if (e.CallbackQuery.Data == "time") { TimeBot(TimeApi); return; }

            var ArrayData = e.CallbackQuery.Data.Split('/');
            if (ArrayData.Length > 0) { TimeBot(TimeApi + "/" + e.CallbackQuery.Data); return; }
        }



        public static async void StartBot()
        {
            var Inline = new InlineKeyboardMarkup(new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton{ Text = "time country", CallbackData = "time"},
                new InlineKeyboardButton{ Text = "currency exchange", CallbackData = "currency"}
            });

            await botClient.SendTextMessageAsync(
                chatId: ChatId,
                text: "Welcome in this bot, you can use one of these commands below:",
                parseMode: ParseMode.Markdown,
                disableNotification: true,
                replyMarkup: Inline
          );
        }

        public static async void CurrencyBot()
        {
            await botClient.SendTextMessageAsync(
                chatId: ChatId,
                text: "type (FROM currency code) then (TO currency code) like:" + Environment.NewLine
                         + @"/c `USD IQD`" + Environment.NewLine
                         + "optionally you can add money amount like:" + Environment.NewLine
                         + @"/c `USD IQD` 100",
                parseMode: ParseMode.Markdown,
                disableNotification: true
          );
        }

        public static async void CurrencyFunction(string command)
        {
            var CuntiresCodes = new List<string>
            {
                "AED","AFN","ALL","AMD","ANG","AOA","ARS","AUD","AWG","AZN","BAM","BBD","BDT","BGN",
                "BHD","BIF","BMD","BND","BOB","BRL","BSD","BTC","BTN","BWP","BYN","BYR","BZD","CAD",
                "CDF","CHF","CLF","CLP","CNY","COP","CRC","CUC","CUP","CVE","CZK","DJF","DKK","DOP",
                "DZD","EGP","ERN","ETB","EUR","FJD","FKP","GBP","GEL","GGP","GHS","GIP","GMD","GNF",
                "GTQ","GYD","HKD","HNL","HRK","HTG","HUF","IDR","ILS","IMP","INR","IQD","IRR","ISK",
                "JEP","JMD","JOD","JPY","KES","KGS","KHR","KMF","KPW","KRW","KWD","KYD","KZT","LAK",
                "LBP","LKR","LRD","LSL","LTL","LVL","LYD","MAD","MDL","MGA","MKD","MMK","MNT","MOP",
                "MRO","MUR","MVR","MWK","MXN","MYR","MZN","NAD","NGN","NIO","NOK","NPR","NZD","OMR",
                "PAB","PEN","PGK","PHP","PKR","PLN","PYG","QAR","RON","RSD","RUB","RWF","SAR","SBD",
                "SCR","SDG","SEK","SGD","SHP","SLL","SOS","SRD","STD","SVC","SYP","SZL","THB","TJS",
                "TMT","TND","TOP","TRY","TTD","TWD","TZS","UAH","UGX","USD","UYU","UZS","VEF","VND",
                "VUV","WST","XAF","XAG","XAU","XCD","XDR","XOF","XPF","YER","ZAR","ZMK","ZMW","ZWL"
            };
            var arrayCommand = command.Split(' ');
            var validCommand = arrayCommand[0];
            var FromCode = arrayCommand[1];
            var ToCode = arrayCommand[2];

            var Quantity = 1;
            if (arrayCommand.Length > 3)
                Quantity = int.Parse(arrayCommand[3]);

            if (!(validCommand == @"/c" &&
                  CuntiresCodes.Contains(FromCode) &&
                  CuntiresCodes.Contains(ToCode)))
            {
                await botClient.SendTextMessageAsync(
                chatId: ChatId,
                text: "err: please type (FROM currency code) then (TO currency code) like:"
                        + Environment.NewLine + @"/c `USD IQD`",
                parseMode: ParseMode.Markdown,
                disableNotification: true
                );
                return;
            }

            //var test = new CurrencyObject
            //{
            //    USD = 1.13285,
            //    IQD = 1654.527236,
            //};
            //var FromValue = (double)GetPropValue(test, FromCode);
            //var ToValue = (double)GetPropValue(test, ToCode);

            var data = await Http<CurrencyGlobal>.clint(CurrencyApi);
            var FromValue = (double)GetPropValue(data.rates, FromCode);
            var ToValue = (double)GetPropValue(data.rates, ToCode);

            //var exchangeValue = String.Format("1 {0} {1}",{ FromCode,ToCode});
            var exchangeValue = String.Format("1 {0} = {1:n4} {2}", FromCode, Math.Round((ToValue / FromValue), 4, MidpointRounding.AwayFromZero), ToCode) + Environment.NewLine;
            exchangeValue += String.Format("1 {0} = {1:n4} {2}", ToCode, Math.Round((FromValue / ToValue), 4, MidpointRounding.AwayFromZero), FromCode) + Environment.NewLine;

            if (Quantity != 1)
                exchangeValue += Quantity + String.Format(" {0} = {1:n4} {2}", FromCode, Math.Round((Quantity * (ToValue / FromValue)), 4, MidpointRounding.AwayFromZero), ToCode) + Environment.NewLine;

            //Console.WriteLine(exchangeValue);




            //var result =
            //          "City : " + data.timezone + Environment.NewLine
            //        + "Date : " + data.datetime.Split("T")[0] + Environment.NewLine
            //        + "Time : " + data.datetime.Split("T")[1].Split(".")[0] + Environment.NewLine
            //        + "UTC : " + data.utc_offset;

            await botClient.SendTextMessageAsync(
                chatId: ChatId,
                text: exchangeValue,
                parseMode: ParseMode.Markdown,
                disableNotification: true
                );

        }

        public static async void TimeBot(string URL)
        {
            var ArrayUrl = URL.Split('/').Length;

            if (ArrayUrl == 5)
            {
                var countries = new List<string>
                {
                    "Africa","America","Antarctica",
                    "Asia","Atlantic","Australia",
                    "Europe","Indian","Pacific","Etc"
                };

                var buttons = new List<List<InlineKeyboardButton>>();
                var buttonsInline = new List<InlineKeyboardButton>();

                for (int i = 0; i < countries.Count; i++)
                {
                    if (i > 0 && i % 3 == 0)
                    {
                        buttons.Add(buttonsInline);
                        buttonsInline = new List<InlineKeyboardButton>();
                    }
                    buttonsInline.Add(new InlineKeyboardButton { Text = countries[i], CallbackData = countries[i] });
                }
                buttons.Add(buttonsInline);

                await botClient.SendTextMessageAsync(
                    chatId: ChatId,
                    text: "Choose from these continents",
                    parseMode: ParseMode.Markdown,
                    disableNotification: true,
                    replyMarkup: new InlineKeyboardMarkup(buttons)
                    );
            }

            if (ArrayUrl == 6)
            {
                var data = await Http<List<string>>.clint(URL);
                var buttons = new List<List<InlineKeyboardButton>>();
                var buttonsInline = new List<InlineKeyboardButton>();

                for (int i = 0; i < data.ToArray().Length - 1; i++)
                {
                    if (i > 0 && i % 3 == 0)
                    {
                        buttons.Add(buttonsInline);
                        buttonsInline = new List<InlineKeyboardButton>();
                    }
                    buttonsInline.Add(new InlineKeyboardButton { Text = data[i].Split('/')[1], CallbackData = data[i] });
                }
                buttons.Add(buttonsInline);

                await botClient.SendTextMessageAsync(
                    chatId: ChatId,
                    text: "Choose from these countires",
                    parseMode: ParseMode.Markdown,
                    disableNotification: true,
                    replyMarkup: new InlineKeyboardMarkup(buttons)
                    );

            }

            if (ArrayUrl == 7)
            {
                var data = await Http<TimeObject>.clint(URL);
                var result =
                      "City : " + data.timezone + Environment.NewLine
                    + "Date : " + data.datetime.Split('T')[0] + Environment.NewLine
                    + "Time : " + data.datetime.Split('T')[1].Split('.')[0] + Environment.NewLine
                    + "UTC : " + data.utc_offset;

                try
                {
                    await botClient.SendTextMessageAsync(
                    chatId: ChatId,
                    text: result,
                    parseMode: ParseMode.Default,
                    disableNotification: true
                    );
                }
                catch (Exception e)
                {
                    //Console.WriteLine(e.Message);
                }
            }
        }



        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }
    }
}
