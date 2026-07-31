using System.Text.Json.Serialization;

namespace SimpleCurrencyConverter.Models;

public class ExchangeRateData
{
    [JsonPropertyName("result")]
    public string Result { get; set; } = string.Empty;

    [JsonPropertyName("base_code")]
    public string BaseCode { get; set; } = string.Empty;

    [JsonPropertyName("time_last_update_utc")]
    public string TimeLastUpdateUtc { get; set; } = string.Empty;

    [JsonPropertyName("conversion_rates")]
    public Dictionary<string, double> ConversionRates { get; set; } = new();
}

public class CachedRateData
{
    public Dictionary<string, Dictionary<string, double>> AllRates { get; set; } = new();
    public DateTime LastUpdated { get; set; }
}
