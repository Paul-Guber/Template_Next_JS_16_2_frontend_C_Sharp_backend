using Microsoft.AspNetCore.CookiePolicy;
using Serilog;
using Start_Template_CSharp.Api.EndPoints.Extensions;

namespace Start_Template_CSharp.Api.Extensions;

internal static class ApplicationExtensions
{
    internal static void AddApplicationDi(this WebApplication app)
    {
        // app.UseHttpsRedirection();
        //app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
        // app.UseCookiePolicy(new CookiePolicyOptions()
        // {
        //     Secure = CookieSecurePolicy.Always,
        //     MinimumSameSitePolicy = SameSiteMode.Strict,
        //     HttpOnly = HttpOnlyPolicy.Always
        // });
        // app.UseCors();

        app.UseSerilogRequestLogging(options =>
        {
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent.ToString());
            };

            // Exclude health check endpoints from request logs
            options.GetLevel = (httpContext, elapsed, _) =>
            {
                if (httpContext.Request.Path.StartsWithSegments("/health"
                        , StringComparison.Ordinal))
                {
                    return Serilog.Events.LogEventLevel.Verbose;
                }

                return elapsed > 500
                    ? Serilog.Events.LogEventLevel.Warning
                    : Serilog.Events.LogEventLevel.Information;
            };
        });
        // app.UseAuthentication();
        // app.UseAuthorization();
        app.UseMyEndPoints();
    }
}
