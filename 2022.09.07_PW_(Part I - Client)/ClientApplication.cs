using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _2022._09._07_PW__Part_I___Client_
{
    public class ClientApplication
    {
        public static ClientApplication Run()
        {
            return new ClientApplication();
        }

        public ClientApplication() => Program();

        public void Start() => Program();

        private void Program()
        {
            IPAddress iPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint iPEndPoint = new(iPAddress, 3000);
            Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                socket.Connect(iPEndPoint);
                string message = "Hello, Server!";
                byte[] buff = Encoding.Default.GetBytes(message);
                socket.Send(buff);
                int length = 0;
                buff = new byte[1024];
                socket.Shutdown(SocketShutdown.Send);
                StringBuilder builder = new();
                do
                {
                    length = socket.Receive(buff);
                    string str = Encoding.Default.GetString(buff, 0, length);
                    builder.Append(str);
                }
                while (socket.Available > 0);
                Console.WriteLine(builder.ToString());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }
    }
}
