using System.Net.Sockets;
using System.Net;
using System.Text;

namespace _2022._09._09_PW_2__Part_II___Server_
{
    public class ServerApplication
    {
        //private ManualResetEvent manualExit;

        public ServerApplication()
        {
            //manualExit = new ManualResetEvent(false);
        }

        public void Start() => Program();

        private async void Program()
        {
            await Task.Run(ExitWait);
            IPAddress myAddress = IPAddress.Loopback;
            IPEndPoint serverEP = new(myAddress, 3050);
            Socket listeningSocket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            listeningSocket.Bind(serverEP);
            listeningSocket.Listen();
            try
            {
                while(true)
                {
                    Socket newSocket = await listeningSocket.AcceptAsync(); //Ты тут. Разобраться, почему происходит выход. В main пример ThreadSleep, поэксперементировать. Возможно перенести туда mre и ExitWait.
                     //manualExit.WaitOne();
                    try
                    {
                        //throw new SemaphoreFullException();
                        StringBuilder builder = new();
                        byte[] buff = new byte[1024];
                        int length = 0;
                        do
                        {
                            length = await newSocket.ReceiveAsync(buff, SocketFlags.None);
                            string str = Encoding.Default.GetString(buff, 0, length);
                            builder.Append(str);
                        }
                        while (newSocket.Available > 0);
                        string val = builder.ToString();
                        if (val == "D")
                        {
                            string message = $"{DateTime.Now.ToLongDateString()}";
                            buff = Encoding.Default.GetBytes(message);
                            await newSocket.SendAsync(buff, SocketFlags.None);
                        }
                        else if (val == "T")
                        {
                            string message = $"{DateTime.Now.ToShortTimeString()}";
                            buff = Encoding.Default.GetBytes(message);
                            await newSocket.SendAsync(buff, SocketFlags.None);
                        }
                        else if (val != "T" && val != "D")
                        {
                            string message = $"Неизвестный запрос '{val}'";
                            buff = Encoding.Default.GetBytes(message);
                            await newSocket.SendAsync(buff, SocketFlags.None);
                        }
                    }
                    finally
                    {
                        newSocket.Shutdown(SocketShutdown.Both);
                        newSocket.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
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