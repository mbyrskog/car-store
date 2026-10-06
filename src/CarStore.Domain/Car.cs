namespace CarStore.Domain;

public record CarSpecs(
    int Year,
    int Mileage,
    string Color,
    string Transmission,
    string Fuel,
    int Kilowatt
)
{
    public int Horsepower => (int)Math.Round(Kilowatt * 1.341m);
}

public record Car(
    string Id,
    string Brand,
    string Model,
    decimal Price,
    string Category,
    CarSpecs Specs
);
