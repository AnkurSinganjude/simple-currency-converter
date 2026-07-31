using SimpleCurrencyConverter.Models;

namespace SimpleCurrencyConverter.Helpers;

public static class CurrencyData
{
    public static readonly List<Currency> SupportedCurrencies = new()
    {
        new("USD", "US Dollar", "🇺🇸"),
        new("INR", "Indian Rupee", "🇮🇳"),
        new("EUR", "Euro", "🇪🇺"),
        new("GBP", "British Pound", "🇬🇧"),
        new("JPY", "Japanese Yen", "🇯🇵"),
        new("AUD", "Australian Dollar", "🇦🇺"),
        new("CAD", "Canadian Dollar", "🇨🇦"),
        new("CHF", "Swiss Franc", "🇨🇭"),
        new("CNY", "Chinese Yuan", "🇨🇳"),
        new("HKD", "Hong Kong Dollar", "🇭🇰"),
        new("SGD", "Singapore Dollar", "🇸🇬"),
        new("NZD", "New Zealand Dollar", "🇳🇿"),
        new("KRW", "South Korean Won", "🇰🇷"),
        new("BRL", "Brazilian Real", "🇧🇷"),
        new("ZAR", "South African Rand", "🇿🇦"),
        new("AED", "UAE Dirham", "🇦🇪"),
        new("MXN", "Mexican Peso", "🇲🇽"),
        new("THB", "Thai Baht", "🇹🇭"),
        new("SEK", "Swedish Krona", "🇸🇪"),
        new("NOK", "Norwegian Krone", "🇳🇴"),
        new("DKK", "Danish Krone", "🇩🇰"),
        new("RUB", "Russian Ruble", "🇷🇺"),
        new("MYR", "Malaysian Ringgit", "🇲🇾"),
        new("IDR", "Indonesian Rupiah", "🇮🇩"),
        new("PHP", "Philippine Peso", "🇵🇭"),
        new("TRY", "Turkish Lira", "🇹🇷"),
        new("PLN", "Polish Zloty", "🇵🇱"),
        new("HUF", "Hungarian Forint", "🇭🇺"),
        new("CZK", "Czech Koruna", "🇨🇿"),
        new("ILS", "Israeli Shekel", "🇮🇱"),
        new("SAR", "Saudi Riyal", "🇸🇦"),
        new("ARS", "Argentine Peso", "🇦🇷"),
        new("CLP", "Chilean Peso", "🇨🇱"),
        new("EGP", "Egyptian Pound", "🇪🇬"),
        new("VND", "Vietnamese Dong", "🇻🇳"),
        new("PKR", "Pakistani Rupee", "🇵🇰"),
        new("BDT", "Bangladeshi Taka", "🇧🇩"),
        new("NGN", "Nigerian Naira", "🇳🇬"),
        new("COP", "Colombian Peso", "🇨🇴"),
        new("TWD", "Taiwan Dollar", "🇹🇼"),
        new("QAR", "Qatari Riyal", "🇶🇦"),
        new("KWD", "Kuwaiti Dinar", "🇰🇼"),
        new("OMR", "Omani Rial", "🇴🇲"),
        new("BHD", "Bahraini Dinar", "🇧🇭"),
        new("LKR", "Sri Lankan Rupee", "🇱🇰"),
        new("KES", "Kenyan Shilling", "🇰🇪"),
        new("GHS", "Ghanaian Cedi", "🇬🇭"),
        new("MMK", "Myanmar Kyat", "🇲🇲"),
        new("NPR", "Nepalese Rupee", "🇳🇵"),
        new("MAD", "Moroccan Dirham", "🇲🇦"),
        new("DZD", "Algerian Dinar", "🇩🇿"),
        new("TND", "Tunisian Dinar", "🇹🇳"),
        new("UYU", "Uruguayan Peso", "🇺🇾"),
        new("BOB", "Bolivian Boliviano", "🇧🇴"),
        new("GEL", "Georgian Lari", "🇬🇪"),
    };

    public static Currency? GetCurrency(string code) =>
        SupportedCurrencies.FirstOrDefault(c => c.Code == code);
}
