using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _2022._09._09_HW__Part_II___Client_
{
    public partial class Form1 : Form
    {
        private IPAddress iPAddress;
        private IPEndPoint iPEndPoint;

        public Form1()
        {
            InitializeComponent();
            iPAddress = IPAddress.Parse("127.0.0.1");
            iPEndPoint = new(iPAddress, 3050);

        }

        private async void button1_Click(object sender, EventArgs e)
            => textBox1.Text = await Task.Run(()=>GetQuote.GetString(iPAddress, iPEndPoint));
    }
}