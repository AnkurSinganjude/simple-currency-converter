# Simple Currency Converter

A modern WinUI 3 currency converter app built with the Windows App SDK and .NET 8.

![Windows](https://img.shields.io/badge/platform-Windows-blue)
![.NET 8](https://img.shields.io/badge/.NET-8.0-purple)
![WinUI 3](https://img.shields.io/badge/WinUI-3-green)

## Features

- **50+ currencies** with emoji flags and real-time exchange rates
- **Offline mode** — cached rates are used automatically when offline, with a banner indicating stale data
- **Favorites** — star currency pairs for quick access
- **Conversion history** — last 20 conversions saved locally
- **Modern Fluent UI** — Mica backdrop, light/dark theme, NavigationView, InfoBar, NumberBox
- **MVVM architecture** using CommunityToolkit.Mvvm

## Getting Started

### Prerequisites

- Windows 10 (version 1809+) or Windows 11
- [Visual Studio 2022](https://visualstudio.microsoft.com/) with the **.NET desktop development** and **Windows App SDK** workloads
- .NET 8 SDK

### API Key Setup

This app uses [ExchangeRate-API](https://www.exchangerate-api.com/) for live exchange rates.

1. Sign up for a free API key at [https://www.exchangerate-api.com/](https://www.exchangerate-api.com/)
2. Open `SimpleCurrencyConverter/Services/ExchangeRateService.cs`
3. Replace `YOUR_API_KEY_HERE` with your actual API key:

```csharp
private const string ApiKey = "your-actual-api-key";
```

### Build & Run

1. Clone this repository
2. Open `SimpleCurrencyConverter.sln` in Visual Studio 2022
3. Set the target platform (x64 recommended)
4. Press **F5** to build and run

Or from the command line:

```bash
dotnet restore SimpleCurrencyConverter.sln
dotnet build SimpleCurrencyConverter.sln -c Debug
```

## Project Structure

```
SimpleCurrencyConverter/
├── Models/
│   ├── Currency.cs              # Currency record with code, name, flag
│   ├── ConversionResult.cs      # Conversion result model
│   └── ExchangeRateData.cs      # API response model
├── Services/
│   ├── ExchangeRateService.cs   # API calls with offline fallback
│   ├── CacheService.cs          # JSON file-based rate caching
│   ├── FavoritesService.cs      # Favorite pairs via LocalSettings
│   └── HistoryService.cs        # Conversion history persistence
├── ViewModels/
│   ├── MainViewModel.cs         # Main converter logic
│   └── HistoryViewModel.cs      # History page logic
├── Views/
│   ├── MainPage.xaml/.cs        # Currency converter UI
│   └── HistoryPage.xaml/.cs     # Conversion history UI
├── Helpers/
│   └── CurrencyData.cs          # Static list of 55 currencies
├── App.xaml/.cs                 # Application entry point
├── MainWindow.xaml/.cs          # NavigationView shell
└── Package.appxmanifest         # App package manifest
```

## Architecture

- **MVVM** pattern with `CommunityToolkit.Mvvm` (ObservableObject, RelayCommand, ObservableProperty)
- **Services** handle data access, caching, favorites, and history
- **Offline resilience** — exchange rates are cached in `ApplicationData.LocalFolder` and used when the API is unreachable
- **Local persistence** — favorites stored in `LocalSettings`, history in a JSON file

## License

MIT
