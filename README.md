# CarStore

Console-based car listing app built with .NET. Browse cars sorted by price, search, and switch currency.

## 🚀 Tech Stack

- .NET 10
- C#
- xUnit
- System.Text.Json

## 🎮 Features

- Load cars from JSON
- Cars sorted by price
- Table shows color, fuel and power (kW/hp) for each car
- Mileage in miles (USD, GBP) or km (SEK, DKK), following the chosen currency
- Search by brand, model, year, color, category, transmission or fuel; each search narrows the current results
- Currency switching (USD, SEK, GBP, DKK)
- Unit tests across layers

## 🛠 Run locally

Visual Studio:

- Open solution in Visual Studio
- Set `CarStore.Presentation` as startup project
- Run (F5)

Command line:

```
dotnet run --project src/CarStore.Presentation
```

## 🧪 Run tests

- Visual Studio: Test Explorer → Run All
- Command line: `dotnet test`
