using System.Text;
using System.Text.Json;

namespace _2022._09._26_PW
{
    internal class PrivatBankExchangeRatesValues
    {
        public string ccy { get; set; }
        public string base_ccy { get; set; }
        public string buy { get; set; }
        public string sale { get; set; }
    }
    public class PrivatBankExchangeRates
    {
        public string GetExchangeRates()
        {
            using HttpClient client = new();
            using HttpRequestMessage httpRequest = new(HttpMethod.Get, $"https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5");
            HttpResponseMessage httpResponse = client.Send(httpRequest);

            List<PrivatBankExchangeRatesValues> ratesList = JsonSerializer.Deserialize<List<PrivatBankExchangeRatesValues>>(httpResponse.Content.ReadAsStream())!;

            StringBuilder stringBuilder = new();
            stringBuilder.Append("Курс доллара: ");
            stringBuilder.Append(ratesList.Where(r => r.ccy == "USD").Select(r => r.buy).First() + " / ");
            stringBuilder.AppendLine(ratesList.Where(r => r.ccy == "USD").Select(r => r.sale).First());
            stringBuilder.Append("Курс евро: ");
            stringBuilder.Append(ratesList.Where(r => r.ccy == "EUR").Select(r => r.buy).First() + " / ");
            stringBuilder.AppendLine(ratesList.Where(r => r.ccy == "EUR").Select(r => r.sale).First());
            stringBuilder.Append("Курс биткойна: ");
            stringBuilder.Append(ratesList.Where(r => r.ccy == "BTC").Select(r => r.buy).First() + " / ");
            stringBuilder.Append(ratesList.Where(r => r.ccy == "BTC").Select(r => r.sale).First());

            return stringBuilder.ToString();
        }
    }
}
