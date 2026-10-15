using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ticketing.Application.Contracts.Repositories;
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
