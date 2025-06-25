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
                .AsNoTracking()
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
        }).WithDescription("1. Diseñar un query que permita informar el ranking de clientes por \"valor_total\", esto es, de acuerdo al importe total que ha comprado el cliente. La consulta debe mostrar los siguientes campos: descripcion del cliente, cantidad total de pedidos realizados, suma total de valores de los pedidos, ranking de acuerdo a la suma total de valores de los pedidos. Debe mostrar ordenado por el ranking.");
        
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
                .AsNoTracking()
                .ToListAsync();
            return Results.Ok(result);
        }).WithDescription("2. Diseñar un query que permita informar la cantidad total de pedidos por ciudad y sucursal. La consulta debe mostrar los siguientes campos: descripcion de la ciudad, descripcion de la sucursal, cantidad total de pedidos por ciudad y sucursal. Debe mostrar ordenado por ciudad, sucursal. Si una sucursal no ha generado pedidos, igual debe mostrarse dentro del informe.");
        
        app.MapGet("/api/sales/guayaquil", async (ApplicationDbContext context) =>
        {
            // Obtener las sucursales y sus ventas totales
            var branchesSales = await context.Branches
                .Where(b => b.City.Name == "Guayaquil")
                .Select(b => new
                {
                    BranchName = b.Name,
                    TotalSales = b.Orders.Sum(o => o.Total)
                })
                .AsNoTracking()
                .ToListAsync();

            // Calcular el promedio de ventas totales por sucursal en la ciudad
            var cityAverage = branchesSales.Any() ? branchesSales.Average(b => b.TotalSales) : 0;

            // Proyectar los resultados finales, incluyendo el porcentaje
            var result = branchesSales
                .Select(b => new
                {
                    b.BranchName,
                    b.TotalSales,
                    CityAverage = cityAverage,
                    Percentage = cityAverage != 0
                        ? ((b.TotalSales - cityAverage) / cityAverage) * 100 // Convertir a porcentaje
                        : 0
                })
                .OrderByDescending(b => b.TotalSales)
                .ToList();
            return Results.Ok(result);
        }).WithDescription("3. Diseñar un query que permita informar las ventas en $ de la ciudad \"Guayaquil\", desglosado por Sucursal. Debe mostrar Sucursal, ventas en $, promedio de ventas de la ciudad, relación en % entre ventas y promedio de ventas (ventas - ventas promedio)/ventas promedio. Ordenar por ventas en $.");
        
        app.MapGet("/api/sales/sierra", async (ApplicationDbContext context) =>
        {
            var result = (await context.Cities
                .Where(c => c.Name == "Quito" || c.Name == "Cuenca" || c.Name == "Ambato")
                .SelectMany(c => c.Branches)
                .SelectMany(b => b.Orders)
                .GroupBy(o => o.Branch.City.Name)
                .Select(g => new
                {
                    CityName = g.Key,
                    TotalQuantities = g.Sum(o => o.Amount)
                })
                .OrderByDescending(g => g.TotalQuantities)
                .AsNoTracking()
                .ToListAsync())
                .Select((g, index) => new
                {
                    g.CityName,
                    g.TotalQuantities,
                    Ranking = index + 1
                })
                .ToList();
            return Results.Ok(result);
        }).WithDescription("4. Diseñar un query que permita informar las cantidades vendidas de la región \"Sierra\" (si bien no hay un tabla de regiones, inferir por medio de las ciudades) desglosado por Ciudad. Debe mostrar Ciudad, Cantidades vendidas, Ranking en base a cantidades vendidas. Ordernar por Ranking de acuerdo a mayores cantidades vendidas.");
        
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
                .AsNoTracking()
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
        }).WithDescription("5. Diseñar un query de Clientes que hayan efectuado pedidos, por ventas en $ en ciudades de \"Guayaquil\" o \"Quito\". Debe mostrar cliente, ventas en $, cantidades pedidas, Ranking en base a ventas en $. Ordenar por Ranking de acuerdo a ventas en $. Debe mostrar los 3 mejores clientes.");
    }
}