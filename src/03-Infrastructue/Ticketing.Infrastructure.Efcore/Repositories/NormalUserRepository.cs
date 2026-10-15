using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Repositories;

public sealed class NormalUserRepository : Repository<NormalUser>, INormalUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public NormalUserRepository(ApplicationDbContext dbContext, ILogger<Repository<NormalUser>> logger) : base(dbContext, logger)
    {
        _dbContext = dbContext;
    }

    public async Task<NormalUser?> GetByAppUserIdAsync(
        Guid appUserId,
        CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.NormalUsers
            .AsNoTracking()
            .SingleOrDefaultAsync(user => user.AppUserId == appUserId, cancellationToken);

        return user;
    }
}
