using Microsoft.AspNetCore.Identity;

namespace Ticketing.Domain.Entites;

public class AppUser : IdentityUser<Guid>
{
    public NormalUser? NormalUser { get; set; }
    public Agent? Agent { get; set; }
    public Admin? Admin { get; set; }
    public ICollection<TicketMessage> SentTicketMessages { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<Ticket> AssignedTickets { get; set; } = [];
}
