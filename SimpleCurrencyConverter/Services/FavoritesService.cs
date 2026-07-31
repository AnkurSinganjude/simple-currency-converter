using System.Text.Json;
using Windows.Storage;

namespace SimpleCurrencyConverter.Services;

public class FavoritesService
{
    private const string FavoritesKey = "favorite_pairs";

    public List<string> GetFavorites()
    {
        try
        {
            var json = ApplicationData.Current.LocalSettings.Values[FavoritesKey] as string;
            if (!string.IsNullOrEmpty(json))
            {
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
            ApplicationData.Current.LocalSettings.Values[FavoritesKey] = json;
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
