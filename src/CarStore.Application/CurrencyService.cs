namespace CarStore.Application;

public class CurrencyService
{
    public decimal? GetRate(string input)
    {
        return input.Trim().ToUpperInvariant() switch
        {
            "USD" => 1m,
            "GBP" => 0.71m,
            "SEK" => 8.38m,
            "DKK" => 6.06m,
            _ => null
        };
    }

    public decimal GetDistanceMultiplier(string currency)
    {
        return currency switch
        {
            "SEK" or "DKK" => 0.1609344m,
            _ => 1m
        };
    }
}