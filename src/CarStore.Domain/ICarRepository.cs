using CarStore.Domain;

namespace CarStore.Domain;

public interface ICarRepository
{
    Task<IReadOnlyList<Car>> GetAllAsync(CancellationToken cancellationToken = default);
}