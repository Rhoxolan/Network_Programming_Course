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
            TcpClient client = null!;
            try
            {
                server.Start();
                AddToLog($"{DateTime.Now}: Запуск сервера {server.LocalEndpoint}");
                while (true)
                {
                    client = server.AcceptTcpClient();
                    AddToLog($"{DateTime.Now}: Подключился клиент {client.Client.RemoteEndPoint}");
                    Task.Run(()=>ReceiveImage(client));
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                server.Stop();
                client.Close();
            }
        }

        private void ReceiveImage(TcpClient client)
        {
            byte[] bytes = new byte[2048];
            NetworkStream stream = client.GetStream();
            int i = stream.Read(bytes, 0, bytes.Length);
            string data = Encoding.Default.GetString(bytes, 0, i); //Ты тут. Разобраться, как сериализовать и передавать Image.
            AddToLog($"{DateTime.Now}: Получено изображение от {client.Client.RemoteEndPoint}");
            MessageBox.Show(data);
            EndPoint closedEndPoint = client.Client.RemoteEndPoint!;
            client.Close();
            AddToLog($"{DateTime.Now}: Подключение с клиентом {closedEndPoint} было закрыто.");
        }

        private void PictureBoxRefresh(PictureBox pictureBox, Image image)
        {
            pictureBox.Image = image;
        }

        private void AddToLog(string str)
        {
            FileStream fs = new(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "serverLogs.log"), FileMode.Append);
            using StreamWriter sw = new(fs);
            sw.WriteLine(str);
        }

    }
}