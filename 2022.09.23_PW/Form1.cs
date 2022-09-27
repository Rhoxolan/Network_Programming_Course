using System.Text.Json;

namespace _2022._09._23_PW
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            using HttpClient client = new();
            using HttpRequestMessage httpRequest = new(HttpMethod.Get,
                $"");
            HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
            MyWeather myWeather = JsonSerializer.Deserialize<MyWeather>(await httpResponse.Content.ReadAsStringAsync())!;
        }
    }
}