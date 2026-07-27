using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Start_Template_CSharp.Core.Entities;
using Start_Template_CSharp.Core.Interfaces;
using Start_Template_CSharp.Infrastructure.Repository;

namespace Start_Template_CSharp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDi(this IServiceCollection services, IConfiguration configuration
         )
    {
         
        services.AddScoped<IRepository<EmployeeEntity>,Repository<EmployeeEntity>>();
        
        return services;
    }
}