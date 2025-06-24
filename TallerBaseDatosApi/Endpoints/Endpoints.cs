namespace TallerBaseDatosApi.Endpoints;

public class Endpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/api/customers/ranking", async (ApplicationDbContext context) =>
        {
            var ranking =(await context.Customers
                .Select(c => new
                {
                    CustomerName = c.Name,
                    TotalOrders = c.Orders.Count,
                    TotalValue = c.Orders.Sum(o => o.Total)
                })
                .OrderByDescending(c => c.TotalValue)
                .ToListAsync())
                .Select((c, index) => new
                {
                    c.CustomerName,
                    c.TotalOrders,
                    c.TotalValue,
                    Ranking = index + 1
                })
                .ToList();
            return Results.Ok(ranking);
        });
        
        app.MapGet("/api/orders/by-city-branch", async (ApplicationDbContext context) =>
        {
            var result = await context.Branches
                .Select(b => new
                {
                    CityName = b.City.Name,
                    BranchName = b.Name,
                    TotalOrders = b.Orders.Count
                })
                .OrderBy(b => b.CityName)
                .ThenBy(b => b.BranchName)
                .ToListAsync();
            return Results.Ok(result);
        });
        
        app.MapGet("/api/sales/guayaquil", async (ApplicationDbContext context) =>
        {
            var result = await context.Branches
                .Where(b => b.City.Name == "Guayaquil")
                .Select(b => new
                {
                    BranchName = b.Name,
                    TotalSales = b.Orders.Sum(o => o.Total),
                    CityAverage = b.Orders.Any() ? b.Orders.Average(o => o.Total) : 0,
                    Percentage = b.Orders.Any()
                        ? (b.Orders.Sum(o => o.Total) - b.Orders.Average(o => o.Total)) /
                        b.Orders.Average(o => o.Total) * 100
                        : 0
                })
                .OrderBy(b => b.TotalSales)
                .ToListAsync();

            return Results.Ok(result);
        });
        
        app.MapGet("/api/sales/sierra", async (ApplicationDbContext context) =>
        {
            var result = (await context.Cities
                .Where(c => c.Name == "Sierra" || c.Name == "Guayaquil" || c.Name == "Quito")
                .SelectMany(c => c.Branches)
                .SelectMany(b => b.Orders)
                .GroupBy(o => o.Branch.City.Name)
                .Select(g => new
                {
                    CityName = g.Key,
                    TotalQuantities = g.Sum(o => o.Amount)
                })
                .OrderByDescending(g => g.TotalQuantities)
                .ToListAsync())
                .Select((g, index) => new
                {
                    g.CityName,
                    g.TotalQuantities,
                    Ranking = index + 1
                })
                .ToList();
            return Results.Ok(result);
        });
        
        app.MapGet("/api/customers/top3", async(ApplicationDbContext context) =>
        {
            var result = (await context.Customers
                .Where(c => c.Orders.Any(o => o.Branch.City.Name == "Guayaquil" || o.Branch.City.Name == "Quito"))
                .Select(c => new
                {
                    CustomerName = c.Name,
                    TotalSales = c.Orders
                        .Where(o => o.Branch.City.Name == "Guayaquil" || o.Branch.City.Name == "Quito")
                        .Sum(o => o.Total),
                    TotalOrders = c.Orders
                        .Count(o => o.Branch.City.Name == "Guayaquil" || o.Branch.City.Name == "Quito")
                })
                .OrderByDescending(c => c.TotalSales)
                .Take(3)
                .ToListAsync())
                .Select((c, index) => new
                {
                    c.CustomerName,
                    c.TotalSales,
                    c.TotalOrders,
                    Ranking = index + 1
                })
                .ToList();

            return Results.Ok(result);
        });
    }
}