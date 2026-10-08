namespace CarStore.Domain;

public static class CarSearch
{
    public static IEnumerable<Car> Search(IEnumerable<Car> cars, string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return cars;
        }

        text = text.Trim();

        return cars.Where(c =>
            c.Brand.Contains(text, StringComparison.OrdinalIgnoreCase) ||
            c.Model.Contains(text, StringComparison.OrdinalIgnoreCase) ||
            c.Specs.Year.ToString() == text ||
            c.Category.Contains(text, StringComparison.OrdinalIgnoreCase) ||
            c.Specs.Transmission.Contains(text, StringComparison.OrdinalIgnoreCase) ||
            c.Specs.Color.Contains(text, StringComparison.OrdinalIgnoreCase) ||
            c.Specs.Fuel.Contains(text, StringComparison.OrdinalIgnoreCase));
    }
}
