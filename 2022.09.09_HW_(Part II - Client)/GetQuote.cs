using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _2022._09._09_HW__Part_II___Client_
{
    public static class GetQuote
    {
        public static string GetString(IPAddress iPAddress, IPEndPoint iPEndPoint)
        {
            Socket socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                socket.Connect(iPEndPoint);
                int length = 0;
                byte[] buff = new byte[1024];
                socket.Shutdown(SocketShutdown.Send);
                StringBuilder builder = new();
                do
                {
                    length = socket.Receive(buff);
                    string str = Encoding.Default.GetString(buff, 0, length);
                    builder.Append(str);
                }
                while (socket.Available > 0);
                return builder.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return String.Empty;
            }
            finally
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                }
                catch(Exception ex) 
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
