using CarStore.Application;
using CarStore.Domain;
using CarStore.Infrastructure;
using CarStore.Presentation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddSingleton<ICarRepository, CarRepository>();
        services.AddSingleton<CarService>();
        services.AddSingleton<CurrencyService>();
        services.AddTransient<ConsoleApp>();
    })
    .Build();

var app = host.Services.GetRequiredService<ConsoleApp>();

await app.RunAsync(CancellationToken.None);

