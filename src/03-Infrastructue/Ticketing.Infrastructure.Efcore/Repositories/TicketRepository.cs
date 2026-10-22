using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Domain.Entites;
using Ticketing.Domain.Enums;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Repositories;

public sealed class TicketRepository : Repository<Ticket>, ITicketRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TicketRepository(ApplicationDbContext dbContext, ILogger<Repository<Ticket>> logger) : base(dbContext, logger)
    {
        _dbContext = dbContext;
    }

    public async Task<Ticket?> GetWithMessagesAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var ticket = await _dbContext.Tickets
            .AsNoTracking()
            .Include(ticket => ticket.AssignedAgent)
            .Include(ticket => ticket.NormalUser)
                .ThenInclude(user => user.AppUser)
            .Include(ticket => ticket.NormalUser)
                .ThenInclude(user => user.Department)
            .Include(ticket => ticket.TicketMessages)
                .ThenInclude(message => message.Sender)
            .SingleOrDefaultAsync(ticket => ticket.Id == id, cancellationToken);

        return ticket;
    }

    public async Task<(IReadOnlyList<Ticket> Items, int TotalCount)> GetFilteredAsync(
        Guid? normalUserId,
        TicketLifeCycleEnum? status,
        TicketPriorityEnum? priority,
        Guid? visibleToAgentId,
        bool assignedOnly,
        bool unassignedOnly,
        int page,
        int pageSize,
        bool oldestFirst,
        CancellationToken cancellationToken = default)
    {
        var query = _dbContext.Tickets.AsNoTracking().AsQueryable();

        if (normalUserId.HasValue)
            query = query.Where(ticket => ticket.NormalUserId == normalUserId.Value);
        if (status.HasValue)
            query = query.Where(ticket => ticket.TicketLifeCycle == status.Value);
        if (priority.HasValue)
            query = query.Where(ticket => ticket.TicketPriority == priority.Value);
        if (visibleToAgentId.HasValue && assignedOnly)
            query = query.Where(ticket => ticket.AssignedAgentId == visibleToAgentId.Value);
        else if (visibleToAgentId.HasValue && unassignedOnly)
            query = query.Where(ticket => ticket.AssignedAgentId == null);
        else if (visibleToAgentId.HasValue)
            query = query.Where(ticket => ticket.AssignedAgentId == null
                || ticket.AssignedAgentId == visibleToAgentId.Value);

        var totalCount = await query.CountAsync(cancellationToken);
        var orderedQuery = oldestFirst
            ? query.OrderBy(ticket => ticket.CreatedAt).ThenBy(ticket => ticket.Id)
            : query.OrderByDescending(ticket => ticket.CreatedAt).ThenByDescending(ticket => ticket.Id);
        var items = await orderedQuery
            .Include(ticket => ticket.AssignedAgent)
            .Include(ticket => ticket.NormalUser)
                .ThenInclude(user => user.AppUser)
            .Include(ticket => ticket.NormalUser)
                .ThenInclude(user => user.Department)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        return (items, totalCount);
    }

    public async Task<bool> TryAssignToAgentAsync(
        Guid ticketId,
        Guid agentUserId,
        DateTime assignedAtUtc,
        CancellationToken cancellationToken = default)
    {
        var claimed = await _dbContext.Tickets
            .Where(ticket => ticket.Id == ticketId
                && ticket.AssignedAgentId == null
                && ticket.TicketLifeCycle != TicketLifeCycleEnum.CLOSED)
            .ExecuteUpdateAsync(setters => setters
                .SetProperty(ticket => ticket.AssignedAgentId, agentUserId)
                .SetProperty(ticket => ticket.TicketLifeCycle, TicketLifeCycleEnum.INPROGRESS)
                .SetProperty(ticket => ticket.UpdatedAt, assignedAtUtc)
                .SetProperty(ticket => ticket.UpdatedById, agentUserId), cancellationToken);

        if (claimed == 1)
            return true;

        return await _dbContext.Tickets.AnyAsync(ticket => ticket.Id == ticketId
            && ticket.AssignedAgentId == agentUserId
            && ticket.TicketLifeCycle != TicketLifeCycleEnum.CLOSED, cancellationToken);
    }


}
