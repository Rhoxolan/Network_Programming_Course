using System.Text;
using System.Text.Json;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using System.Configuration;

namespace _2022._09._23_PW
{
    public partial class Form1 : Form
    {
        StringBuilder searchResponseStringBuilder;

        public Form1()
        {
            InitializeComponent();
            searchResponseStringBuilder = new();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                using HttpClient client = new();
                using HttpRequestMessage httpRequest = new(HttpMethod.Get,
                    $"http://www.omdbapi.com/?apikey=key&t={textBox1.Text}&y={textBox2.Text}");
                HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
                Movie mov = JsonSerializer.Deserialize<Movie>(await httpResponse.Content.ReadAsStringAsync())!;

                searchResponseStringBuilder.Clear();
                searchResponseStringBuilder.AppendLine($"{nameof(mov.Title)}: {mov.Title}");
                searchResponseStringBuilder.AppendLine($"{nameof(mov.Year)}: {mov.Year}");
                searchResponseStringBuilder.AppendLine($"{nameof(mov.Director)}: {mov.Director}");
                searchResponseStringBuilder.AppendLine($"{nameof(mov.Actors)}: {mov.Actors}");
                searchResponseStringBuilder.AppendLine($"{nameof(mov.Released)}: {mov.Released}");
                searchResponseStringBuilder.AppendLine($"{nameof(mov.Genre)}: {mov.Genre}");
                searchResponseStringBuilder.AppendLine($"{nameof(mov.Awards)}: {mov.Awards}");
                searchResponseStringBuilder.AppendLine($"{nameof(mov.DVD)}: {mov.DVD}");

                textBox2.Text = searchResponseStringBuilder.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (searchResponseStringBuilder.Length > 0)
            {
                try
                {
                    using (FileStream fs = new("SearchAnswer.txt", FileMode.Create))
                    {
                        using StreamWriter sw = new(fs);
                        sw.Write(searchResponseStringBuilder.ToString());
                    }

                    var message = new MimeMessage();
                    message.From.Add(new MailboxAddress("Name", "example@example.com"));
                    message.To.Add(new MailboxAddress("Name", textBox3.Text));
                    message.Subject = "Результаты поиска";
                    BodyBuilder builder = new();
                    await builder.Attachments.AddAsync("SearchAnswer.txt", File.OpenRead("SearchAnswer.txt"), new ContentType("text", "txt"));
                    message.Body = builder.ToMessageBody();
                    using var client = new SmtpClient();
                    await client.ConnectAsync("smtp.example.com", 465, true);
                    await client.AuthenticateAsync("example@example.com", "password");
                    await client.SendAsync(message);
                    await client.DisconnectAsync(true);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}