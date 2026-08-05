namespace SimpleCurrencyConverter.Models;

public record Currency(string Code, string Name, string Flag)
{
    public string DisplayName => $"{Code} - {Name}";
    public override string ToString() => DisplayName;
}
