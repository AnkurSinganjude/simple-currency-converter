using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SimpleCurrencyConverter.Views;

namespace SimpleCurrencyConverter;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        this.InitializeComponent();

        // Set Mica backdrop
        this.SystemBackdrop = new MicaBackdrop();

        // Set window size
        var appWindow = this.AppWindow;
        appWindow.Resize(new Windows.Graphics.SizeInt32(800, 750));
        appWindow.Title = "Simple Currency Converter";
    }

    private void NavView_Loaded(object sender, RoutedEventArgs e)
    {
        // Select the first item by default
        NavView.SelectedItem = NavView.MenuItems[0];
        ContentFrame.Navigate(typeof(MainPage));
    }

    private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        if (args.SelectedItemContainer is NavigationViewItem item)
        {
            var tag = item.Tag?.ToString();
            switch (tag)
            {
                case "MainPage":
                    ContentFrame.Navigate(typeof(MainPage));
                    break;
                case "HistoryPage":
                    ContentFrame.Navigate(typeof(HistoryPage));
                    break;
            }
        }
    }
}
