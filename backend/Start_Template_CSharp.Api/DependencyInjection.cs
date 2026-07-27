using DotNetEnv;
using DotNetEnv.Configuration;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using Start_Template_CSharp.Api.EndPoints.Extensions;
using Start_Template_CSharp.Application;
using Start_Template_CSharp.Core;
using Start_Template_CSharp.Infrastructure;
using Start_Template_CSharp.Infrastructure.Context;


namespace Start_Template_CSharp.Api;

public static class DependencyInjection
{
    public static WebApplicationBuilder AddMyBuilder(this WebApplicationBuilder builder   )
    {
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
        
        return builder;
    }
    
    private static void AddApiDi(this IServiceCollection services, IConfiguration configuration)
    {
            // Ищем ключ "CONNECTION_DB" из файла .env 
        string connectionString = Environment.GetEnvironmentVariable("CONNECTION_DB") ?? "";
        
        // Региструем службу для подключения к бд
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(@connectionString, b
                => b.MigrationsAssembly("Start_Template_CSharp.Infrastructure")));
        
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
        app.UseMyEndPoints();
    }
}