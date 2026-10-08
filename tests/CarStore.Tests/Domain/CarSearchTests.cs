using CarStore.Domain;
using Xunit;

namespace CarStore.Tests.Domain;

public class CarSearchTests
{
    [Fact]
    public void Search_IsCaseInsensitive()
    {
        var cars = new List<Car>
        {
            new("1", "Volvo", "XC60", 1, "SUV",
                new CarSpecs(2020, 1, "x", "Automatic", "Gasoline", 1)),
            new("2", "BMW", "X5", 1, "SUV",
                new CarSpecs(2020, 1, "x", "Automatic", "Gasoline", 1)),
        };

        var result = CarSearch.Search(cars, "volvo").ToList();

        Assert.Single(result);
        Assert.Equal("Volvo", result[0].Brand);
    }

    [Fact]
    public void Search_Year_MatchesWholeYearOnly()
    {
        var cars = new List<Car>
        {
            new("1", "Volvo", "XC60", 1, "SUV",
                new CarSpecs(2020, 1, "x", "Automatic", "Gasoline", 1)),
            new("2", "BMW", "X5", 1, "SUV",
                new CarSpecs(2022, 1, "x", "Automatic", "Gasoline", 1)),
        };

        var fullYear = CarSearch.Search(cars, "2022").ToList();
        var partOfYear = CarSearch.Search(cars, "20").ToList();

        Assert.Single(fullYear);
        Assert.Equal("BMW", fullYear[0].Brand);
        Assert.Empty(partOfYear);
    }
}
