using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace _2022._09._19_PW__Part_II___Client_2_
{
    public partial class Form1 : Form
    {
        UdpClient udpClient;

        public Form1()
        {
            InitializeComponent();
            this.Load += Form1_Load!;
            udpClient = new();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                udpClient.JoinMulticastGroup(IPAddress.Parse("224.5.5.1"), 2);
                IPEndPoint localEP = new(GetLocalIpAddress(), 7000);
                udpClient.Client.Bind(localEP);
                Task.Run(() => Listener(localEP));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                try
                {
                    udpClient.Close();
                }
                finally
                {
                    Environment.Exit(-1);
                }
            }
        }

        private void Listener(IPEndPoint iPEndPoint)
        {
            try
            {
                while (true)
                {
                    var data = udpClient.Receive(ref iPEndPoint);
                    string message = Encoding.Unicode.GetString(data);
                    textBox1.BeginInvoke(() => textBox1.Text = message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                try
                {
                    udpClient.Close();
                }
                finally
                {
                    Environment.Exit(-1);
                }
            }
        }


        private IPAddress GetLocalIpAddress()
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }
            return IPAddress.Parse(localIP);
        }
    }
}