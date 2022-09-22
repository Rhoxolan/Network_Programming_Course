using System.Net.Sockets;
using System.Net;
using System.Text;
using ScreenSaver;
using System.Drawing.Imaging;
using System.Drawing;
using System.Runtime.Serialization.Formatters.Binary;
using System.Diagnostics;
using System.Text.Json;
using System.IO;

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
                    using (Stream nstream = tcpClient.GetStream())
                    {
                        new BinaryFormatter().Serialize(nstream, screenshot);
                    }
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