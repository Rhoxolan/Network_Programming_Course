using System.Net.Sockets;
using System.Net;
using System.Text;

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

        private void Program()
        {
            Task.Run(ExitWait);
            IPAddress myAddress = IPAddress.Loopback;
            IPEndPoint serverEP = new(myAddress, 3050);
            Socket listeningSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            listeningSocket.Bind(serverEP);
            listeningSocket.Listen();
            while (true)
            {
                Socket newSocket = listeningSocket.Accept();
                Task.Run(() => Answer(newSocket));
            }
        }

        private void Answer(Socket socket)
        {
            try
            {
                Random random = new();
                string message = quotes[random.Next(quotes.Count)];
                byte[] buff = Encoding.Default.GetBytes(message);
                socket.Send(buff);
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        private void ExitWait()
        {
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.End)
                {
                    Environment.Exit(0);
                }
            }
        }
    }
}