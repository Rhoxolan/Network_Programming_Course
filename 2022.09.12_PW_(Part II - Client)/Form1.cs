using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace _2022._09._12_PW__Part_II___Client_
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            Task.Run(Clock);
        }

        private void Clock()
        {
            try
            {
                Socket receiveSocket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                EndPoint remoteEP = new IPEndPoint(IPAddress.Any, 5000);
                receiveSocket.Bind(remoteEP);
                byte[] buff = new byte[1024];
                do
                {
                    int len = receiveSocket.ReceiveFrom(buff, ref remoteEP);
                    string text = Encoding.Default.GetString(buff, 0, len);
                    textBox1.BeginInvoke(UpdateText, text);
                }
                while (true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Environment.Exit(0);
            }
        }

        private void UpdateText(string text)
        {
            textBox1.Text = text;
        }
    }
}