namespace CarStore.Application;

public sealed class CurrencyService
{
    private static readonly Dictionary<string, decimal> Rates = new()
    {
        ["USD"] = 1m,
        ["GBP"] = 0.7562m,
        ["SEK"] = 10.0433m,
        ["DKK"] = 6.6713m
    };

    public IReadOnlyCollection<string> Currencies => Rates.Keys;

    public decimal? GetRate(string input)
    {
        return Rates.TryGetValue(input.Trim().ToUpperInvariant(), out var rate) ? rate : null;
    }

    public bool UsesKilometers(string currency)
    {
        return currency.Trim().ToUpperInvariant() is "SEK" or "DKK";
    }
}
