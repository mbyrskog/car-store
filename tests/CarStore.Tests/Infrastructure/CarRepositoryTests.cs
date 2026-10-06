using System.Text.Json;
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
        Assert.Equal(180, cars[0].Specs.Kilowatt);
    }

    [Fact]
    public async Task GetAllAsync_LoadsRealCarsJson()
    {
        var repo = new CarRepository();

        var cars = await repo.GetAllAsync(TestContext.Current.CancellationToken);

        Assert.NotEmpty(cars);
        Assert.Equal(cars.Count, cars.Select(c => c.Id).Distinct().Count());
        Assert.All(cars, c => Assert.True(c.Specs.Kilowatt > 0));
    }

    [Fact]
    public async Task GetAllAsync_MissingFile_Throws()
    {
        var repo = new CarRepository(Path.Combine(AppContext.BaseDirectory, "does-not-exist.json"));

        await Assert.ThrowsAsync<FileNotFoundException>(
            () => repo.GetAllAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public Task GetAllAsync_MisspelledField_Throws() =>
        AssertRejected("""[{ "id": "1", "brnd": "Volvo", "model": "XC60", "price": 1, "category": "SUV", "specs": { "year": 2020, "mileage": 1, "color": "Black", "transmission": "Automatic", "fuel": "Gasoline", "kilowatt": 1 } }]""");

    [Fact]
    public Task GetAllAsync_MissingSpecs_Throws() =>
        AssertRejected("""[{ "id": "1", "brand": "Volvo", "model": "XC60", "price": 1, "category": "SUV" }]""");

    [Fact]
    public Task GetAllAsync_NullEntry_Throws() =>
        AssertRejected("[null]");

    private static async Task AssertRejected(string json)
    {
        var filePath = Path.GetTempFileName();
        await File.WriteAllTextAsync(filePath, json, TestContext.Current.CancellationToken);

        try
        {
            var repo = new CarRepository(filePath);

            await Assert.ThrowsAsync<JsonException>(
                () => repo.GetAllAsync(TestContext.Current.CancellationToken));
        }
        finally
        {
            File.Delete(filePath);
        }
    }
}
