namespace Ticketing.Domain.Entites;

public class NormalUser : BaseEntity
{
    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
    public Department Department { get; set; } = null!;
    public Guid DepartmentId { get; set; }
    public List<Ticket> Tickets { get; set; } = [];
}
