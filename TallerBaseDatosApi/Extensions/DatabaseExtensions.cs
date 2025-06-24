namespace TallerBaseDatosApi.Extensions;

public static class DatabaseExtensions
{
    public static async Task InitializeDatabaseAsync(this WebApplication app, CancellationToken cancellationToken)
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await context.Database.EnsureCreatedAsync(cancellationToken);
        await context.Database.MigrateAsync(cancellationToken);
        await SeedAsync(context, cancellationToken);
    }
    private static async Task SeedAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        await SeedCustomersAsync(context, cancellationToken);
        await SeedCitiesAsync(context, cancellationToken);
        await SeedBranchesAsync(context, cancellationToken);
        await SeedOrdersAsync(context, cancellationToken);
    }
    
    private static async Task SeedCustomersAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.Customers.AnyAsync(cancellationToken: cancellationToken))
        {
            List<Customer> customers = [
                new() { Name = "Ricardo Agama"},
                new() { Name = "Luis Castillo"},
                new() { Name = "Wilson Fernandez"},
                new() { Name = "Jose Ronquillo"},
            ];
            await context.Customers.AddRangeAsync(customers, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
    private static async Task SeedCitiesAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.Cities.AnyAsync(cancellationToken: cancellationToken))
        {
            List<City> cities = [
               new() { Name = "Guayaquil"},
               new() { Name = "Quito"},
               new() { Name = "Cuenca"},
               new() { Name = "Machala"},
               new() { Name = "Manta"},
               new() { Name = "Portoviejo"},
               new() { Name = "Ambato"},
               new() { Name = "Sto. Domingo"},
               new() { Name = "Salinas"},
            ];
            await context.Cities.AddRangeAsync(cities, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
    
    private static async Task SeedBranchesAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.Branches.AnyAsync(cancellationToken: cancellationToken))
        {
            List<Branch> branches = [
               new() { Name = "Urdesa" , Address = "Urdesa", CityId = 1},
               new() { Name = "Sauces" , Address = "Sauces", CityId = 1},
               new() { Name = "Miraflores" , Address = "Miraflores", CityId = 1},
               new() { Name = "Ponciano" , Address = "Ponciano", CityId = 2},
               new() { Name = "La Carolina" , Address = "La Carolina", CityId = 2},
               new() { Name = "Norte" , Address = "Norte", CityId = 3},
               new() { Name = "Sur" , Address = "Sur", CityId = 3},
               new() { Name = "9 de Mayo" , Address = "9 de Mayo", CityId = 4},
               new() { Name = "Jocay" , Address = "Jocay", CityId = 5},
               new() {  Name = "Chirijos" , Address = "Chirijos", CityId = 6},
               new() {  Name = "La Matriz" , Address = "La Matriz", CityId = 7},
               new() {  Name = "La Merced" , Address = "La Merced", CityId = 7},
               new() {  Name = "Rio verde" , Address = "Rio verde", CityId = 8},
               new() {  Name = "Santa rosa" , Address = "Santa rosa", CityId = 9},
               new() {  Name = "Vicente rocafuerte" , Address = "Vicente rocafuerte", CityId = 9},
            ];
            await context.Branches.AddRangeAsync(branches, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
    
    private static async Task SeedOrdersAsync(ApplicationDbContext context, CancellationToken cancellationToken)
    {
        if (!await context.Orders.AnyAsync(cancellationToken: cancellationToken))
        {
            List<Order> orders = [
            new() { BranchId = 1, CustomerId = 1, Date = new DateTime(2020, 1, 1), Total = 25, Amount = 2 },
            new() { BranchId = 1, CustomerId = 2, Date = new DateTime(2020, 1, 2), Total = 20, Amount = 2 },
            new() { BranchId = 1, CustomerId = 3, Date = new DateTime(2020, 1, 5), Total = 18, Amount = 3 },
            new() { BranchId = 2, CustomerId = 4, Date = new DateTime(2020, 2, 10), Total = 13, Amount = 2 },
            new() { BranchId = 2, CustomerId = 1, Date = new DateTime(2020, 2, 15), Total = 23, Amount = 3 },
            new() { BranchId = 2, CustomerId = 4, Date = new DateTime(2020, 2, 18), Total = 40, Amount = 2 },
            new() { BranchId = 3, CustomerId = 2, Date = new DateTime(2020, 2, 20), Total = 45, Amount = 3 },
            new() { BranchId = 4, CustomerId = 2, Date = new DateTime(2020, 1, 18), Total = 13, Amount = 2 },
            new() { BranchId = 5, CustomerId = 3, Date = new DateTime(2020, 1, 19), Total = 34, Amount = 4 },
            new() {  BranchId = 6, CustomerId = 3, Date = new DateTime(2020, 2, 13), Total = 29, Amount = 2 },
            new() {  BranchId = 7, CustomerId = 4, Date = new DateTime(2020, 2, 22), Total = 42, Amount = 3 },
            new() {  BranchId = 8, CustomerId = 1, Date = new DateTime(2020, 3, 1), Total = 45, Amount = 4 },
            new() {  BranchId = 9, CustomerId = 1, Date = new DateTime(2020, 3, 1), Total = 50, Amount = 3 },
            new() {  BranchId = 9, CustomerId = 2, Date = new DateTime(2020, 3, 10), Total = 20, Amount = 5 },
            new() {  BranchId = 11, CustomerId = 3, Date = new DateTime(2020, 3, 13), Total = 28, Amount = 6 },
            new() {  BranchId = 14, CustomerId = 4, Date = new DateTime(2020, 1, 30), Total = 22, Amount = 2 },
            new() {  BranchId = 14, CustomerId = 2, Date = new DateTime(2020, 1, 30), Total = 34, Amount = 4 },
            new() {  BranchId = 15, CustomerId = 2, Date = new DateTime(2020, 2, 29), Total = 18, Amount = 3 },
            new() {  BranchId = 15, CustomerId = 1, Date = new DateTime(2020, 2, 29), Total = 12, Amount = 6 },
            new() {  BranchId = 15, CustomerId = 3, Date = new DateTime(2020, 3, 4), Total = 14, Amount = 7 }
        ];
            await context.Orders.AddRangeAsync(orders, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
    
}