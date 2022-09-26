using LiveCharts.WinForms;
using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.Definitions.Charts;
using LiveCharts.Defaults;
using System.Text.Json;
using System.Windows.Media;
using Timer = System.Windows.Forms.Timer;
using Microsoft.VisualBasic.Logging;

namespace _2022._09._21_PW
{
    public partial class Form1 : Form
    {
        Timer timer;

        public Form1()
        {
            InitializeComponent();
            timer = new Timer();
            timer.Interval = 10800000;
            timer.Tick += (s, e) => DataLoader();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = "47.908091";
            textBox2.Text = "33.387017";
            DataLoader();
            timer.Start();
        }


        private async void DataLoader()
        {
            try
            {
                using HttpClient client = new();
                using HttpRequestMessage httpRequest = new(HttpMethod.Get,
                    $"https://api.openweathermap.org/data/2.5/forecast?lat={textBox1.Text}&lon={textBox2.Text}&units=metric&appid=(hidden)");
                HttpResponseMessage httpResponse = await client.SendAsync(httpRequest);
                MyWeather myWeather = JsonSerializer.Deserialize<MyWeather>(await httpResponse.Content.ReadAsStringAsync())!;

                cartesianChart1.AxisX.Clear();
                cartesianChart1.AxisX.Add(new Axis()
                {
                    Labels = myWeather.list.Select(l => DateTime.Parse(l.dt_txt).ToLocalTime().ToString()).ToArray()
                });

                cartesianChart1.Series = new SeriesCollection()
            {
                new LineSeries
                {
                    Values = new ChartValues<double>(myWeather.list.Select(l => l.main.temp))
                },
            };
                textBox3.Text = myWeather.city.name;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e) => DataLoader();
    }
}