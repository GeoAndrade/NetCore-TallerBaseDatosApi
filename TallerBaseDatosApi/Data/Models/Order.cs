namespace TallerBaseDatosApi.Data.Models;
[Table(nameof(Order), Schema ="DBO")]
public class Order : Entity<long>
{
    public long BranchId { get; set; }
    public virtual Branch Branch { get; set; } = default!;
    public long CustomerId { get; set; }
    public virtual Customer Customer { get; set; } = default!;
    public required DateTime Date { get; set; } 
    [Precision(10, 2)]
    public required decimal Total { get; set; }
    public required int Amount { get; set; }
}