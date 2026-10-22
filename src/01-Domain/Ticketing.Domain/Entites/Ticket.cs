using Ticketing.Domain.Enums;

namespace Ticketing.Domain.Entites;

public class Ticket : BaseEntity
{
    public string Subject { get; set; } = null!;
    public TicketLifeCycleEnum TicketLifeCycle { get; set; }
    public TicketPriorityEnum TicketPriority { get; set; }
    public List<TicketMessage> TicketMessages { get; set; } = [];
    public DateTime? ClosedAt { get; set; }
    public Guid? ClosedById { get; set; }
    public NormalUser NormalUser { get; set; } = null!;
    public Guid NormalUserId { get; set; }
    public Guid? AssignedAgentId { get; set; }
    public AppUser? AssignedAgent { get; set; }
}
