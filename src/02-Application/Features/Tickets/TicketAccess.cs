using Ticketing.Application.Contracts.Repositories;
using Ticketing.Application.Results;
using Ticketing.Domain.Entites;

namespace Ticketing.Application.Features.Tickets;

internal static class TicketAccess
{
    public static async Task<ApplicationError?> GetAccessErrorAsync(
        Ticket ticket,
        Guid userId,
        bool isStaff,
        INormalUserRepository normalUsers,
        CancellationToken cancellationToken)
    {
        if (isStaff)
        {
            if (ticket.AssignedAgentId is not null && ticket.AssignedAgentId != userId)
                return ApplicationErrors.Forbidden("این تیکت به عامل دیگری واگذار شده است.");

            return null;
        }

        var normalUser = await normalUsers.GetByAppUserIdAsync(userId, cancellationToken);
        if (normalUser is null || normalUser.Id != ticket.NormalUserId)
            return ApplicationErrors.Forbidden("شما به این تیکت دسترسی ندارید.");

        return null;
    }
}
