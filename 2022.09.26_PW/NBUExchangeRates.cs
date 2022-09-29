using System.Linq;
using System.Text;
using System.Text.Json;

namespace _2022._09._26_PW
{
    internal class NBUExchangeRatesValues
    {
        public int r030 { get; set; }
        public string txt { get; set; }
        public double rate { get; set; }
        public string cc { get; set; }
        public string exchangedate { get; set; }
    }

    public class NBUExchangeRates
    {
        public string GetExchangeRates()
        {
            using HttpClient client = new();
            using HttpRequestMessage httpRequest = new(HttpMethod.Get, $"https://bank.gov.ua/NBUStatService/v1/statdirectory/exchange?json");
            HttpResponseMessage httpResponse = client.Send(httpRequest);

            List<NBUExchangeRatesValues> ratesList = JsonSerializer.Deserialize<List<NBUExchangeRatesValues>>(httpResponse.Content.ReadAsStream())!;

            StringBuilder stringBuilder = new();
            stringBuilder.Append("Курс доллара: ");
            stringBuilder.AppendLine(ratesList.Where(r => r.cc == "USD").Select(r => r.rate).First().ToString());
            stringBuilder.Append("Курс евро: ");
            stringBuilder.AppendLine(ratesList.Where(r => r.cc == "EUR").Select(r => r.rate).First().ToString());
            stringBuilder.Append("Курс злотого: ");
            stringBuilder.AppendLine(ratesList.Where(r => r.cc == "PLN").Select(r => r.rate).First().ToString());
            stringBuilder.Append("Курс алжирского динара: ");
            stringBuilder.AppendLine(ratesList.Where(r => r.cc == "PLN").Select(r => r.rate).First().ToString());
            stringBuilder.Append("Курс паладия: ");
            stringBuilder.AppendLine(ratesList.Where(r => r.cc == "XPD").Select(r => r.rate).First().ToString());

            return stringBuilder.ToString();
        }
    }
}
