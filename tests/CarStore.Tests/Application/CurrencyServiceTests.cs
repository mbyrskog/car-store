using CarStore.Application;
using Xunit;

namespace CarStore.Tests.Application;

public class CurrencyServiceTests
{
    [Fact]
    public void GetRate_IgnoresCaseAndSpaces_AndRejectsUnknown()
    {
        var service = new CurrencyService();

        Assert.Equal(1m, service.GetRate("usd"));
        Assert.NotNull(service.GetRate(" sek "));
        Assert.Null(service.GetRate("eur"));
    }

    [Fact]
    public void UsesKilometers_TrueForSekAndDkk()
    {
        var service = new CurrencyService();

        Assert.True(service.UsesKilometers("SEK"));
        Assert.True(service.UsesKilometers("dkk"));
    }

    [Fact]
    public void UsesKilometers_FalseForUsdAndGbp()
    {
        var service = new CurrencyService();

        Assert.False(service.UsesKilometers("USD"));
        Assert.False(service.UsesKilometers("GBP"));
    }

    [Fact]
    public void Currencies_ListsAllSupported()
    {
        var service = new CurrencyService();

        Assert.Equal(new[] { "DKK", "GBP", "SEK", "USD" }, service.Currencies.Order());
    }
}
