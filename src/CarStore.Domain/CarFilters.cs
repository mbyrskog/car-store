namespace CarStore.Domain;

public static class CarFilters
{
    public static IEnumerable<Car> ByBrand(IEnumerable<Car> cars, string input) =>
        cars.Where(c => c.Brand.Contains(input, StringComparison.OrdinalIgnoreCase));

    public static IEnumerable<Car> ByModel(IEnumerable<Car> cars, string input) =>
        cars.Where(c => c.Model.Contains(input, StringComparison.OrdinalIgnoreCase));

    public static IEnumerable<Car> ByCategory(IEnumerable<Car> cars, string input) =>
        cars.Where(c => c.Category.Contains(input, StringComparison.OrdinalIgnoreCase));

    public static IEnumerable<Car> ByTransmission(IEnumerable<Car> cars, string input) =>
        cars.Where(c => c.Specs.Transmission.Contains(input, StringComparison.OrdinalIgnoreCase));

    public static IEnumerable<Car> MinYear(IEnumerable<Car> cars, int year) =>
        cars.Where(c => c.Specs.Year >= year);

    public static IEnumerable<Car> MaxMileage(IEnumerable<Car> cars, int mileage) =>
        cars.Where(c => c.Specs.Mileage <= mileage);
}
