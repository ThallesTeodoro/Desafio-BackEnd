using DesafioBackEnd.Domain.Contracts.Persistence;
using Microsoft.EntityFrameworkCore;

namespace DesafioBackEnd.Infrastructure.Persistence.Repositories;

public abstract class Repository<TEntity> : IRepository<TEntity>
    where TEntity : class
{
    protected readonly ApplicationDbContext _dbContext;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddAsync(TEntity entity)
    {
        await _dbContext.AddAsync(entity);
    }

    public async Task AddManyAsync(List<TEntity> entities)
    {
        await _dbContext.AddRangeAsync(entities);
    }

    public async Task<List<TEntity>> AllAsync()
    {
        return await _dbContext
            .Set<TEntity>()
            .ToListAsync();
    }

    public async Task<TEntity?> FindByIdAsync(Guid id)
    {
        return await _dbContext
            .Set<TEntity>()
            .FindAsync(id);
    }

    public void Remove(TEntity entity)
    {
        _dbContext
            .Set<TEntity>()
            .Remove(entity);
    }

    public void Update(TEntity entity)
    {
        _dbContext
            .Set<TEntity>()
            .Update(entity);
    }
}
