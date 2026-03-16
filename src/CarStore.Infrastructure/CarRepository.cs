using CarStore.Domain;
using System.Text.Json;

namespace CarStore.Infrastructure;

public sealed class CarRepository : ICarRepository
{
    private readonly string _filePath;

    public CarRepository(string? filepath = null)
    {
        _filePath = filepath ?? Path.Combine(AppContext.BaseDirectory, "cars.json");
    }

    public async Task<IReadOnlyList<Car>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(_filePath))
            throw new FileNotFoundException($"Could not find cars.json at '{_filePath}'. Make sure it's copied to output.");

        var json = await File.ReadAllTextAsync(_filePath, cancellationToken);

        return JsonSerializer.Deserialize<List<Car>>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }) ?? [];
    }
}
