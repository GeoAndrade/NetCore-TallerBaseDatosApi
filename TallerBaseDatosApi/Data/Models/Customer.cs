namespace TallerBaseDatosApi.Data.Models;
[Table(nameof(Customer), Schema ="DBO")]
public class Customer  : Entity<long>
{
    [MaxLength(40)]
    public string Name { get; set; } = default!;
    public virtual ICollection<Order> Orders { get; set; } = [];
}