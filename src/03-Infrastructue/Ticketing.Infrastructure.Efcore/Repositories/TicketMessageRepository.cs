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

    public async Task<TicketMessage?> GetByTicketIdAdminAsync(
        Guid ticketId,
        Guid messageId,
        CancellationToken cancellationToken = default)
    {
        var message = await _dbContext.TicketMessages
            .Include(message => message.Sender)
            .Where(message => message.TicketId == ticketId && message.IsConfirmed == false && message.Id == messageId && message.Sender.Agent != null)
            .FirstOrDefaultAsync(cancellationToken);


        return message;
    }

    public async Task<List<TicketMessage>> GetByTicketIdAsync(
        Guid ticketId,
        CancellationToken cancellationToken = default)
    {
        var messages = await _dbContext.TicketMessages
            .AsNoTracking()
            .Where(message => message.TicketId == ticketId && message.IsConfirmed == true)
            .Include(message => message.Sender)
            .OrderBy(message => message.CreatedAt)
            .ToListAsync(cancellationToken);

        return messages;
    }

    public async Task<bool> ConfirmAgentMessageAsync(
        Guid messageId,
        Guid adminUserId,
        CancellationToken cancellationToken = default
    )
    {
        var now = DateTime.UtcNow;
        var affected = await _dbContext.TicketMessages
            .Where(x => x.Id == messageId && x.IsConfirmed == false)
            .ExecuteUpdateAsync(setters => setters
            .SetProperty(c => c.IsConfirmed, true)
            .SetProperty(c => c.UpdatedAt, now)
            .SetProperty(c => c.UpdatedById, adminUserId), cancellationToken);

        return affected > 0;
    }
}
