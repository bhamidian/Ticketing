using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Repositories
{
    public class AdminRepository(ApplicationDbContext dbContext) : IAdminRepository
    {
        public async Task<bool> AssignTicketToAgentByIdAsync(Guid ticketId, Guid agentId, Guid adminUserId, CancellationToken cancellationToken = default)
        {
            var ticket = await dbContext.Tickets.FirstOrDefaultAsync(t => t.Id == ticketId, cancellationToken);
            var agent = await dbContext.Agents.FirstOrDefaultAsync(a => a.AppUserId == agentId, cancellationToken);
            if (ticket is null || agent is null || ticket.TicketLifeCycle == Ticketing.Domain.Enums.TicketLifeCycleEnum.CLOSED)
                return false;

            ticket.AssignedAgentId = agent.AppUserId;
            ticket.TicketLifeCycle = Ticketing.Domain.Enums.TicketLifeCycleEnum.INPROGRESS;
            ticket.UpdatedAt = DateTime.UtcNow;
            ticket.UpdatedById = adminUserId;
            return await dbContext.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<Admin?> GetByAppUserIdAsync(Guid appUserId, CancellationToken cancellationToken = default)
        {
            var admin = await dbContext.Admins
                .AsNoTracking()
                .SingleOrDefaultAsync(admin => admin.AppUserId == appUserId, cancellationToken);

            return admin;
        }
    }
}
