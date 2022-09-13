using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace _2022._09._09_PW__Part_II___Server_
{
    public class ServerApplication
    {
        public void Start() => Program();

        ManualResetEvent manualExit = new ManualResetEvent(false);

        private void Program()
        {
            Task.Run(ExitWait);
            Task.Run(ManualExitWait);
            IPAddress myAddress = IPAddress.Loopback;
            IPEndPoint serverEP = new IPEndPoint(myAddress, 3070);
            Socket listeningSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            listeningSocket.Bind(serverEP);
            listeningSocket.Listen((int)SocketOptionName.MaxConnections);
            try
            {
                listeningSocket.BeginAccept(AcceptCallBack, listeningSocket);
                manualExit.WaitOne();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;
            Socket newSocket = socket.EndAccept(ar);

            StringBuilder builder = new StringBuilder();
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
            newSocket.BeginSend(buff, 0, buff.Length, SocketFlags.None, SendCallBack, newSocket);
            socket.BeginAccept(AcceptCallBack, socket);
        }

        private void SendCallBack(IAsyncResult ar)
        {
            Socket newSocket = ar.AsyncState as Socket;
            newSocket.Shutdown(SocketShutdown.Both);
            newSocket.Close();
        }

        private void ExitWait()
        {
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.Delete || Console.ReadKey().Key == ConsoleKey.Backspace)
                {
                    Environment.Exit(0);
                }
            }
        }

        private void ManualExitWait()
        {
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.End)
                {
                    manualExit.Set();
                }
            }
        }
    }
}