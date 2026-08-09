using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Start_Template_CSharp.Api.EndPoints.Extensions;

public static class EndPointsExtensions
{
    public static IServiceCollection AddMyEndPoints(this IServiceCollection services, Assembly assembly)
    {
        IEnumerable<ServiceDescriptor> serviceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type));
        services.TryAddEnumerable(serviceDescriptors);
        return services;
    }

    public static IApplicationBuilder UseMyEndPoints(this WebApplication app, RouteGroupBuilder? routeGroupBuilder = null)
    {
        IEnumerable<IEndpoint> endPoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();

        IEndpointRouteBuilder builder = routeGroupBuilder is null ? app : routeGroupBuilder;
        foreach (IEndpoint endPoint in endPoints)
        {
            endPoint.MapEndPointCreate(builder);
        }
        return app;
    }
}