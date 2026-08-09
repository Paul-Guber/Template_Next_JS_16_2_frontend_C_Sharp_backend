using Microsoft.OpenApi;

namespace Start_Template_CSharp.Api.Extensions;

internal static class SwaggerExtensions
{
    /// <summary>
    /// Конфигурация Swagger
    /// </summary>
    internal static void AddSwaggerDi(this WebApplicationBuilder builder)
    {

        // Конфигурация Swagger
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
}
