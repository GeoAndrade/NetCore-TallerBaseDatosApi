namespace TallerBaseDatosApi.Abstractions;

public abstract class Entity<T> : IEntity<T> where T : notnull
{
    public T Id { get; set; } = default!;
    public DateTime? CreatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string? DeletedBy { get; set; }
    public bool? Active { get; set; }
}