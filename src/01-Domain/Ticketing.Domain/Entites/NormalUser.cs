namespace Ticketing.Domain.Entites
{
    public class NormalUser : BaseEntity
    {
        public Guid AppUserId { get; set; }
        public AppUser AppUser { get; set; } = null!;
        public List<Ticket> Tickets {get; set;} = [];
    }
}
