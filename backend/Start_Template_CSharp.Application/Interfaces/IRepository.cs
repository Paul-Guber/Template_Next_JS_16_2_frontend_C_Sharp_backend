using Microsoft.EntityFrameworkCore.Query;

namespace Start_Template_CSharp.Application.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<IList<TEntity>> GetAllAsync();
    Task<TEntity?> GetAsync(Func<TEntity, bool> predicate);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(Func<TEntity, bool> predicate);
    Task DeleteAllAsync();
    Task<IList<TEntity>> SearchByFilterAsync(
        Func<IQueryable<TEntity>,IQueryable<TEntity>>? expression = null,
        CancellationToken cancellationToken = default);
}
