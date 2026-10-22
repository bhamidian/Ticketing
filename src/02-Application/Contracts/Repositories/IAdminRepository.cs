using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Ticketing.Domain.Entites;

namespace Ticketing.Application.Contracts.Repositories
{
    public interface IAdminRepository
    {
        public Task<Admin?> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default);
        public Task<bool> AssignTicketToAgentByIdAsync(Guid ticketId, Guid agentId, Guid adminUserId, CancellationToken cancellationToken = default);
    }
}
