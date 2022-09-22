using System.Net.Sockets;
using ScreenSaver;
using System.Drawing.Imaging;
using System.Drawing;

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
                TcpClient tcpClient = new("127.0.0.1", 8000);
                if (Console.ReadKey().Key == ConsoleKey.P)
                {
                    Image screenshot = new ScreenCapture().CaptureScreen();
                    screenshot.Save(tcpClient.GetStream(), ImageFormat.Png);
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

//Image screenshot = new ScreenCapture().CaptureScreen();  //Версия с устаревшей двоичной сериализацией
//using (Stream nstream = tcpClient.GetStream())
//{
//    new BinaryFormatter().Serialize(nstream, screenshot);
//}