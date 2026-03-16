using CarStore.Domain;
using Xunit;

namespace CarStore.Tests.Domain;

public class CarFiltersTests
{
    [Fact]
    public void ByBrand_IsCaseInsensitive()
    {
        var cars = new List<Car>
        {
            new("1", "Volvo", "XC60", 1, "SUV",
                new CarSpecs(2020, 1, "x", "Automatic", "Gasoline", new Power(1,1))),
            new("2", "BMW", "X5", 1, "SUV",
                new CarSpecs(2020, 1, "x", "Automatic", "Gasoline", new Power(1,1))),
        };

        var result = CarFilters.ByBrand(cars, "volvo").ToList();

        Assert.Single(result);
        Assert.Equal("Volvo", result[0].Brand);
    }
}
