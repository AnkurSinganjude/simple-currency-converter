using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleCurrencyConverter.Helpers;
using SimpleCurrencyConverter.Models;
using SimpleCurrencyConverter.Services;
using System.Collections.ObjectModel;

namespace SimpleCurrencyConverter.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly ExchangeRateService _exchangeRateService;
    private readonly FavoritesService _favoritesService;
    private readonly HistoryService _historyService;
    private readonly CacheService _cacheService;

    public MainViewModel(
        ExchangeRateService exchangeRateService,
        FavoritesService favoritesService,
        HistoryService historyService,
        CacheService cacheService)
    {
        _exchangeRateService = exchangeRateService;
        _favoritesService = favoritesService;
        _historyService = historyService;
        _cacheService = cacheService;

        Currencies = new ObservableCollection<Currency>(CurrencyData.SupportedCurrencies);
        Favorites = new ObservableCollection<string>();

        _selectedFromCurrency = CurrencyData.GetCurrency("USD");
        _selectedToCurrency = CurrencyData.GetCurrency("INR");
        _amount = 1;
    }

    public ObservableCollection<Currency> Currencies { get; }
    public ObservableCollection<string> Favorites { get; }

    [ObservableProperty]
    private Currency? _selectedFromCurrency;

    [ObservableProperty]
    private Currency? _selectedToCurrency;

    [ObservableProperty]
    private double _amount;

    [ObservableProperty]
    private double _convertedAmount;

    [ObservableProperty]
    private double _exchangeRate;

    [ObservableProperty]
    private bool _isLoading;

    [ObservableProperty]
    private bool _isOffline;

    [ObservableProperty]
    private bool _hasError;

    [ObservableProperty]
    private string _errorMessage = string.Empty;

    [ObservableProperty]
    private string _lastUpdatedText = string.Empty;

    [ObservableProperty]
    private string _rateSummary = string.Empty;

    [ObservableProperty]
    private bool _isFavorite;

    [ObservableProperty]
    private bool _hasResult;

    partial void OnSelectedFromCurrencyChanged(Currency? value) => _ = ConvertAsync();
    partial void OnSelectedToCurrencyChanged(Currency? value) => _ = ConvertAsync();
    partial void OnAmountChanged(double value) => _ = ConvertAsync();

    [RelayCommand]
    private async Task ConvertAsync()
    {
        if (SelectedFromCurrency == null || SelectedToCurrency == null || Amount <= 0)
        {
            HasResult = false;
            return;
        }

        IsLoading = true;
        HasError = false;
        IsOffline = false;

        try
        {
            var (rates, isFromCache, lastUpdated) = await _exchangeRateService.GetRatesAsync(SelectedFromCurrency.Code);

            if (rates != null && rates.TryGetValue(SelectedToCurrency.Code, out var rate))
            {
                ExchangeRate = rate;
                ConvertedAmount = Amount * rate;
                RateSummary = $"1 {SelectedFromCurrency.Code} = {rate:N6} {SelectedToCurrency.Code}";
                HasResult = true;
                IsOffline = isFromCache;

                if (lastUpdated != DateTime.MinValue)
                {
                    var localTime = lastUpdated.Kind == DateTimeKind.Utc ? lastUpdated.ToLocalTime() : lastUpdated;
                    LastUpdatedText = $"Rates updated: {localTime:g}";
                }

                // Save to history
                var result = new ConversionResult
                {
                    FromCurrency = SelectedFromCurrency.Code,
                    ToCurrency = SelectedToCurrency.Code,
                    Amount = Amount,
                    ConvertedAmount = ConvertedAmount,
                    ExchangeRate = rate,
                    Timestamp = DateTime.Now
                };
                await _historyService.AddToHistoryAsync(result);

                UpdateFavoriteState();
            }
            else
            {
                HasError = true;
                ErrorMessage = "Unable to fetch exchange rates. Please check your API key and internet connection.";
                HasResult = false;
            }
        }
        catch (Exception ex)
        {
            HasError = true;
            ErrorMessage = $"Conversion failed: {ex.Message}";
            HasResult = false;
        }
        finally
        {
            IsLoading = false;
        }
    }

    [RelayCommand]
    private void SwapCurrencies()
    {
        (SelectedFromCurrency, SelectedToCurrency) = (SelectedToCurrency, SelectedFromCurrency);
    }

    [RelayCommand]
    private void ToggleFavorite()
    {
        if (SelectedFromCurrency == null || SelectedToCurrency == null) return;

        _favoritesService.ToggleFavorite(SelectedFromCurrency.Code, SelectedToCurrency.Code);
        UpdateFavoriteState();
        LoadFavorites();
    }

    public void LoadFavorites()
    {
        Favorites.Clear();
        foreach (var pair in _favoritesService.GetFavoritePairs())
        {
            var fromCurrency = CurrencyData.GetCurrency(pair.From);
            var toCurrency = CurrencyData.GetCurrency(pair.To);
            if (fromCurrency != null && toCurrency != null)
            {
                Favorites.Add($"{pair.From} → {pair.To}");
            }
        }
    }

    public void SelectFavorite(int index)
    {
        var pairs = _favoritesService.GetFavoritePairs();
        if (index >= 0 && index < pairs.Count)
        {
            SelectedFromCurrency = CurrencyData.GetCurrency(pairs[index].From);
            SelectedToCurrency = CurrencyData.GetCurrency(pairs[index].To);
        }
    }

    private void UpdateFavoriteState()
    {
        if (SelectedFromCurrency != null && SelectedToCurrency != null)
        {
            IsFavorite = _favoritesService.IsFavorite(SelectedFromCurrency.Code, SelectedToCurrency.Code);
        }
    }

    public async Task InitializeAsync()
    {
        LoadFavorites();
        await ConvertAsync();
    }
}
