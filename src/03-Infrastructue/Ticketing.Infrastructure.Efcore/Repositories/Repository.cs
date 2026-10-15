using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Ticketing.Application.Contracts.Repositories;
using Ticketing.Domain.Entites;
using Ticketing.Infrastructure.Efcore.Persistence;

namespace Ticketing.Infrastructure.Efcore.Repositories;

public class Repository<TEntity>(ApplicationDbContext dbContext, ILogger<Repository<TEntity>> logger) : IRepository<TEntity>
    where TEntity : BaseEntity
{
    protected DbSet<TEntity> Set => dbContext.Set<TEntity>();

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Set.FindAsync(new object?[] { id }, cancellationToken);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await Set.AsNoTracking().ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
    {
        await Set.AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        Set.Update(entity);
    }

    public void Delete(TEntity entity)
    {
        Set.Remove(entity);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var changes = await dbContext.SaveChangesAsync(cancellationToken);
            return changes;
        }
        catch (Exception exception)
        {
            logger.LogError(exception, "Failed to persist changes for entity type {EntityType}", typeof(TEntity).Name);
            throw;
        }
    }
}
