using Microsoft.UI.Xaml;
using SimpleCurrencyConverter.Services;
using SimpleCurrencyConverter.ViewModels;

namespace SimpleCurrencyConverter;

public partial class App : Application
{
    private static readonly CacheService _cacheService = new();
    private static readonly ExchangeRateService _exchangeRateService = new(_cacheService);
    private static readonly FavoritesService _favoritesService = new();
    private static readonly HistoryService _historyService = new();

    private static readonly MainViewModel _mainViewModel = new(_exchangeRateService, _favoritesService, _historyService, _cacheService);
    private static readonly HistoryViewModel _historyViewModel = new(_historyService);

    private Window? _window;

    public App()
    {
        this.InitializeComponent();
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        _window = new MainWindow();
        _window.Activate();
    }

    public static T GetService<T>() where T : class
    {
        if (typeof(T) == typeof(MainViewModel)) return (_mainViewModel as T)!;
        if (typeof(T) == typeof(HistoryViewModel)) return (_historyViewModel as T)!;
        if (typeof(T) == typeof(ExchangeRateService)) return (_exchangeRateService as T)!;
        if (typeof(T) == typeof(CacheService)) return (_cacheService as T)!;
        if (typeof(T) == typeof(FavoritesService)) return (_favoritesService as T)!;
        if (typeof(T) == typeof(HistoryService)) return (_historyService as T)!;

        throw new InvalidOperationException($"Service {typeof(T).Name} not registered.");
    }
}
