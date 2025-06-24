namespace TallerBaseDatosApi.Data.Models;
[Table(nameof(Branch), Schema ="DBO")]
public class Branch  : Entity<long>
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public long CityId { get; set; }
    public virtual City City { get; set; } = default!;
    public virtual ICollection<Order> Orders { get; set; } = [];
}