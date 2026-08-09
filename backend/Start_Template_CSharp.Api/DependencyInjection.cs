using DotNetEnv;
using DotNetEnv.Configuration;
using FluentValidation;
using Start_Template_CSharp.Api.EndPoints.Extensions;
using Start_Template_CSharp.Api.Extensions;
using Start_Template_CSharp.Infrastructure.Context;


namespace Start_Template_CSharp.Api;

public static class DependencyInjection
{
    public static void AddMyBuilder(this WebApplicationBuilder builder)
    {
        // Подключаем логирование SeriaLog
        builder.AddSerilogDi();

        // Подключаем файл .env в конфигурацию,
        // Чтобы прочитать значение в любом месте нужно использовать:
        // System.Environment.GetEnvironmentVariable("IP"); - Где IP это ключ в файле .env
        builder.Configuration.AddDotNetEnv(".env", LoadOptions.TraversePath());

        // Регистрируем службу для подключения к бд
        builder.Services.AddDbContext<ApplicationDbContext>();

        // Подключаем fluent validator в DI
        builder.Services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        // Подключаем DI из других слоёв приложения.
        builder.Services.AddOtherLayers(builder.Configuration);

        // Подключаем конфигурацию Swagger
        builder.AddSwaggerDi();

        builder.Services.AddOpenApi();

        builder.Services.AddMyEndPoints(typeof(Program).Assembly);
    }
}
