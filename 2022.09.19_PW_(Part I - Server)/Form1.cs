using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _2022._09._19_PW__Part_I___Server_
{
    public partial class Form1 : Form
    {
        UdpClient udpServer;

        public Form1()
        {
            InitializeComponent();
            udpServer = new();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.Unicode.GetBytes(textBox1.Text);
            udpServer.Send(data, new IPEndPoint(IPAddress.Parse("224.5.5.0"), 7000));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            byte[] data = Encoding.Unicode.GetBytes(textBox1.Text);
            udpServer.Send(data, new IPEndPoint(IPAddress.Parse("224.5.5.1"), 7000));
        }
    }
}