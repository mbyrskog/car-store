using CarStore.Application;
using CarStore.Domain;

namespace CarStore.Presentation;

public class ConsoleApp
{
    private readonly CarService _service;
    private readonly ConsoleRenderer _renderer = new();

    private IReadOnlyList<Car> _allCars = [];
    private IReadOnlyList<Car> _visibleCars = [];

    private string _currencyName = "USD";
    private decimal _currencyRate = 1m;
    private decimal _distanceMultiplier = 1m;

    public ConsoleApp(CarService service)
    {
        _service = service;
    }

    public async Task RunAsync(CancellationToken cancellationToken)
    {
        _allCars = await _service.GetAllCarsAsync(cancellationToken);
        _visibleCars = _allCars;

        _renderer.WriteColoredLine("Welcome to the CarStore", ConsoleColor.Cyan);

        var shouldRun = true;

        while (shouldRun && !cancellationToken.IsCancellationRequested)
        {
            _renderer.DisplayOptions();

            System.Console.Write("Enter an option: ");
            var key = System.Console.ReadKey(intercept: true).Key;
            System.Console.WriteLine();

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
        System.Console.WriteLine($"Available filters: {filters}");

        System.Console.Write("Enter an option: ");
        var rawFilter = (System.Console.ReadLine() ?? string.Empty).Trim();

        if (!Enum.TryParse<CarFilterType>(rawFilter, true, out var filterType))
        {
            _renderer.WriteColoredLine("Invalid option", ConsoleColor.Red);
            return;
        }

        System.Console.Write("Enter value: ");
        var input = (System.Console.ReadLine() ?? string.Empty).Trim();
        var success = _service.TryFilter(_visibleCars, filterType, input, out var filteredCars);
        if (!success)
        {
            _renderer.WriteColoredLine("No cars matched or invalid input.", ConsoleColor.Red);
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
        System.Console.WriteLine("Available currencies: USD, GBP, SEK, DKK");
        System.Console.Write("Enter currency: ");
        var input = (System.Console.ReadLine() ?? string.Empty).Trim().ToUpperInvariant();

        switch (input)
        {
            case "USD":
                _currencyName = "USD";
                _currencyRate = 1m;
                _distanceMultiplier = 1m;
                break;

            case "GBP":
                _currencyName = "GBP";
                _currencyRate = 0.71m;
                _distanceMultiplier = 1m;
                break;

            case "SEK":
                _currencyName = "SEK";
                _currencyRate = 8.38m;
                _distanceMultiplier = 0.1609344m;
                break;

            case "DKK":
                _currencyName = "DKK";
                _currencyRate = 6.06m;
                _distanceMultiplier = 0.1609344m;
                break;

            default:
                _renderer.WriteColoredLine("Invalid currency. Keeping current selection.", ConsoleColor.Red);
                return;
        }

        _renderer.WriteColoredLine($"Currency set to {_currencyName}.", ConsoleColor.DarkYellow);
    }
}