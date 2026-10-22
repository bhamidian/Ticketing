using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticketing.Domain.Entites
{
    public class Admin : BaseEntity
    {
        public AppUser? AppUser { get; set; } = null!;
        public Guid? AppUserId { get; set; }


    }
}
