using CarStore.Application;
using CarStore.Domain;

namespace CarStore.Presentation;

public class ConsoleApp
{
    private readonly CarService _carService;
    private readonly CurrencyService _currencyService;

    private readonly ConsoleRenderer _renderer = new();

    private IReadOnlyList<Car> _allCars = [];
    private IReadOnlyList<Car> _visibleCars = [];

    private string _currencyName = "USD";
    private decimal _currencyRate = 1m;
    private decimal _distanceMultiplier = 1m;

    public ConsoleApp(CarService carService, CurrencyService currencyService)
    {
        _carService = carService;
        _currencyService = currencyService;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _allCars = await _carService.GetAllCarsAsync(cancellationToken);
        _visibleCars = _allCars;

        _renderer.WriteColoredLine("Welcome to the CarStore", ConsoleColor.Cyan);

        var shouldRun = true;

        while (shouldRun && !cancellationToken.IsCancellationRequested)
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
                    _renderer.PrintCarsPaginated(
                        _visibleCars,
                        pageSize: 5,
                        _currencyName,
                        _currencyRate,
                        _distanceMultiplier);
                    break;

                case ConsoleKey.D3:
                    _renderer.PrintCarsGroupedByPrice(
                        _visibleCars,
                        _currencyName,
                        _currencyRate,
                        _distanceMultiplier);
                    break;

                case ConsoleKey.D4:
                    ChangeCurrency();
                    break;

                case ConsoleKey.D5:
                    ApplyFilter();
                    break;

                case ConsoleKey.D6:
                    ResetFilters();
                    break;

                case ConsoleKey.Q:
                    shouldRun = false;
                    break;

                case ConsoleKey.Escape:
                    return;

                default:
                    _renderer.WriteColoredLine("Invalid option!", ConsoleColor.Red);
                    break;
            }
        }
    }

    private void PrintCars()
    {
        _renderer.PrintCars(
            _visibleCars,
            _currencyName,
            _currencyRate,
            _distanceMultiplier);
    }

    private void ApplyFilter()
    {
        var filters = string.Join(", ", Enum.GetNames<CarFilterType>());
        Console.WriteLine($"Available filters: {filters}");

        Console.Write("Enter an option: ");
        var rawFilter = (Console.ReadLine() ?? string.Empty).Trim();

        if (!Enum.TryParse<CarFilterType>(rawFilter, true, out var filterType))
        {
            _renderer.WriteColoredLine("Invalid option", ConsoleColor.Red);
            return;
        }

        Console.Write("Enter value: ");
        var input = (Console.ReadLine() ?? string.Empty).Trim();
        var success = _carService.TryFilter(_visibleCars, filterType, input, out var filteredCars);

        if (!success)
        {
            _renderer.WriteColoredLine("Invalid input.", ConsoleColor.Red);
            return;
        }

        if (filteredCars.Count == 0)
        {
            _renderer.WriteColoredLine("No cars matched.", ConsoleColor.Yellow);
            return;
        }

        _visibleCars = filteredCars;

        _renderer.WriteColoredLine("Filter applied.", ConsoleColor.DarkYellow);
        PrintCars();
    }

    private void ResetFilters()
    {
        _visibleCars = _allCars;
        _renderer.WriteColoredLine("Filter reset.", ConsoleColor.DarkYellow);
    }

    private void ChangeCurrency()
    {
        Console.WriteLine("Available currencies: USD, GBP, SEK, DKK");
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
        _distanceMultiplier = _currencyService.GetDistanceMultiplier(_currencyName);

        _renderer.WriteColoredLine($"Currency set to {_currencyName}.", ConsoleColor.DarkYellow);
    }
}