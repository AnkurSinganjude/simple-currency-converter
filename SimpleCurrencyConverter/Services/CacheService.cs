using System.Text.Json;
using Windows.Storage;

namespace SimpleCurrencyConverter.Services;

public class CacheService
{
    private const string CacheFileName = "exchange_rates_cache.json";
    private const string LastUpdatedKey = "cache_last_updated";

    private readonly string _cacheFilePath;

    public CacheService()
    {
        var localFolder = ApplicationData.Current.LocalFolder.Path;
        _cacheFilePath = Path.Combine(localFolder, CacheFileName);
    }

    public async Task SaveRatesAsync(string baseCurrency, Dictionary<string, double> rates, DateTime lastUpdated)
    {
        try
        {
            Dictionary<string, Dictionary<string, double>> allRates;

            if (File.Exists(_cacheFilePath))
            {
                var existing = await File.ReadAllTextAsync(_cacheFilePath);
                allRates = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, double>>>(existing) ?? new();
            }
            else
            {
                allRates = new();
            }

            allRates[baseCurrency] = rates;
            var json = JsonSerializer.Serialize(allRates, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_cacheFilePath, json);

            ApplicationData.Current.LocalSettings.Values[LastUpdatedKey] = lastUpdated.Ticks;
        }
        catch (Exception)
        {
            // Silently fail cache writes
        }
    }

    public async Task<(Dictionary<string, double> Rates, DateTime LastUpdated)?> LoadRatesAsync(string baseCurrency)
    {
        try
        {
            if (!File.Exists(_cacheFilePath))
                return null;

            var json = await File.ReadAllTextAsync(_cacheFilePath);
            var allRates = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, double>>>(json);

            if (allRates != null && allRates.TryGetValue(baseCurrency, out var rates))
            {
                var ticks = ApplicationData.Current.LocalSettings.Values[LastUpdatedKey] as long? ?? 0;
                var lastUpdated = ticks > 0 ? new DateTime(ticks, DateTimeKind.Utc) : DateTime.MinValue;
                return (rates, lastUpdated);
            }
        }
        catch (Exception)
        {
            // Silently fail cache reads
        }
        return null;
    }

    public DateTime GetLastUpdated()
    {
        var ticks = ApplicationData.Current.LocalSettings.Values[LastUpdatedKey] as long? ?? 0;
        return ticks > 0 ? new DateTime(ticks, DateTimeKind.Utc).ToLocalTime() : DateTime.MinValue;
    }
}
