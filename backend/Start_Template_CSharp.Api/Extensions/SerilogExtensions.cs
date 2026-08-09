using Serilog;

namespace Start_Template_CSharp.Api.Extensions;

internal static class SerilogExtensions
{
    internal static void AddSerilogDi(this WebApplicationBuilder builder)
    {
        builder.Services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services));
    }
}
