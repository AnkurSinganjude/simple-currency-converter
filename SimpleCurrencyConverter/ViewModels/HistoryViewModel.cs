using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SimpleCurrencyConverter.Models;
using SimpleCurrencyConverter.Services;
using System.Collections.ObjectModel;

namespace SimpleCurrencyConverter.ViewModels;

public partial class HistoryViewModel : ObservableObject
{
    private readonly HistoryService _historyService;

    public HistoryViewModel(HistoryService historyService)
    {
        _historyService = historyService;
        History = new ObservableCollection<ConversionResult>();
    }

    public ObservableCollection<ConversionResult> History { get; }

    [ObservableProperty]
    private bool _isEmpty = true;

    [RelayCommand]
    private async Task LoadHistoryAsync()
    {
        var items = await _historyService.GetHistoryAsync();
        History.Clear();
        foreach (var item in items)
        {
            History.Add(item);
        }
        IsEmpty = History.Count == 0;
    }

    [RelayCommand]
    private async Task ClearHistoryAsync()
    {
        await _historyService.ClearHistoryAsync();
        History.Clear();
        IsEmpty = true;
    }
}
