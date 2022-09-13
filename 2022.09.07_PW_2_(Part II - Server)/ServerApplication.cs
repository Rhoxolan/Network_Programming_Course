using System.Net.Sockets;
using System.Net;
using System.Text;

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
                try
                {
                    StringBuilder builder = new();
                    byte[] buff = new byte[1024];
                    int length = 0;
                    do
                    {
                        length = newSocket.Receive(buff);
                        string str = Encoding.Default.GetString(buff, 0, length);
                        builder.Append(str);
                    }
                    while (newSocket.Available > 0);
                    string val = builder.ToString();
                    if(val == "D")
                    {
                        string message = $"{DateTime.Now.ToLongDateString()}";
                        buff = Encoding.Default.GetBytes(message);
                        newSocket.Send(buff);
                    }
                    else if(val == "T")
                    {
                        string message = $"{DateTime.Now.ToShortTimeString()}";
                        buff = Encoding.Default.GetBytes(message);
                        newSocket.Send(buff);
                    }
                    else if (val != "T" && val != "D")
                    {
                        string message = $"Неизвестный запрос '{val}'";
                        buff = Encoding.Default.GetBytes(message);
                        newSocket.Send(buff);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    newSocket.Shutdown(SocketShutdown.Both);
                    newSocket.Close();
                }
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