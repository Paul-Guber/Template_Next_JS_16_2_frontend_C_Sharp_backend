using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Start_Template_CSharp.Application.Interfaces;
using Start_Template_CSharp.Core.Entities;
using Start_Template_CSharp.Infrastructure.Repository;
using Start_Template_CSharp.Infrastructure.Services;

namespace Start_Template_CSharp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureDi(this IServiceCollection services, IConfiguration configuration
         )
    {
         
        services.AddScoped<IRepository<EmployeeEntity>,Repository<EmployeeEntity>>();
        services.AddScoped<IEmployeeServices, EmployeeServices>();
        return services;
    }
}