using System.Net.Sockets;
using System.Net;
using System.Text;

namespace _2022._09._12_HW__Part_II___Server_
{
    public class ServerApplication
    {
        public void Start() => Program();

        private void Program()
        {
            AddToLog($"{DateTime.Now}: Запуск сервера.");
            Task.Run(ExitWait);
            
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