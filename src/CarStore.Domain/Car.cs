namespace CarStore.Domain;

public record Power(int Kw, int Hp);

public record CarSpecs(
    int Year,
    int Mileage,
    string Color,
    string Transmission,
    string Fuel,
    Power Power
);

public record Car(
    string Id,
    string Brand,
    string Model,
    decimal Price,
    string Category,
    CarSpecs Specs
);
