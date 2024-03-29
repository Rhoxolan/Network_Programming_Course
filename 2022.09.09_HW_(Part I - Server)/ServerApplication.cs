﻿using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Net.WebSockets;

namespace _2022._09._09_HW__Part_I___Server_
{
    public class ServerApplication
    {
        public void Start() => Program();

        private readonly List<string> quotes = new()
            {
                "\"При помощи C вы легко можете выстрелить себе в ногу. При помощи C++ это сделать сложнее, но если это произойдёт, вам оторвёт всю ногу целиком.\" - Bjarne Stroustrup",
                "\"Проблема С++ в том, что необходимо узнать всё о нём перед тем, как начать писать на нём все что угодно.\" - Larry Wall",
                "\"Я думаю, что Microsoft назвал технологию .NET для того, чтобы она не показывалась в списках директорий Unix\" - Oktal",
                "\"Если вы считаете, что С++ труден, попытайтесь выучить английский.\" - Bjarne Stroustrup",
                "\"Если бы в Java действительно работала сборка мусора, большинство программ бы удаляли сами себя при первом же запуске.\" - Robert Sewell"
            };

        public ServerApplication()
        {
            AddToLog($"{DateTime.Now}: Запуск сервера");
        }

        private void Program()
        {
            IPAddress? myAddress = null;
            IPEndPoint? serverEP = null;
            Socket? listeningSocket = null;
            try
            {
                myAddress = IPAddress.Loopback;
                serverEP = new(myAddress, 3050);
                listeningSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                Task.Run(ExitWait);
                listeningSocket.Bind(serverEP);
                listeningSocket.Listen();
                while (true)
                {
                    Socket? newSocket = listeningSocket?.Accept();
                    AddToLog($"{DateTime.Now}: Подключился {newSocket?.RemoteEndPoint}");
                    Task.Run(() => Interaction(newSocket!));
                }
            }
            catch (Exception ex)
            {
                AddToLog($"{DateTime.Now}: Критическая ошибка: {ex.Message}. Работа сервера приостановлена.");
            }
        }

        private void Interaction(Socket socket)
        {
            int count = 0;
            while (true)
            {
                try
                {
                    StringBuilder builder = new();
                    byte[] buff = new byte[1024];
                    int length = 0;
                    do
                    {
                        length = socket.Receive(buff);
                        string str = Encoding.Default.GetString(buff, 0, length);
                        builder.Append(str);
                    }
                    while (socket.Available > 0);
                    if (builder.ToString() == "GET_QUOTE")
                    {
                        if(count >= 10)
                        {
                            buff = Encoding.Default.GetBytes("Достигнуто максимальное количество получений цитат. Подключение сброшено.");
                            socket.Send(buff);
                            socket.Shutdown(SocketShutdown.Both);
                            socket.Close();
                            return;
                        }
                        Random random = new();
                        int randIndex = random.Next(quotes.Count);
                        string message = quotes[randIndex];
                        buff = Encoding.Default.GetBytes(message);
                        socket.Send(buff);
                        count++;
                        AddToLog($"{DateTime.Now}: Клиенту {socket.RemoteEndPoint} Отправлена цитата №{randIndex}");
                    }
                    if (builder.ToString() == "Goodbye")
                    {
                        AddToLog($"{DateTime.Now}: Клиент {socket.RemoteEndPoint} отключился");
                        socket.Close();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    AddToLog($"{DateTime.Now}: Вызвано исключение: {ex.Message}. Взаимодействие с клиентом сброшено.");
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    return;
                }
            }
        }

        private void AddToLog(string str)
        {
            FileStream fs = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "serverLogs.log"), FileMode.Append);
            using StreamWriter sw = new(fs);
            sw.WriteLine(str);
        }

        private void ExitWait()
        {
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.End)
                {
                    AddToLog($"{DateTime.Now}: Выключение сервера");
                    Environment.Exit(0);
                }
            }
        }
    }
}