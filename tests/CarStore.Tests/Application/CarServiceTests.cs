using CarStore.Application;
using CarStore.Domain;
using Xunit;

namespace CarStore.Tests.Application;

public class CarServiceTests
{
    [Fact]
    public async Task GetAllCarsAsync_ReturnsCarsFromRepository()
    {
        var service = new CarService(new FakeRepo());

        var cars = await service.GetAllCarsAsync(CancellationToken.None);

        Assert.Equal(3, cars.Count);
    }

    [Fact]
    public async Task GetAllCarsAsync_SortsByPrice()
    {
        var service = new CarService(new FakeRepo());

        var cars = await service.GetAllCarsAsync(CancellationToken.None);

        Assert.Equal(new[] { 30000m, 40000m, 60000m }, cars.Select(c => c.Price));
    }

    [Fact]
    public async Task Search_Brand_NarrowsCars()
    {
        var service = new CarService(new FakeRepo());

        var cars = await service.GetAllCarsAsync(CancellationToken.None);

        var result = service.Search(cars, "volvo");

        Assert.Equal(2, result.Count);
        Assert.All(result, c => Assert.Equal("Volvo", c.Brand));
    }

    [Fact]
    public async Task Search_EmptyText_ReturnsAllCars()
    {
        var service = new CarService(new FakeRepo());

        var cars = await service.GetAllCarsAsync(CancellationToken.None);

        var result = service.Search(cars, "  ");

        Assert.Equal(cars.Count, result.Count);
    }

    [Fact]
    public async Task Search_Category_MatchesAcrossBrands()
    {
        var service = new CarService(new FakeRepo());

        var cars = await service.GetAllCarsAsync(CancellationToken.None);

        var result = service.Search(cars, "suv");

        Assert.Equal(2, result.Count);
        Assert.Contains(result, c => c.Brand == "Volvo");
        Assert.Contains(result, c => c.Brand == "BMW");
    }

    [Fact]
    public async Task Search_WithinResults_NarrowsByYear()
    {
        var service = new CarService(new FakeRepo());

        var cars = await service.GetAllCarsAsync(CancellationToken.None);

        var volvos = service.Search(cars, "volvo");
        var result = service.Search(volvos, "2020");

        Assert.Single(result);
        Assert.Equal("XC60", result[0].Model);
    }

    private sealed class FakeRepo : ICarRepository
    {
        public Task<IReadOnlyList<Car>> GetAllAsync(CancellationToken token)
            => Task.FromResult<IReadOnlyList<Car>>(Cars);

        private static readonly IReadOnlyList<Car> Cars = new List<Car>
        {
            new(
                Id: "1",
                Brand: "Volvo",
                Model: "XC60",
                Price: 40000,
                Category: "SUV",
                Specs: new CarSpecs(
                    Year: 2020,
                    Mileage: 50000,
                    Color: "Black",
                    Transmission: "Automatic",
                    Fuel: "Gasoline",
                    Kilowatt: 180
                )
            ),
            new(
                Id: "2",
                Brand: "Volvo",
                Model: "V60",
                Price: 30000,
                Category: "Wagon",
                Specs: new CarSpecs(
                    Year: 2018,
                    Mileage: 80000,
                    Color: "White",
                    Transmission: "Automatic",
                    Fuel: "Diesel",
                    Kilowatt: 140
                )
            ),
            new(
                Id: "3",
                Brand: "BMW",
                Model: "X5",
                Price: 60000,
                Category: "SUV",
                Specs: new CarSpecs(
                    Year: 2020,
                    Mileage: 40000,
                    Color: "Blue",
                    Transmission: "Automatic",
                    Fuel: "Gasoline",
                    Kilowatt: 210
                )
            )
        };
    }
}
