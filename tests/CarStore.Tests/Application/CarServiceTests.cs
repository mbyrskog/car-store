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
    public async Task ApplyFilter_Brand_NarrowsCars()
    {
        var service = new CarService(new FakeRepo());

        var cars = await service.GetAllCarsAsync(CancellationToken.None);

        var ok = service.TryFilter(
            cars,
            CarFilterType.Brand,
            "volvo",
            out var filteredCars);

        Assert.True(ok);
        Assert.Equal(2, filteredCars.Count);
        Assert.All(filteredCars, c => Assert.Equal("Volvo", c.Brand));
    }

    [Fact]
    public async Task ApplyFilter_InvalidYear_ReturnsFalse_AndDoesNotChangeCars()
    {
        var service = new CarService(new FakeRepo());

        var cars = await service.GetAllCarsAsync(CancellationToken.None);

        var before = cars.Count;

        var ok = service.TryFilter(
            cars,
            CarFilterType.Year,
            "abc",
            out var filteredCars);

        Assert.False(ok);
        Assert.Empty(filteredCars);
    }

    [Fact]
    public async Task FilteringFromFilteredList_CanNarrowFurther()
    {
        var service = new CarService(new FakeRepo());

        var cars = await service.GetAllCarsAsync(CancellationToken.None);

        var ok1 = service.TryFilter(cars, CarFilterType.Brand, "volvo", out var brandFiltered);

        var ok2 = service.TryFilter(brandFiltered, CarFilterType.Year, "2020", out var yearFiltered);

        Assert.True(ok1);
        Assert.True(ok2);

        Assert.Single(yearFiltered);
        Assert.Equal("XC60", yearFiltered[0].Model);
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
                    Power: new Power(Kw: 180, Hp: 245)
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
                    Power: new Power(Kw: 140, Hp: 190)
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
                    Power: new Power(Kw: 210, Hp: 286)
                )
            )
        };
    }
}