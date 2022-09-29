using _2022._09._26_PW;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace RhxExchangeRatesBot
{
    public class MyTelegramBot
    {
        private TelegramBotClient client;
        private CancellationTokenSource cts;
        ReceiverOptions receiverOptions;
        ManualResetEvent waitHandler;

        public MyTelegramBot()
        {
            client = new("token");
            cts = new CancellationTokenSource();
            receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = Array.Empty<UpdateType>()
            };
            waitHandler = new(false);
        }

        public void Start() => Program();

        private void Program()
        {
            AddToLog($"{DateTime.Now}: Запуск приложения");
            Task.Run(ExitWait);
            client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync, receiverOptions, cts.Token);
            waitHandler.WaitOne();
            AddToLog($"{DateTime.Now}: Завершение работы");
        }

        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
            {
                return;
            }

            string messageText = String.Empty;
            if (update.Message?.Type != null)
            {
                messageText = update.Message.Text!;
            }

            long chatId = message.Chat.Id;

            Task.Run(() => ResponseAsync(messageText, chatId, client, cancellationToken, message));
        }

        private async void ResponseAsync(string messageText, long chatId, ITelegramBotClient client, CancellationToken cancellationToken, Message? message)
        {
            switch (messageText)
            {
                case "/start":
                    ReplyKeyboardMarkup keyboardMarkup = new(new KeyboardButton[] { "Курс валют Монобанк", "Курс валют ПриватБанк", "Курс валют НБУ" })
                    {
                        ResizeKeyboard = true
                    };
                    await client.SendTextMessageAsync(chatId, "Пожалуйста, сделайте ваш выбор:", replyMarkup: keyboardMarkup, cancellationToken: cancellationToken);
                    AddToLog($"{DateTime.Now}: Клиент {message.Chat.Id} начал работу с ботом");
                    break;

                case "Курс валют Монобанк":
                    await client.SendTextMessageAsync(chatId, new MonobankExchangeRates().GetExchangeRates(), cancellationToken: cancellationToken);
                    AddToLog($"{DateTime.Now}: Клиенту {message.Chat.Id} отправлена информация о курсе валют Монобанка");
                    break;

                case "Курс валют ПриватБанк":
                    await client.SendTextMessageAsync(chatId, new PrivatBankExchangeRates().GetExchangeRates(), cancellationToken: cancellationToken);
                    AddToLog($"{DateTime.Now}: Клиенту {message.Chat.Id} отправлена информация о курсе валют ПриватБанка");
                    break;

                case "Курс валют НБУ":
                    await client.SendTextMessageAsync(chatId, new NBUExchangeRates().GetExchangeRates(), cancellationToken: cancellationToken);
                    AddToLog($"{DateTime.Now}: Клиенту {message.Chat.Id} отправлена информация о курсе валют от НБУ");
                    break;
            }
        }

        private Task HandlePollingErrorAsync(ITelegramBotClient client, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"{apiRequestException.ErrorCode}\n{apiRequestException.Message}",
                _ => exception.ToString()
            };
            AddToLog($"{DateTime.Now}: Произошла ошибка: {ErrorMessage}");
            return Task.CompletedTask;
        }

        private void AddToLog(string str)
        {
            FileStream fs = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "logs.log"), FileMode.Append);
            using StreamWriter sw = new(fs);
            sw.WriteLine(str);
        }

        private void ExitWait()
        {
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.End)
                {
                    cts.Cancel();
                    waitHandler.Set();
                }
            }
        }
    }
}
