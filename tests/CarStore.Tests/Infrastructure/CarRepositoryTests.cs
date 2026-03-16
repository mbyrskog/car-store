using CarStore.Infrastructure;
using Xunit;

namespace CarStore.Tests.Infrastructure;

public class CarRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_LoadsCarsFromJson()
    {
        var filePath = Path.Combine(AppContext.BaseDirectory, "Infrastructure", "cars.test.json");

        var repo = new CarRepository(filePath);

        var cars = await repo.GetAllAsync(TestContext.Current.CancellationToken);

        Assert.Single(cars);
        Assert.Equal("Volvo", cars[0].Brand);
        Assert.Equal(2020, cars[0].Specs.Year);
    }
}
