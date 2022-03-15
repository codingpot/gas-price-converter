using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using gas_price_converter.Shared.Models;

namespace gas_price_converter.Shared;

public class CurrencyService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private CurrencyResponse? _currencyResponse;

    public CurrencyService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["CURRENCY_FREAKS_API_KEY"];
    }

    public async Task Initialize()
    {
        _currencyResponse = await GetCurrency();
    }

    public Currency Convert(Currency from, CurrencyType to)
    {
        switch (from.CurrencyType, to)
        {
            case (CurrencyType.Krw, CurrencyType.Usd):
                return new Currency(
                    to,
                    from.Value / _currencyResponse!.Rates!["KRW"]
                );
            
            case (CurrencyType.Usd, CurrencyType.Krw):
                return new Currency(
                    to,
                    from.Value * _currencyResponse!.Rates!["KRW"]
                );
                
            default:
                return from;
        }
    }

    private async Task<CurrencyResponse?> GetCurrency()
    {
        return await _httpClient.GetFromJsonAsync<CurrencyResponse>(
            $"https://api.currencyfreaks.com/latest?apikey={_apiKey}&symbols=USD,KRW");
    }

    public class CurrencyResponse
    {
        [JsonConverter(typeof(DateTimeOffsetJsonConverter))]
        public DateTimeOffset Date { get; set; }

        public class DateTimeOffsetJsonConverter : JsonConverter<DateTimeOffset>
        {
            public override DateTimeOffset Read(ref Utf8JsonReader reader, Type typeToConvert,
                JsonSerializerOptions options)
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.String:
                        return DateTimeOffset.ParseExact(reader.GetString()!, "yyyy-MM-dd HH:mm:sszz",
                            CultureInfo.CurrentCulture.DateTimeFormat);
                    default:
                        throw new JsonException();
                }
            }

            public override void Write(Utf8JsonWriter writer, DateTimeOffset value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString("yyyy-MM-dd HH:mm:sszz"));
            }
        }

        public string? Base { get; set; }
        public IDictionary<string, double>? Rates { get; set; }
    }
}