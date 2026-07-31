using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SimpleCurrencyConverter.ViewModels;

namespace SimpleCurrencyConverter.Views;

public sealed partial class HistoryPage : Page
{
    public HistoryViewModel ViewModel { get; }

    public HistoryPage()
    {
        ViewModel = App.GetService<HistoryViewModel>();
        this.InitializeComponent();
    }

    protected override async void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        await ViewModel.LoadHistoryCommand.ExecuteAsync(null);
        UpdateUI();
    }

    private async void ClearButton_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "Clear History",
            Content = "Are you sure you want to clear all conversion history?",
            PrimaryButtonText = "Clear",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Close,
            XamlRoot = this.XamlRoot
        };

        var result = await dialog.ShowAsync();
        if (result == ContentDialogResult.Primary)
        {
            await ViewModel.ClearHistoryCommand.ExecuteAsync(null);
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        HistoryListView.ItemsSource = ViewModel.History;
        EmptyState.Visibility = ViewModel.IsEmpty ? Visibility.Visible : Visibility.Collapsed;
        HistoryListView.Visibility = ViewModel.IsEmpty ? Visibility.Collapsed : Visibility.Visible;
    }
}
