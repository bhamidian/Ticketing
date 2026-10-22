using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.DTOs.AgentDtos;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Repositories;

public sealed class AgentRepository : Repository<Agent>, IAgentRepository
{
    private readonly ApplicationDbContext _dbContext;

    public AgentRepository(ApplicationDbContext dbContext, ILogger<Repository<Agent>> logger) : base(dbContext, logger)
    {
        _dbContext = dbContext;
    }

    public Task<List<GetAllAgentDto>> GetAllAgentsAsync(CancellationToken cancellationToken = default)
    {
        var agents = _dbContext.Agents
            .AsNoTracking()
            .Select(agent => new GetAllAgentDto
            {
                Id = agent.AppUserId,
                Email = agent.AppUser.Email
            })
            .ToListAsync(cancellationToken);

        return agents;
    }

    public Task<List<GetReportAgentDto>> GetAgentResponseCountsAsync(CancellationToken cancellationToken = default)
    {
        var agents = _dbContext.Agents
            .AsNoTracking()
            .Select(agent => new GetReportAgentDto
            {
                Id = agent.AppUserId,
                Email = agent.AppUser.Email!,
                RespondedTicketsCount = _dbContext.TicketMessages
                    .Where(message => message.SenderId == agent.AppUserId && message.IsConfirmed == true)
                    .Select(message => message.TicketId)
                    .Distinct()
                    .Count()
            })
            .OrderByDescending(agent => agent.RespondedTicketsCount)
            .ThenBy(agent => agent.Email)
            .ToListAsync(cancellationToken);

        return agents;
    }

    public async Task<Agent?> GetByAppUserIdAsync(
        Guid appUserId,
        CancellationToken cancellationToken = default)
    {
        var agent = await _dbContext.Agents
            .AsNoTracking()
            .SingleOrDefaultAsync(agent => agent.AppUserId == appUserId, cancellationToken);

        return agent;
    }
}
