using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Net.WebSockets;

namespace _2022._09._07_PW_2__Part_II___Server_
{
    public class ServerApplication
    {
        public void Start() => Program();

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
                string val = builder.ToString();
                if (val == "D")
                {
                    string message = $"{DateTime.Now.ToLongDateString()}";
                    buff = Encoding.Default.GetBytes(message);
                    socket.Send(buff);
                }
                else if (val == "T")
                {
                    string message = $"{DateTime.Now.ToShortTimeString()}";
                    buff = Encoding.Default.GetBytes(message);
                    socket.Send(buff);
                }
                else if (val != "T" && val != "D")
                {
                    string message = $"Неизвестный запрос '{val}'";
                    buff = Encoding.Default.GetBytes(message);
                    socket.Send(buff);
                }
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