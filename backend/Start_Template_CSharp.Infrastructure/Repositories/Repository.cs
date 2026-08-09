using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Start_Template_CSharp.Application.Interfaces;
using Start_Template_CSharp.Infrastructure.Context;

namespace Start_Template_CSharp.Infrastructure.Repositories;

public class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();
    private readonly IQueryable<TEntity> _query = context.Set<TEntity>().AsNoTracking();

    public async Task<IList<TEntity>> GetAllAsync() =>
        await _query.ToListAsync().ConfigureAwait(false);

    public async Task<TEntity?> GetAsync(Func<TEntity, bool> predicate) =>
        await _query.AsAsyncEnumerable().FirstOrDefaultAsync(predicate).ConfigureAwait(false);

    public async Task<TEntity> CreateAsync(TEntity entity)
    {
       EntityEntry<TEntity> entityCreate = await _dbSet.AddAsync(entity).ConfigureAwait(false);
        await context.SaveChangesAsync().ConfigureAwait(false);

        return entityCreate.Entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        EntityEntry<TEntity> updateEntity = _dbSet.Update(entity);
        Console.WriteLine($"{updateEntity.Entity}");
        try
        {
            await context.SaveChangesAsync().ConfigureAwait(false);
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine("InnerException Message = " + ex.InnerException);
            var innerException = ex.InnerException as SqlException;
            if (innerException?.Number == 2601)
            {
                Console.WriteLine("innerException Message = " + innerException);

            }
        }

        return updateEntity.Entity;
    }

    public async Task<bool> DeleteAsync(Func<TEntity, bool> predicate)
    {
        TEntity? find = await GetAsync(predicate).ConfigureAwait(false);
        if (find is null)
        {
            return false;
        }

        _dbSet.Remove(find);
        int affectedRows = await context.SaveChangesAsync().ConfigureAwait(false);
        return affectedRows > 0;
    }

    public Task DeleteAllAsync () =>
_dbSet.ExecuteDeleteAsync();



    public async Task<IList<TEntity>> SearchByFilterAsync(
        Func<IQueryable<TEntity>,IQueryable<TEntity>>? expression = null,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = _query;
        if (expression is not null)
        {
            query = expression(query);
        }
        return await query.ToListAsync(cancellationToken).ConfigureAwait(false);
    }
}
