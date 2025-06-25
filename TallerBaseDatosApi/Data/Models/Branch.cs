namespace TallerBaseDatosApi.Data.Models;
[Table(nameof(Branch), Schema ="DBO")]
public class Branch  : Entity<long>
{
    [MaxLength(25)]
    public required string Name { get; set; }
    [MaxLength(60)]
    public required string Address { get; set; }
    public long CityId { get; set; }
    public virtual City City { get; set; } = default!;
    public virtual ICollection<Order> Orders { get; set; } = [];
}