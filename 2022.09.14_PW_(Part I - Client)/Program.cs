using ScreenSaver;
using System.Drawing.Imaging;
using System.Net.Sockets;

while (true)
{
    if (Console.ReadKey().Key == ConsoleKey.P)
    {
        using TcpClient tcpClient = new("127.0.0.1", 8000);
        new ScreenCapture().CaptureScreen().Save(tcpClient.GetStream(), ImageFormat.Png);
    }
}