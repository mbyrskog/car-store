using CarStore.Application;
using CarStore.Domain;

namespace CarStore.Presentation;

public sealed class ConsoleApp
{
    private readonly CarService _carService;
    private readonly CurrencyService _currencyService;

    private readonly ConsoleRenderer _renderer = new();

    private IReadOnlyList<Car> _allCars = [];
    private IReadOnlyList<Car> _visibleCars = [];

    private string _currencyName = "USD";
    private decimal _currencyRate = 1m;
    private bool _useKilometers;
    private readonly List<string> _searchTerms = [];

    public ConsoleApp(CarService carService, CurrencyService currencyService)
    {
        _carService = carService;
        _currencyService = currencyService;
    }

    public async Task RunAsync()
    {
        try
        {
            _allCars = await _carService.GetAllCarsAsync(CancellationToken.None);
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or System.Text.Json.JsonException)
        {
            _renderer.WriteColoredLine($"Could not load cars: {ex.Message}", ConsoleColor.Red);
            return;
        }

        _visibleCars = _allCars;

        _renderer.WriteColoredLine("Welcome to the CarStore", ConsoleColor.Cyan);

        var shouldRun = true;

        while (shouldRun)
        {
            _renderer.DisplayOptions();

            Console.Write("Enter an option: ");
            var key = Console.ReadKey(intercept: true).Key;
            Console.WriteLine();

            switch (key)
            {
                case ConsoleKey.D1:
                    PrintCars();
                    break;

                case ConsoleKey.D2:
                    Search();
                    break;

                case ConsoleKey.D3:
                    ChangeCurrency();
                    break;

                case ConsoleKey.D4:
                    ResetSearch();
                    break;

                case ConsoleKey.Q:
                case ConsoleKey.Escape:
                    shouldRun = false;
                    break;

                default:
                    _renderer.WriteColoredLine("Invalid option!", ConsoleColor.Red);
                    break;
            }
        }
    }

    private void PrintCars()
    {
        Console.Clear();

        if (_searchTerms.Count > 0)
        {
            _renderer.WriteColoredLine(
                $"Search: {string.Join(" > ", _searchTerms)} ({_visibleCars.Count} of {_allCars.Count} cars)",
                ConsoleColor.Green);
        }

        _renderer.PrintCars(
            _visibleCars,
            _currencyName,
            _currencyRate,
            _useKilometers);
    }

    private void Search()
    {
        Console.Write("Search within current results (brand, model, year, color, category, transmission or fuel, empty resets): ");
        var input = (Console.ReadLine() ?? string.Empty).Trim();

        if (input.Length == 0)
        {
            ResetSearch();
            return;
        }

        var matches = _carService.Search(_visibleCars, input);

        if (matches.Count == 0)
        {
            _renderer.WriteColoredLine("No cars matched.", ConsoleColor.Green);
            return;
        }

        _searchTerms.Add(input);
        _visibleCars = matches;
        PrintCars();
    }

    private void ResetSearch()
    {
        _searchTerms.Clear();
        _visibleCars = _allCars;
        _renderer.WriteColoredLine("Search reset.", ConsoleColor.Green);
    }

    private void ChangeCurrency()
    {
        Console.WriteLine($"Available currencies: {string.Join(", ", _currencyService.Currencies)}");
        Console.Write("Enter currency: ");
        var input = (Console.ReadLine() ?? string.Empty).Trim();

        var rate = _currencyService.GetRate(input);

        if (rate is null)
        {
            _renderer.WriteColoredLine("Invalid currency. Keeping current selection.", ConsoleColor.Red);
            return;
        }

        _currencyName = input.ToUpperInvariant();
        _currencyRate = rate.Value;
        _useKilometers = _currencyService.UsesKilometers(_currencyName);

        var distanceUnit = _useKilometers ? "km" : "miles";
        _renderer.WriteColoredLine($"Currency set to {_currencyName}. Mileage now in {distanceUnit}.", ConsoleColor.Green);
    }
}
