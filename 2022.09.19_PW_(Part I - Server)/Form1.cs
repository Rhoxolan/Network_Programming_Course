using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

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
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(textBox1.Text);
                udpServer.Send(data, new IPEndPoint(IPAddress.Parse("224.5.5.0"), 7000)); //Группа админов
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                try
                {
                    udpServer.Close();
                }
                finally
                {
                    Environment.Exit(-1);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(textBox1.Text);
                udpServer.Send(data, new IPEndPoint(IPAddress.Parse("224.5.5.1"), 7000)); //Группа техподдержки
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                try
                {
                    udpServer.Close();
                }
                finally
                {
                    Environment.Exit(-1);
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] data = Encoding.Unicode.GetBytes(textBox1.Text);
                udpServer.Send(data, new IPEndPoint(IPAddress.Broadcast, 7000));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                try
                {
                    udpServer.Close();
                }
                finally
                {
                    Environment.Exit(-1);
                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                udpServer.Close();
            }
            catch
            {
                Environment.Exit(-1);
            }
        }
    }
}