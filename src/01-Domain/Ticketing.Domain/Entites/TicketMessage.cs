using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticketing.Domain.Entites
{
    public class TicketMessage : BaseEntity
    {
        public string Message { get; set; } = null!;
        public string? PictureUrl { get; set; }
        public Ticket Ticket {get;set;} = null!;
        public Guid TicketId {get; set;}
        public AppUser Sender { get; set; } = null!;
        public Guid SenderId { get; set; }

    }
}
