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
        return cars.OrderBy(c => c.Price).ToList();
    }

    public IReadOnlyList<Car> Search(IReadOnlyList<Car> cars, string text) =>
        CarSearch.Search(cars, text).ToList();
}
