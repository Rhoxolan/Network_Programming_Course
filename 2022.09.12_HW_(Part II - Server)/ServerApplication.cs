using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;

namespace _2022._09._12_HW__Part_II___Server_
{
    public class ServerApplication //Добавить trai cetch с добавлениев в логи
    {
        public void Start() => Program();

        private List<Products> products = new()
        {
            new() {Id = 1, Name = "Процессор AMD Ryzen 5", Price = 3000 },
            new() {Id = 2, Name = "Звуковая карта Asus Soar Strix", Price = 2500},
            new() {Id = 3, Name = "Видеокарта GT710", Price = 1500}
        };

        private void Program()
        {
            try
            {
                AddToLog($"{DateTime.Now}: Запуск сервера.");
                Task.Run(ExitWait);
                IPAddress serverAddress = IPAddress.Loopback;
                IPEndPoint serverEP = new(serverAddress, 3025);
                Socket listeningSocket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                listeningSocket.Bind(serverEP);

                Socket receiveSocket = listeningSocket;
                EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 3025);
                do
                {
                    byte[] buff = new byte[2048];
                    int len = receiveSocket.ReceiveFrom(buff, ref remoteEP);
                    string message = Encoding.Default.GetString(buff, 0, len);
                    if (message == "GET_PRODUCTS")
                    {
                        AddToLog($"{DateTime.Now}: От {remoteEP} получен запрос на предоставление списка товаров");
                        List<string> names = new() { products[0].Name, products[1].Name, products[2].Name };
                        buff = Encoding.Default.GetBytes(JsonSerializer.Serialize(names));
                        receiveSocket.SendTo(buff, remoteEP);
                        AddToLog($"{DateTime.Now}: К {remoteEP} отправлена информация о списке товаров");
                    }
                    if ((from p in products where p.Name == message select p.Name).Any())
                    {
                        AddToLog($"{DateTime.Now}: От {remoteEP} получен запрос на предоставление цены товара {message}");
                        int price = (from p in products
                                     where p.Name == message
                                     select p.Price).Single();
                        buff = Encoding.Default.GetBytes(JsonSerializer.Serialize(price));
                        receiveSocket.SendTo(buff, remoteEP);
                        AddToLog($"{DateTime.Now}: К {remoteEP} отправлена информация о цене товара {message}");
                    }
                }
                while (true);
            }
            catch(Exception  ex)
            {
                AddToLog($"{DateTime.Now}: Вызвано исключение: {ex.Message}. Работа сервера прекращена.");
            }
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