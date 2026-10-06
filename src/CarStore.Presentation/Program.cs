using CarStore.Application;
using CarStore.Domain;
using CarStore.Infrastructure;
using CarStore.Presentation;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddSingleton<ICarRepository, CarRepository>();
services.AddSingleton<CarService>();
services.AddSingleton<CurrencyService>();
services.AddTransient<ConsoleApp>();

using var provider = services.BuildServiceProvider();

var app = provider.GetRequiredService<ConsoleApp>();

await app.RunAsync();

