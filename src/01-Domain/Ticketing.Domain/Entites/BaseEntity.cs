namespace Ticketing.Domain.Entites
{
    public abstract class BaseEntity
    {
        public Guid Id {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public Guid CreatedById {get; set;}
        public Guid UpdatedById {get; set;}
    }
}