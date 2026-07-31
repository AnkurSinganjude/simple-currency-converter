namespace SimpleCurrencyConverter.Models;

public class ConversionResult
{
    public string FromCurrency { get; set; } = string.Empty;
    public string ToCurrency { get; set; } = string.Empty;
    public double Amount { get; set; }
    public double ConvertedAmount { get; set; }
    public double ExchangeRate { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.Now;

    public string Summary => $"{Amount:N2} {FromCurrency} = {ConvertedAmount:N2} {ToCurrency}";
    public string RateSummary => $"1 {FromCurrency} = {ExchangeRate:N6} {ToCurrency}";
    public string TimestampDisplay => Timestamp.ToString("g");
}
