using System.Net;
using System.Net.Sockets;
using System.Text;

namespace _2022._09._14_PW__Part_II___Server_
{
    public partial class Form1 : Form
    {
        TcpListener server;

        public Form1()
        {
            InitializeComponent();
            server = new(IPAddress.Loopback, 8000);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Task.Run(ImageReceiver);
        }

        private void ImageReceiver()
        {
            try
            {
                server.Start();

                byte[] bytes = new byte[2048];
                string data = null!;

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    data = null!;
                    NetworkStream stream = client.GetStream();

                    int i;
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.Default.GetString(bytes, 0, i); //Ты тут. Разобраться, как сериализовать и передавать Image.
                    }

                    client.Close();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                server.Stop();
            }
        }

        private void PictureBoxRefresh(PictureBox pictureBox, Image image)
        {
            pictureBox.Image = image;
        }
    }
}