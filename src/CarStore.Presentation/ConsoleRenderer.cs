using CarStore.Domain;

namespace CarStore.Presentation;

public sealed class ConsoleRenderer
{
    private const int ColWidth = 16;
    private const decimal KmPerMile = 1.609344m;

    public void WriteColoredLine(string message, ConsoleColor color)
    {
        Console.ForegroundColor = color;
        Console.WriteLine(message);
        Console.ResetColor();
    }

    public void DisplayOptions()
    {
        var options = new[]
        {
            "1 - Show cars",
            "2 - Search",
            "3 - Change currency (also sets km/miles)",
            "4 - Reset search",
            "q / Esc - Quit"
        };

        foreach (var option in options)
        {
            WriteColoredLine(option, ConsoleColor.DarkYellow);
        }
    }

    public void PrintCars(
        IEnumerable<Car> cars,
        string currencyName,
        decimal currencyRate,
        bool useKilometers)
    {
        WriteColoredLine(GetTableHeader(currencyName, useKilometers), ConsoleColor.DarkYellow);

        var distanceMultiplier = useKilometers ? KmPerMile : 1m;

        foreach (var car in cars)
        {
            Console.Write(car.Brand.PadRight(ColWidth) + car.Model.PadRight(ColWidth));
            Console.Write(car.Specs.Year.ToString().PadRight(ColWidth));
            Console.Write(car.Category.PadRight(ColWidth) + car.Specs.Transmission.PadRight(ColWidth));
            Console.Write($"{car.Specs.Mileage * distanceMultiplier:0}".PadRight(ColWidth));
            Console.WriteLine($"{car.Price * currencyRate:N0}");
        }
    }

    private string GetTableHeader(string currencyName, bool useKilometers)
    {
        var distanceLabel = useKilometers ? "km" : "miles";

        return
            "Brand".PadRight(ColWidth) +
            "Model".PadRight(ColWidth) +
            "Year".PadRight(ColWidth) +
            "Category".PadRight(ColWidth) +
            "Transmission".PadRight(ColWidth) +
            $"Mileage ({distanceLabel})".PadRight(ColWidth) +
            $"Price ({currencyName})";
    }
}
