using System.Net.Sockets;
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
            IPAddress myAddress = IPAddress.Loopback;
            IPEndPoint serverEP = new(myAddress, 3050);
            Socket listeningSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                Task.Run(ExitWait);
                listeningSocket.Bind(serverEP);
                listeningSocket.Listen();
            }
            catch(Exception ex)
            {
                AddToLog($"{DateTime.Now}: Вызвано исключение: {ex.Message}");
            }
            while (true)
            {
                try
                {
                    Socket newSocket = listeningSocket.Accept();
                    AddToLog($"{DateTime.Now}: Подключился {newSocket.RemoteEndPoint}");
                    Task.Run(() => Answer(newSocket));
                }
                catch(Exception ex)
                {
                    AddToLog($"{DateTime.Now}: Вызвано исключение: {ex.Message}");
                }
            }
        }

        private void Answer(Socket socket)
        {
            try
            {
                Random random = new();
                int randIndex = random.Next(quotes.Count);
                string message = quotes[randIndex];
                byte[] buff = Encoding.Default.GetBytes(message);
                socket.Send(buff);
                AddToLog($"{DateTime.Now}: Отправлена цитата №{randIndex}");
            }
            catch(Exception ex)
            {
                AddToLog($"{DateTime.Now}: Вызвано исключение: {ex.Message}");
            }
            finally
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch(Exception ex)
                {
                    AddToLog($"{DateTime.Now}: Вызвано исключение: {ex.Message}");
                }
            }
        }

        private void AddToLog(string str)
        {
            FileStream fs = new("serverLogs.log", FileMode.Append);
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