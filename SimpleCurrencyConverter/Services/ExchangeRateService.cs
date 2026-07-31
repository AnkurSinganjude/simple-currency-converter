using System.Net.Http;
using System.Text.Json;
using SimpleCurrencyConverter.Models;

namespace SimpleCurrencyConverter.Services;

public class ExchangeRateService
{
    private const string ApiKey = "42fcd8db760daa050cde4453";
    private const string BaseUrl = "https://v6.exchangerate-api.com/v6";
    private readonly HttpClient _httpClient;
    private readonly CacheService _cacheService;

    public ExchangeRateService(CacheService cacheService)
    {
        _httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(15) };
        _cacheService = cacheService;
    }

    public async Task<(Dictionary<string, double>? Rates, bool IsFromCache, DateTime LastUpdated)> GetRatesAsync(string baseCurrency)
    {
        try
        {
            var url = $"{BaseUrl}/{ApiKey}/latest/{baseCurrency}";
            var response = await _httpClient.GetStringAsync(url);
            var data = JsonSerializer.Deserialize<ExchangeRateData>(response);

            if (data?.Result == "success" && data.ConversionRates.Count > 0)
            {
                var now = DateTime.UtcNow;
                await _cacheService.SaveRatesAsync(baseCurrency, data.ConversionRates, now);
                return (data.ConversionRates, false, now);
            }
        }
        catch (Exception)
        {
            // Fall through to cache
        }

        // Try cache as fallback
        var cached = await _cacheService.LoadRatesAsync(baseCurrency);
        if (cached != null)
        {
            return (cached.Value.Rates, true, cached.Value.LastUpdated);
        }

        return (null, false, DateTime.MinValue);
    }

    public async Task<double?> ConvertAsync(string from, string to, double amount)
    {
        var (rates, _, _) = await GetRatesAsync(from);
        if (rates != null && rates.TryGetValue(to, out var rate))
        {
            return amount * rate;
        }
        return null;
    }
}
