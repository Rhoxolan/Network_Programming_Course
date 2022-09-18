using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace _2022._09._12_HW__Part_I___Client_
{
    public partial class Form1 : Form
    {
        private IPAddress iPAddress;
        private IPEndPoint iPEndPoint;
        private Socket socket;

        public Form1()
        {
            InitializeComponent();
            iPAddress = IPAddress.Loopback;
            iPEndPoint = new(iPAddress, 3025);
            socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string message = "GET_PRODUCTS";
            byte[] buff = Encoding.Default.GetBytes(message);
            socket.SendTo(buff, iPEndPoint);

            Socket receiveSocket = socket;
            EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 3025);
            buff = new byte[1024];
            int len = receiveSocket.ReceiveFrom(buff, ref remoteEP);
            object productList = JsonSerializer.Deserialize<object>(Encoding.Default.GetString(buff, 0, len))!;
            //List<(string Name, int Price)> products = productList as List<(string Name, int Price)>;
        }
    }
}