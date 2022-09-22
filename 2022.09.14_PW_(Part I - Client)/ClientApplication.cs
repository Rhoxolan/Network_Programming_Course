using System.Net.Sockets;
using System.Net;
using System.Text;
using ScreenSaver;
using System.Drawing.Imaging;

namespace _2022._09._14_PW__Part_I___Client_
{
    internal class ClientApplication
    {
        public void Start() => Program();

        private void Program()
        {
            Task.Run(ExitWait);
            while (true)
            {
                TcpClient tcpClient = new("127.0.0.1", 8000); //Ты тут. Подумать, можно ли сделать лучше и передать изображение. Сервак уже готов.
                if (Console.ReadKey().Key == ConsoleKey.P)
                {
                    byte[] data = Encoding.Default.GetBytes("Hello");
                    NetworkStream stream = tcpClient.GetStream();
                    stream.Write(data, 0, data.Length);
                    stream.Close();
                    tcpClient.Close();
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


//ScreenCapture screenCapture = new ScreenCapture();
//if (Console.ReadKey().Key == ConsoleKey.P)
//{
//    screenCapture.CaptureScreenToFile("img.png", ImageFormat.Png);
//}