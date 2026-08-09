using Start_Template_CSharp.Application;
using Start_Template_CSharp.Core;
using Start_Template_CSharp.Infrastructure;

namespace Start_Template_CSharp.Api.Extensions;

internal static class AddOtherLayersExtension
{
    internal static void AddOtherLayers(this IServiceCollection services, IConfiguration configuration)
    {
        // Подключаем DI из других слоёв приложения.
        services
            .AddApplicationDi()
            .AddInfrastructureDi(configuration)
            .AddCoreDi();
    }
}
