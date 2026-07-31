using System.Text.Json;

namespace SimpleCurrencyConverter.Services;

public class FavoritesService
{
    private readonly string _favoritesFilePath;

    public FavoritesService()
    {
        var dataFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SimpleCurrencyConverter");
        Directory.CreateDirectory(dataFolder);
        _favoritesFilePath = Path.Combine(dataFolder, "favorites.json");
    }

    public List<string> GetFavorites()
    {
        try
        {
            if (File.Exists(_favoritesFilePath))
            {
                var json = File.ReadAllText(_favoritesFilePath);
                return JsonSerializer.Deserialize<List<string>>(json) ?? new();
            }
        }
        catch { }
        return new();
    }

    public void SaveFavorites(List<string> favorites)
    {
        try
        {
            var json = JsonSerializer.Serialize(favorites);
            File.WriteAllText(_favoritesFilePath, json);
        }
        catch { }
    }

    public bool IsFavorite(string from, string to)
    {
        var key = $"{from}/{to}";
        return GetFavorites().Contains(key);
    }

    public void ToggleFavorite(string from, string to)
    {
        var key = $"{from}/{to}";
        var favorites = GetFavorites();

        if (favorites.Contains(key))
            favorites.Remove(key);
        else
            favorites.Add(key);

        SaveFavorites(favorites);
    }

    public List<(string From, string To)> GetFavoritePairs()
    {
        return GetFavorites()
            .Select(f => f.Split('/'))
            .Where(parts => parts.Length == 2)
            .Select(parts => (parts[0], parts[1]))
            .ToList();
    }
}
