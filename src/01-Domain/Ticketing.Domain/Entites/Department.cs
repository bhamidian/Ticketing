using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticketing.Domain.Entites
{
    public class Department : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public List<NormalUser> NormalUsers { get; set; } = new List<NormalUser>();

    }
}