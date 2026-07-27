using System.Linq.Expressions;
using Start_Template_CSharp.Core.Responses;

namespace Start_Template_CSharp.Core.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    Task<List<TEntity>> GetAllAsync<TKeyOrderBy>(Func<TEntity, bool>? searchQuery, Func<TEntity, TKeyOrderBy> orderBy);
    Task<ApiResponse<TEntity>> GetByIdAsync(Guid id);
    Task<ApiResponse<TEntity>> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(Guid id, TEntity entity);
    Task<ApiResponse<string>> DeleteAsync(Guid id);
    Task<ApiResponse<string>> DeleteAllAsync();
}