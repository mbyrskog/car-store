using CarStore.Domain;

namespace CarStore.Presentation;

public sealed class ConsoleRenderer
{
    private const int ColWidth = 20;

    public void WriteColoredLine(string message, ConsoleColor color)
    {
        System.Console.ForegroundColor = color;
        System.Console.WriteLine(message);
        System.Console.ResetColor();
    }

    public void DisplayOptions()
    {
        var options = new[]
        {
            "1 - Print cars",
            "2 - Print cars paginated",
            "3 - Print cars grouped by price",
            "4 - Change currency and distance type",
            "5 - Apply filter and list matching cars",
            "6 - Reset filter",
            "q - Quit"
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
        decimal distanceMultiplier)
    {
        WriteColoredLine(GetTableHeader(currencyName), ConsoleColor.DarkYellow);

        foreach (var car in cars)
        {
            System.Console.Write(car.Brand.PadRight(ColWidth) + car.Model.PadRight(ColWidth));
            System.Console.Write(car.Specs.Year.ToString().PadRight(ColWidth));
            System.Console.Write(car.Category.PadRight(ColWidth) + car.Specs.Transmission.PadRight(ColWidth));
            System.Console.Write(Math.Round(car.Specs.Mileage * distanceMultiplier, 2).ToString().PadRight(ColWidth));
            System.Console.WriteLine($"{car.Price * currencyRate:0.00} {currencyName}");
        }
    }

    private string GetTableHeader(string currencyName)
    {
        var distanceLabel = currencyName is "USD" or "GBP" ? "Miles" : "Mil";

        return
            "Brand".PadRight(ColWidth) +
            "Model".PadRight(ColWidth) +
            "Year".PadRight(ColWidth) +
            "Category".PadRight(ColWidth) +
            "Transmission".PadRight(ColWidth) +
            $"Mileage ({distanceLabel})".PadRight(ColWidth) +
            $"Price ({currencyName})";
    }

    public void PrintCarsPaginated(
        IReadOnlyList<Car> cars,
        int pageSize,
        string currencyName,
        decimal currencyRate,
        decimal distanceMultiplier)
    {
        var totalPages = (int)Math.Ceiling(cars.Count / (double)pageSize);

        for (int pageIndex = 0; pageIndex < totalPages; pageIndex++)
        {
            var pageNumber = pageIndex + 1;
            var page = cars.Skip(pageIndex * pageSize).Take(pageSize).ToList();
            if (page.Count == 0) break;

            WriteColoredLine($"Page {pageNumber} of {totalPages}", ConsoleColor.DarkYellow);
            PrintCars(page, currencyName, currencyRate, distanceMultiplier);

            if (pageNumber < totalPages)
            {
                WriteColoredLine("Press any key to continue", ConsoleColor.DarkYellow);
                System.Console.ReadKey(true);
            }
        }

        WriteColoredLine("Done", ConsoleColor.DarkYellow);
    }

    public void PrintCarsGroupedByPrice(
       IEnumerable<Car> cars,
       string currencyName,
       decimal currencyRate,
       decimal distanceMultiplier)
    {
        var grouped = cars
            .OrderBy(p => p.Price)
            .GroupBy(p => Math.Floor(p.Price / 10000) * 10000)
            .ToList();

        foreach (var segment in grouped)
        {
            WriteColoredLine($"{segment.Key} - {segment.Key + 10000}", ConsoleColor.DarkYellow);
            PrintCars(segment, currencyName, currencyRate, distanceMultiplier);
        }
    }
}
