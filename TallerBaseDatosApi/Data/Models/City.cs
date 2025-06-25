namespace TallerBaseDatosApi.Data.Models;
[Table(nameof(City), Schema ="DBO")]
public class City  : Entity<long>
{
    [MaxLength(40)]
    public string Name { get; set; } = default!;
    public virtual ICollection<Branch> Branches { get; set; } = [];
}