using DotNetEnv;
using DotNetEnv.Configuration;
using FluentValidation;
using Microsoft.OpenApi;
using Serilog;
using Start_Template_CSharp.Api.EndPoints.Extensions;
using Start_Template_CSharp.Application;
using Start_Template_CSharp.Core;
using Start_Template_CSharp.Infrastructure;
using Start_Template_CSharp.Infrastructure.Context;


namespace Start_Template_CSharp.Api;

public static class DependencyInjection
{
    public static void AddMyBuilder(this WebApplicationBuilder builder)
    {
        // Подключаем логирование SeriaLog
        builder.AddSeriaLogDi();
        
        // Подключаем файл .env в конфигурацию,
        // Чтобы прочитать значение в любом месте нужно использовать:
        // System.Environment.GetEnvironmentVariable("IP"); - Где IP это ключ в файле .env
        builder.Configuration.AddDotNetEnv(".env", LoadOptions.TraversePath());
        
        // Подключаем fluent validator в DI
        builder.Services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        
        builder.Services.AddApiDi(builder.Configuration);
        
        builder.AddSwaggerDi();
        
        builder.Services.AddOpenApi();
        
        builder.Services.AddMyEndPoints(typeof(Program).Assembly);
    }

    private static void AddSeriaLogDi(this WebApplicationBuilder builder)
    {
        builder.Services.AddSerilog((services, lc) => lc
            .ReadFrom.Configuration(builder.Configuration)
            .ReadFrom.Services(services));
         
    }
    
    private static void AddApiDi(this IServiceCollection services, IConfiguration configuration)
    {
        // Региструем службу для подключения к бд
        services.AddDbContext<ApplicationDbContext>();
        
       // Подключаем DI из других слоёв приложения.
        services
            .AddApplicationDi()
            .AddInfrastructureDi(configuration)
            .AddCoreDi();
    }
   
    private static void AddSwaggerDi(this WebApplicationBuilder builder)
    {
        // Конфигур
         builder.Services.AddSwaggerGen(options => options.SwaggerDoc("v1", new OpenApiInfo()
        {
            Title = "Start template API",
            Version = "v1",
            Contact = new OpenApiContact()
            {
                Name = "Contact Author",
                Email = "author@gmail.com",
            },
        }));
    }

    public static void AddApplicationDi(this WebApplication app, IConfiguration configuration)
    {
        //app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();
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
                if (httpContext.Request.Path.StartsWithSegments("/health"))
                    return Serilog.Events.LogEventLevel.Verbose;

                return elapsed > 500
                    ? Serilog.Events.LogEventLevel.Warning
                    : Serilog.Events.LogEventLevel.Information;
            };
        });
        app.UseMyEndPoints();
    }
}