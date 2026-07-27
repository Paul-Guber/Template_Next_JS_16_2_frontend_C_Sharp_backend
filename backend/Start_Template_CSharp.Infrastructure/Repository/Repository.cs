using Microsoft.EntityFrameworkCore;
using Start_Template_CSharp.Core.Interfaces;
using Start_Template_CSharp.Core.Responses;
using Start_Template_CSharp.Infrastructure.Context;

namespace Start_Template_CSharp.Infrastructure.Repository;

public class Repository<TEntity>(ApplicationDbContext context) : IRepository<TEntity>
    where TEntity : class
{
    public async Task<List<TEntity>> GetAllAsync<TKeyOrderBy>(
       Func<TEntity, bool>?  searchQuery, Func<TEntity, TKeyOrderBy> orderBy )
    {
       List<TEntity> query = searchQuery is not null ?
               await context.Set<TEntity>().AsNoTracking().AsAsyncEnumerable().Where(searchQuery).ToListAsync()
                : await context.Set<TEntity>().AsNoTracking().ToListAsync();
             
           var find = await query
                                         .OrderBy(orderBy)
                                         .ToAsyncEnumerable().ToListAsync();  
            return find;
    }

    public async Task<ApiResponse<TEntity>> GetByIdAsync(Guid id)
    {
        var find = await context.Set<TEntity>().FindAsync(id);
        var message = find is null ? null : "Данные найдены";
        var errormessage = find is null ? "Данные не найдены!" : null;
        return ApiResponse<TEntity>.MyResponseApi(data:find, message: message);
    }
    public async Task<ApiResponse<TEntity>> CreateAsync(TEntity entity)
    {
      var create = context.Set<TEntity>().Add(entity);
        await context.SaveChangesAsync();
        return ApiResponse<TEntity>.MyResponseApi(data: create.Entity, message: "Данные успешно добавлены");
    }
    
    public async Task<TEntity> UpdateAsync(Guid id, TEntity entity)
    {
         var resp = context.Set<TEntity>().Update(entity);
            await context.SaveChangesAsync();
            return resp.Entity;
         
    }

    public async Task<ApiResponse<string>> DeleteAsync(Guid id)
    {
        ApiResponse<TEntity> find = await GetByIdAsync(id);
        if (find.Data is null)
        {
            return ApiResponse<string>.MyResponseApi(message: "Ошибка при удалении!");  
        } 
        context.Set<TEntity>().Remove(find.Data);
        await context.SaveChangesAsync();
        return ApiResponse<string>.MyResponseApi(message: "Данные успешно удалены!");  
    }

    public async Task<ApiResponse<string>> DeleteAllAsync( )
    {
        await context.Set<TEntity>().ExecuteDeleteAsync();
        return ApiResponse<string>.MyResponseApi(data: null , message: "Данные успешно удалены!");  
    }
}