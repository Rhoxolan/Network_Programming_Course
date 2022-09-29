using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;

namespace _2022._09._26_PW
{
    internal class MonobankExchangeRatesValues
    {
        public int currencyCodeA { get; set; }
        public int currencyCodeB { get; set; }
        public int date { get; set; }
        public double rateBuy { get; set; }
        public double rateSell { get; set; }
        public double? rateCross { get; set; }
    }

    public class MonobankErrorDescription
    {
        public string errorDescription { get; set; }
    }


    public class MonobankExchangeRates
    {
        public string GetExchangeRates()
        {
            using HttpClient client = new();
            using HttpRequestMessage httpRequest = new(HttpMethod.Get, $"https://api.monobank.ua/bank/currency");
            HttpResponseMessage httpResponse = client.Send(httpRequest);

            string response = httpResponse.Content.ReadAsStringAsync().Result;
            if (response == "{\"errorDescription\":\"Too many requests\"}")
            {
                return "Превышено количество запросов по получению курса валют Монобанка в минуту. Пожалуйста, повторите запрос позже.";
            }

            List<MonobankExchangeRatesValues> ratesList = JsonSerializer.Deserialize<List<MonobankExchangeRatesValues>>(response)!;

            StringBuilder stringBuilder = new();
            stringBuilder.Append("Курс доллара: ");
            stringBuilder.Append(ratesList.Where(r => r.currencyCodeA == 840).Select(r => r.rateBuy).First() + " / ");
            stringBuilder.AppendLine(ratesList.Where(r => r.currencyCodeA == 840).Select(r => r.rateSell).First().ToString());
            stringBuilder.Append("Курс евро: ");
            stringBuilder.Append(ratesList.Where(r => r.currencyCodeA == 978).Select(r => r.rateBuy).First() + " / ");
            stringBuilder.Append(ratesList.Where(r => r.currencyCodeA == 978).Select(r => r.rateSell).First());

            return stringBuilder.ToString();
        }
    }
}
