using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Repositories;

public sealed class TicketMessageRepository : Repository<TicketMessage>, ITicketMessageRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TicketMessageRepository(ApplicationDbContext dbContext, ILogger<Repository<TicketMessage>> logger) : base(dbContext, logger)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TicketMessage>> GetByTicketIdAsync(
        Guid ticketId,
        CancellationToken cancellationToken = default)
    {
        var messages = await _dbContext.TicketMessages
            .AsNoTracking()
            .Where(message => message.TicketId == ticketId)
            .Include(message => message.Sender)
            .OrderBy(message => message.CreatedAt)
            .ToListAsync(cancellationToken);

        return messages;
    }
}
