using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Net.WebSockets;

namespace _2022._09._07_PW__Part_II___Server_
{
    public class ServerApplication
    {
        public static ServerApplication Run()
        {
            ServerApplication serverApplication = new();
            serverApplication.Start();
            return serverApplication;
        }

        public void Start() => Program();

        private void Program()
        {
            Task.Run(ExitWait);
            IPAddress myAddress = IPAddress.Loopback;
            IPEndPoint serverEP = new(myAddress, 3000);
            Socket listeningSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            listeningSocket.Bind(serverEP);
            listeningSocket.Listen();
            while(true)
            {
                Socket newSocket = listeningSocket.Accept();
                try
                {
                    StringBuilder builder = new();
                    byte[] buff = new byte[1024];
                    int length = 0;
                    builder.Append($"В {DateTime.Now} от {newSocket.RemoteEndPoint} получено следующее сообщение: ");
                    do
                    {
                        length = newSocket.Receive(buff);
                        string str = Encoding.Default.GetString(buff, 0, length);
                        builder.Append(str);
                    }
                    while (newSocket.Available > 0);
                    Console.WriteLine(builder.ToString());
                    string message = $"Hello from Server!";
                    buff = Encoding.Default.GetBytes(message);
                    newSocket.Send(buff);
                    Thread.Sleep(1000);
                }
                catch(Exception ex)
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
