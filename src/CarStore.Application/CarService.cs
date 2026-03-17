using CarStore.Domain;

namespace CarStore.Application;

public sealed class CarService
{
    private readonly ICarRepository _repo;

    public CarService(ICarRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<Car>> GetAllCarsAsync(CancellationToken ct)
    {
        var cars = await _repo.GetAllAsync(ct);
        return cars;
    }

    public bool TryFilter(
        IReadOnlyList<Car> cars,
        CarFilterType filterType,
        string input,
        out IReadOnlyList<Car> filteredCars)
    {
        filteredCars = [];

        int? number = null;

        if (filterType is CarFilterType.Year or CarFilterType.Mileage)
        {
            if (!int.TryParse(input, out var parsed))
            {
                return false;
            }

            number = parsed;
        }

        var filtered = filterType switch
        {
            CarFilterType.Brand => CarFilters.ByBrand(cars, input),
            CarFilterType.Model => CarFilters.ByModel(cars, input),
            CarFilterType.Category => CarFilters.ByCategory(cars, input),
            CarFilterType.Transmission => CarFilters.ByTransmission(cars, input),
            CarFilterType.Year => CarFilters.MinYear(cars, number!.Value),
            CarFilterType.Mileage => CarFilters.MaxMileage(cars, number!.Value),
            _ => cars
        };

        filteredCars = filtered.ToList();

        return true;
    }
}