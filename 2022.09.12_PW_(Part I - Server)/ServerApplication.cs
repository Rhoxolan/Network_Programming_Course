using System.Net.Sockets;
using System.Net;
using System.Text;

namespace _2022._09._12_PW__Part_I___Server_
{
    public class ServerApplication
    {

        Socket socket;
        IPAddress address;
        EndPoint remoteEP;

        public ServerApplication()
        {
            socket = null;
            AddToLog($"{DateTime.Now}: Запуск сервера.");
        }

        public void Start() => Program();

        private void Program()
        {
            try
            {
                Task.Run(() => ExitWait(socket));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                address = IPAddress.Parse("192.168.43.255");
                remoteEP = new IPEndPoint(address, 5000);
                while (true)
                {
                    byte[] buff = Encoding.Default.GetBytes(DateTime.Now.ToLongTimeString());
                    socket.SendTo(buff, remoteEP);
                    Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                AddToLog($"Критическая ошибка: Вызвано исключение: {ex.Message}. {Environment.NewLine}\tРабота сервера приостановлена.");
            }
            finally
            {
                try
                {
                    socket?.Shutdown(SocketShutdown.Both);
                    socket?.Close();
                }
                catch { }
            }
        }

        private void AddToLog(string str)
        {
            FileStream fs = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "serverLogs.log"), FileMode.Append);
            using StreamWriter sw = new(fs);
            sw.WriteLine(str);
        }

        private void ExitWait(Socket socket)
        {
            while (true)
            {
                if (Console.ReadKey().Key == ConsoleKey.End)
                {
                    AddToLog($"{DateTime.Now}: Выключение сервера.");
                    try
                    {
                        socket?.Shutdown(SocketShutdown.Both);
                        socket?.Close();
                    }
                    finally
                    {
                        Environment.Exit(0);
                    }
                }
            }
        }
    }
}