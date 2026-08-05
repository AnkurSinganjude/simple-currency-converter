using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using SimpleCurrencyConverter.Views;

namespace SimpleCurrencyConverter;

public sealed partial class MainWindow : Window
{
    private bool _isDarkTheme = true;

    public MainWindow()
    {
        this.InitializeComponent();

        // Set Mica backdrop
        this.SystemBackdrop = new MicaBackdrop();

        // Set window size - maximize on launch
        var appWindow = this.AppWindow;
        appWindow.Title = "Simple Currency Converter";

        // Get display area and maximize
        var displayArea = Microsoft.UI.Windowing.DisplayArea.GetFromWindowId(
            Microsoft.UI.Win32Interop.GetWindowIdFromWindow(
                WinRT.Interop.WindowNative.GetWindowHandle(this)), 
            Microsoft.UI.Windowing.DisplayAreaFallback.Primary);
        var workArea = displayArea.WorkArea;
        appWindow.MoveAndResize(new Windows.Graphics.RectInt32(0, 0, workArea.Width, workArea.Height));

        // Set window icon
        var iconPath = Path.Combine(AppContext.BaseDirectory, "Assets", "app.ico");
        if (File.Exists(iconPath))
        {
            appWindow.SetIcon(iconPath);
        }

        // Load saved theme preference
        LoadThemePreference();
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

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer is NavigationViewItem item && item.Tag?.ToString() == "ThemeToggle")
        {
            ToggleTheme();
        }
    }

    private void ToggleTheme()
    {
        _isDarkTheme = !_isDarkTheme;
        ApplyTheme();
        SaveThemePreference();
    }

    private void ApplyTheme()
    {
        if (RootGrid.XamlRoot?.Content is FrameworkElement rootElement)
        {
            rootElement.RequestedTheme = _isDarkTheme ? ElementTheme.Dark : ElementTheme.Light;
        }
        else
        {
            RootGrid.RequestedTheme = _isDarkTheme ? ElementTheme.Dark : ElementTheme.Light;
        }

        // Update toggle button text and icon
        ThemeToggleItem.Content = _isDarkTheme ? "Light Mode" : "Dark Mode";
        ThemeIcon.Glyph = _isDarkTheme ? "\uE793" : "\uE708"; // Sun : Moon
    }

    private void LoadThemePreference()
    {
        try
        {
            var settingsPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SimpleCurrencyConverter", "theme.txt");
            if (File.Exists(settingsPath))
            {
                _isDarkTheme = File.ReadAllText(settingsPath).Trim() == "dark";
            }
        }
        catch { }

        ApplyTheme();
    }

    private void SaveThemePreference()
    {
        try
        {
            var folder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "SimpleCurrencyConverter");
            Directory.CreateDirectory(folder);
            File.WriteAllText(Path.Combine(folder, "theme.txt"), _isDarkTheme ? "dark" : "light");
        }
        catch { }
    }
}
