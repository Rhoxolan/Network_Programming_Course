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

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string message = "GET_PRODUCTS";
                byte[] buff = Encoding.Default.GetBytes(message);
                socket.SendTo(buff, iPEndPoint);

                Socket receiveSocket = socket;
                EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 3025);
                buff = new byte[2048];
                var task = await receiveSocket.ReceiveFromAsync(buff, SocketFlags.None, remoteEP);
                int len = task.ReceivedBytes;
                List<string> productList = JsonSerializer.Deserialize<List<string>>(Encoding.Default.GetString(buff, 0, len))!;
                listBox1.DataSource = productList;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                try
                {
                    string message = (string)listBox1.SelectedItem;
                    byte[] buff = Encoding.Default.GetBytes(message);
                    socket.SendTo(buff, iPEndPoint);

                    Socket receiveSocket = socket;
                    EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 3025);
                    buff = new byte[2048];
                    var task = await receiveSocket.ReceiveFromAsync(buff, SocketFlags.None, remoteEP);
                    int len = task.ReceivedBytes;
                    int price = JsonSerializer.Deserialize<int>(Encoding.Default.GetString(buff, 0, len))!;
                    textBox1.Text = price.ToString();
                }
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}