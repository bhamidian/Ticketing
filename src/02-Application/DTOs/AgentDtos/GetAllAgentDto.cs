using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ticketing.Application.DTOs.AgentDtos
{
    public class GetAllAgentDto
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = null!;
    }
}
