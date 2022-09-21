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
            ScreenCapture screenCapture = new ScreenCapture();
            if (Console.ReadKey().Key == ConsoleKey.P)
            {
                screenCapture.CaptureScreenToFile("img.png", ImageFormat.Png);
            }
        }
    }
}