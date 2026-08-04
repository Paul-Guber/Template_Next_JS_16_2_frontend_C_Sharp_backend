using Microsoft.EntityFrameworkCore;
using Start_Template_CSharp.Application.Interfaces;
using Start_Template_CSharp.Infrastructure.Context;

namespace Start_Template_CSharp.Infrastructure.Repository;

public class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private readonly IQueryable<TEntity> _query = context.Set<TEntity>().AsNoTracking();

    public async Task<List<TEntity>> GetAllAsync() =>
        await _query.ToListAsync();

    public async Task<TEntity?> GetAsync(Func<TEntity, bool> predicate) =>
        await _query.AsAsyncEnumerable().FirstOrDefaultAsync(predicate);
     
    public async Task<TEntity> CreateAsync(TEntity entity)  
    {
       var entityCreate = await _dbSet.AddAsync(entity);
        await context.SaveChangesAsync();
        
        return entityCreate.Entity;
    }
    
    public async Task<TEntity> UpdateAsync(Guid id, TEntity entity)
    {
        var resp = _dbSet.Update(entity);
        await context.SaveChangesAsync();
        return resp.Entity;
    }

    public async Task<bool> DeleteAsync(Func<TEntity, bool> predicate)
    {
        TEntity? find = await GetAsync(predicate);
        if (find is null) return false;
        _dbSet.Remove(find);
        int affectedRows = await context.SaveChangesAsync();
        return affectedRows > 0;  
    }

    public async Task DeleteAllAsync () =>
        await _dbSet.ExecuteDeleteAsync();
    


    public async Task<List<TEntity>> SearchByFilterAsync(
        Func<IQueryable<TEntity>,IQueryable<TEntity>>? expression = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _query;
        if (expression is not null)
        {
            query = expression(query);   
        }
        return await query.ToListAsync(cancellationToken);
    }
}