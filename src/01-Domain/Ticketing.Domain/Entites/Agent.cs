namespace Ticketing.Domain.Entites;

public class Agent : BaseEntity
{
    public Guid AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;
}
