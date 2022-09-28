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
            client = new("");
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
            AddToLog($"Запуск приложения");
            Task.Run(ExitWait);
            client.StartReceiving(HandleUpdateAsync, HandlePollingErrorAsync, receiverOptions, cts.Token);
            waitHandler.WaitOne();
            AddToLog($"Завершение работы");
        }

        //Переписать и проверить
        private async Task HandleUpdateAsync(ITelegramBotClient client, Update update, CancellationToken cancellationToken)
        {
            if (update.Message is not { } message)
                return;
            var messageText = "";
            if (update.Message?.Type != null)
                messageText = update.Message.Text;
            //if (update.Message.Text is not { } messageText) 
            //    return;
            var chatId = message.Chat.Id;
            if (message.Type == MessageType.Location)
            {
                var location = message.Location;
                await client.SendTextMessageAsync(
                        chatId,
                        $"Широта: {location?.Latitude}, Довгота: {location?.Longitude}",
                        replyToMessageId: update?.Message?.MessageId,
                        disableNotification: true,
                        cancellationToken: cancellationToken);
            }
            else
                switch (messageText)
                {
                    case "Записатися на прийом":
                        Message sentMessage = await client.SendTextMessageAsync(
            chatId: chatId,
            text: "Дякую, гарного дня!",
            replyMarkup: new ReplyKeyboardRemove(),
            cancellationToken: cancellationToken);
                        break;
                    case "/help":
                        ReplyKeyboardMarkup keyboardMarkup = new(new[]
                        {
                    new KeyboardButton[] {"Записатися на прийом", "Найближча лікарня" },
                    new KeyboardButton[] {KeyboardButton.WithRequestLocation("Екстренна допомога на місце перебування"), KeyboardButton.WithRequestContact("Зателефонувати ☎️") }

                })
                        { ResizeKeyboard = true };
                        await client.SendTextMessageAsync(
                            chatId,
                            "Як ми можемо Вам допомогти?",
                            replyMarkup: keyboardMarkup,
                            cancellationToken: cancellationToken);
                        break;
                    case "/method":
                        await client.SendTextMessageAsync(
                            chatId,
                            $"Спробуємо *використати всі варіанти* методу `HandleUpdateAsync`",
                            ParseMode.MarkdownV2,
                            replyToMessageId: update?.Message?.MessageId,
                            disableNotification: true,
                            replyMarkup: new InlineKeyboardMarkup(InlineKeyboardButton.WithUrl("переглянути документацію",
                            "https://telegrambots.github.io/book/2/send-msg/text-msg.html")),
                            cancellationToken: cancellationToken);
                        break;
                    case "/asp":
                        await client.SendPhotoAsync(
                            chatId,
                            photo: "https://www.tutorialsteacher.com/Content/images/core/install-dotnetcore3.PNG",
                            caption: "<b>ASP.NET Core</b><a href='https://learn.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0'>Details...</a>",
                            parseMode: ParseMode.Html,
                            cancellationToken: cancellationToken
                            );
                        break;
                    case "/itstep":
                        await client.SendVenueAsync(
                            chatId,
                            title: "Компютерна Академія 'ШАГ' Кривий Ріг",
                            address: "пр-т Миру, 44А",
                            latitude: 47.90792904998941f,
                            longitude: 33.38744426307482f,
                            cancellationToken: cancellationToken
                            );
                        break;
                    case "'/бухгалтерія":
                        await client.SendContactAsync(
                            chatId,
                            "+38097 999 22 55",
                            "Бухгалтерія Академії",
                            cancellationToken: cancellationToken
                            );
                        break;
                    default:
                        await client.SendTextMessageAsync(
                    chatId,
                    $"Ви сказали: {messageText}",
                    cancellationToken: cancellationToken
                    );
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
            AddToLog($"Произошла ошибка: {ErrorMessage}");
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
