using CarStore.Domain;

namespace CarStore.Presentation;

public sealed class ConsoleRenderer
{
    private const int BrandWidth = 16;
    private const int ModelWidth = 11;
    private const int YearWidth = 7;
    private const int ColorWidth = 9;
    private const int CategoryWidth = 12;
    private const int TransmissionWidth = 15;
    private const int FuelWidth = 11;
    private const int MileageWidth = 8;
    private const int PowerWidth = 16;
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
            Console.Write(car.Brand.PadRight(BrandWidth) + car.Model.PadRight(ModelWidth));
            Console.Write(car.Specs.Year.ToString().PadRight(YearWidth) + car.Specs.Color.PadRight(ColorWidth));
            Console.Write(car.Category.PadRight(CategoryWidth) + car.Specs.Transmission.PadRight(TransmissionWidth));
            Console.Write(car.Specs.Fuel.PadRight(FuelWidth));
            Console.Write($"{car.Specs.Mileage * distanceMultiplier:0}".PadRight(MileageWidth));
            Console.Write($"{car.Specs.Kilowatt} / {car.Specs.Horsepower}".PadRight(PowerWidth));
            Console.WriteLine($"{car.Price * currencyRate:N0}");
        }
    }

    private string GetTableHeader(string currencyName, bool useKilometers)
    {
        var distanceLabel = useKilometers ? "Km" : "Miles";

        return
            "Brand".PadRight(BrandWidth) +
            "Model".PadRight(ModelWidth) +
            "Year".PadRight(YearWidth) +
            "Color".PadRight(ColorWidth) +
            "Category".PadRight(CategoryWidth) +
            "Transmission".PadRight(TransmissionWidth) +
            "Fuel".PadRight(FuelWidth) +
            distanceLabel.PadRight(MileageWidth) +
            "Power (kW/hp)".PadRight(PowerWidth) +
            $"Price ({currencyName})";
    }
}
