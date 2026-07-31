using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SimpleCurrencyConverter.Models;
using SimpleCurrencyConverter.ViewModels;

namespace SimpleCurrencyConverter.Views;

public sealed partial class MainPage : Page
{
    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        ViewModel = App.GetService<MainViewModel>();
        this.InitializeComponent();
        SetupBindings();
    }

    private async void SetupBindings()
    {
        // Populate combo boxes
        FromCurrencyBox.ItemsSource = ViewModel.Currencies;
        ToCurrencyBox.ItemsSource = ViewModel.Currencies;

        // Set display member path via item template (using ToString override)
        FromCurrencyBox.SelectedItem = ViewModel.SelectedFromCurrency;
        ToCurrencyBox.SelectedItem = ViewModel.SelectedToCurrency;
        AmountBox.Value = ViewModel.Amount;

        // Subscribe to property changes for UI updates
        ViewModel.PropertyChanged += ViewModel_PropertyChanged;

        await ViewModel.InitializeAsync();
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            switch (e.PropertyName)
            {
                case nameof(MainViewModel.ConvertedAmount):
                    if (ViewModel.SelectedToCurrency != null)
                    {
                        ConvertedAmountText.Text = $"{ViewModel.ConvertedAmount:N2} {ViewModel.SelectedToCurrency.Code}";
                    }
                    break;
                case nameof(MainViewModel.RateSummary):
                    RateText.Text = ViewModel.RateSummary;
                    break;
                case nameof(MainViewModel.LastUpdatedText):
                    LastUpdatedText.Text = ViewModel.LastUpdatedText;
                    break;
                case nameof(MainViewModel.IsLoading):
                    LoadingRing.IsActive = ViewModel.IsLoading;
                    LoadingRing.Visibility = ViewModel.IsLoading ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case nameof(MainViewModel.HasResult):
                    ResultCard.Visibility = ViewModel.HasResult ? Visibility.Visible : Visibility.Collapsed;
                    break;
                case nameof(MainViewModel.IsOffline):
                    OfflineBanner.IsOpen = ViewModel.IsOffline;
                    break;
                case nameof(MainViewModel.HasError):
                    ErrorBanner.IsOpen = ViewModel.HasError;
                    ErrorBanner.Message = ViewModel.ErrorMessage;
                    break;
                case nameof(MainViewModel.IsFavorite):
                    FavoriteIcon.Glyph = ViewModel.IsFavorite ? "\uE735" : "\uE734";
                    FavoriteText.Text = ViewModel.IsFavorite ? "Remove from Favorites" : "Add to Favorites";
                    break;
            }

            UpdateFavoritesSection();
        });
    }

    private void AmountBox_ValueChanged(NumberBox sender, NumberBoxValueChangedEventArgs args)
    {
        if (!double.IsNaN(args.NewValue))
        {
            ViewModel.Amount = args.NewValue;
        }
    }

    private void FromCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (FromCurrencyBox.SelectedItem is Currency currency)
        {
            ViewModel.SelectedFromCurrency = currency;
        }
    }

    private void ToCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ToCurrencyBox.SelectedItem is Currency currency)
        {
            ViewModel.SelectedToCurrency = currency;
        }
    }

    private void SwapButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.SwapCurrenciesCommand.Execute(null);

        // Update combo box selections
        FromCurrencyBox.SelectedItem = ViewModel.SelectedFromCurrency;
        ToCurrencyBox.SelectedItem = ViewModel.SelectedToCurrency;
    }

    private void FavoriteButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.ToggleFavoriteCommand.Execute(null);
    }

    private void FavoritesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (FavoritesList.SelectedIndex >= 0)
        {
            ViewModel.SelectFavorite(FavoritesList.SelectedIndex);
            FromCurrencyBox.SelectedItem = ViewModel.SelectedFromCurrency;
            ToCurrencyBox.SelectedItem = ViewModel.SelectedToCurrency;
            FavoritesList.SelectedIndex = -1;
        }
    }

    private void UpdateFavoritesSection()
    {
        FavoritesSection.Visibility = ViewModel.Favorites.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
        FavoritesList.ItemsSource = ViewModel.Favorites;
    }
}
