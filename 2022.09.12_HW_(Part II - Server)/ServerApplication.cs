using System.Net.Sockets;
using System.Net;
using System.Text;

namespace _2022._09._12_HW__Part_II___Server_
{
    public class ServerApplication //Добавить trai cetch с добавлениев в логи
    {
        public void Start() => Program();

        private List<(string Name, int Price)> products = new()
        {
            ("Процессор AND Ryzen 5", 3000),
            ("Звуковая карта Asus Soar Strix", 2500),
            ("Видеокарта GT710", 1500)
        };

        private void Program()
        {
            AddToLog($"{DateTime.Now}: Запуск сервера.");
            Task.Run(ExitWait);
            IPAddress serverAddress = IPAddress.Loopback;
            IPEndPoint serverEP = new(serverAddress, 3025);
            Socket listeningSocket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            listeningSocket.Bind(serverEP);
            Socket receiveSocket = listeningSocket;
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 3025);
            byte[] buff = new byte[1024];
            do
            {
                int len = receiveSocket.ReceiveFrom(buff, ref remoteEP);
                string message = Encoding.Default.GetString(buff, 0, len);
                AddToLog($"{DateTime.Now}: От {remoteEP} получено следующее сообщение: {message}");
                if(message == "GET_PRODUCTS")
                {
                    buff = products.GetBytes(message);
                    receiveSocket.SendTo(buff, remoteEP);
                }
            }
            while (true);
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
                    AddToLog($"{DateTime.Now}: Выключение сервера.");
                    Environment.Exit(0);
                }
            }
        }
    }
}