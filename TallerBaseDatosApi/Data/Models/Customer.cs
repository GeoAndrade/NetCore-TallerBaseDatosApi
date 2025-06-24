namespace TallerBaseDatosApi.Data.Models;
[Table(nameof(Customer), Schema ="DBO")]
public class Customer  : Entity<long>
{
    public string Name { get; set; } = default!;
    public virtual ICollection<Order> Orders { get; set; } = [];
}