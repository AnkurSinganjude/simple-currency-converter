using System.Text.Json;
using SimpleCurrencyConverter.Models;

namespace SimpleCurrencyConverter.Services;

public class HistoryService
{
    private const string HistoryFileName = "conversion_history.json";
    private const int MaxHistoryItems = 20;

    private readonly string _historyFilePath;

    public HistoryService()
    {
        var dataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SimpleCurrencyConverter");
        Directory.CreateDirectory(dataFolder);
        _historyFilePath = Path.Combine(dataFolder, HistoryFileName);
    }

    public async Task<List<ConversionResult>> GetHistoryAsync()
    {
        try
        {
            if (File.Exists(_historyFilePath))
            {
                var json = await File.ReadAllTextAsync(_historyFilePath);
                return JsonSerializer.Deserialize<List<ConversionResult>>(json) ?? new();
            }
        }
        catch { }
        return new();
    }

    public async Task AddToHistoryAsync(ConversionResult result)
    {
        try
        {
            var history = await GetHistoryAsync();
            history.Insert(0, result);

            if (history.Count > MaxHistoryItems)
                history = history.Take(MaxHistoryItems).ToList();

            var json = JsonSerializer.Serialize(history, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_historyFilePath, json);
        }
        catch { }
    }

    public async Task ClearHistoryAsync()
    {
        try
        {
            if (File.Exists(_historyFilePath))
                File.Delete(_historyFilePath);
        }
        catch { }
        await Task.CompletedTask;
    }
}
