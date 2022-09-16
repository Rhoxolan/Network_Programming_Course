using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _2022._09._09_HW__Part_II___Client_
{
    public partial class Form1 : Form
    {
        private IPAddress iPAddress;
        private IPEndPoint iPEndPoint;
        private Socket? socket;

        public Form1()
        {
            InitializeComponent();
            iPAddress = IPAddress.Parse("127.0.0.1");
            iPEndPoint = new(iPAddress, 3050);
            socket = null;

        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (socket != null)
            {
                textBox1.Text = await Task.Run(() => GetQuote.GetString(iPAddress, iPEndPoint, socket));
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (socket == null)
            {
                try
                {
                    socket = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                    await socket.ConnectAsync(iPEndPoint);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    socket = null;
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (socket != null)
            {
                try
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    socket = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    socket = null;
                }
            }
        }
    }
}