
namespace TallerBaseDatosApi.Abstractions;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result,
        CancellationToken cancellationToken = new())
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void UpdateEntities(DbContext? context)
    {
        if (context is null) return;
        foreach (var entry in context.ChangeTracker.Entries<IEntity>())
        {
            if (entry.State != EntityState.Added && entry.State != EntityState.Modified &&
                !entry.HasChangedOwnedEntities()) continue;
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedBy = "System";
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.Active = true;
                    break;
                case EntityState.Modified:
                    if (entry.Entity.Active is false)
                    {
                        entry.Entity.DeletedBy = "System";
                        entry.Entity.DeletedAt = DateTime.UtcNow;
                        entry.Entity.Active = false;
                    }
                    else
                    {
                        entry.Entity.LastModifiedBy = "System";
                        entry.Entity.LastModified = DateTime.UtcNow;
                    }
                    break;
                case EntityState.Deleted:
                    /*entry.State = EntityState.Modified;
                    entry.Entity.DeletedBy = "System";
                    entry.Entity.DeletedAt = DateTime.UtcNow;
                    entry.Entity.Active = false;
                    break;*/
                case EntityState.Detached:
                case EntityState.Unchanged:
                    break;
                default:
                    return;
            }
        }
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r =>
            r.TargetEntry is not null &&
            r.TargetEntry.Metadata.IsOwned() &&
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}