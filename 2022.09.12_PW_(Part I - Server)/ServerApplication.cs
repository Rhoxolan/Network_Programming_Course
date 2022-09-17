using System.Net.Sockets;
using System.Net;
using System.Text;

namespace _2022._09._12_PW__Part_I___Server_
{
    public class ServerApplication
    {
        public void Start() => Program();

        private void Program()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.IP);
            IPAddress address = IPAddress.Parse("192.168.43.255");
            EndPoint remoteEP = new IPEndPoint(address, 5000);
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