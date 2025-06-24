namespace TallerBaseDatosApi.Data;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        // Aplicar el filtro de consulta global a todas las entidades que implementan IEntity
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (!typeof(IEntity).IsAssignableFrom(entityType.ClrType)) continue;
            var method = typeof(ApplicationDbContext)
                .GetMethod(nameof(SetGlobalQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(entityType.ClrType);
            method?.Invoke(null, [modelBuilder]);
        }
    }
    
    private static void SetGlobalQueryFilter<TEntity>(ModelBuilder modelBuilder) where TEntity : class, IEntity
    {
        modelBuilder.Entity<TEntity>().HasQueryFilter(e => e.Active ?? false);
    }
    
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<City> Cities => Set<City>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Branch> Branches => Set<Branch>();
}


