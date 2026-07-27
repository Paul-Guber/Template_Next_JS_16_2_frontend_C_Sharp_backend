using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Start_Template_CSharp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationDi(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        return services;
    }
}