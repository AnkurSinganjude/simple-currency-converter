using System.Text.Json;

namespace SimpleCurrencyConverter.Services;

public class CacheService
{
    private const string CacheFileName = "exchange_rates_cache.json";
    private const string LastUpdatedFileName = "cache_last_updated.txt";

    private readonly string _dataFolder;
    private readonly string _cacheFilePath;
    private readonly string _lastUpdatedFilePath;

    public CacheService()
    {
        _dataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SimpleCurrencyConverter");
        Directory.CreateDirectory(_dataFolder);
        _cacheFilePath = Path.Combine(_dataFolder, CacheFileName);
        _lastUpdatedFilePath = Path.Combine(_dataFolder, LastUpdatedFileName);
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
            await File.WriteAllTextAsync(_lastUpdatedFilePath, lastUpdated.Ticks.ToString());
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
                var lastUpdated = GetLastUpdated();
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
        try
        {
            if (File.Exists(_lastUpdatedFilePath))
            {
                var ticksStr = File.ReadAllText(_lastUpdatedFilePath);
                if (long.TryParse(ticksStr, out var ticks) && ticks > 0)
                    return new DateTime(ticks, DateTimeKind.Utc).ToLocalTime();
            }
        }
        catch { }
        return DateTime.MinValue;
    }
}
