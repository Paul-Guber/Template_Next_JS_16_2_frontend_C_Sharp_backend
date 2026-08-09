using Serilog;
using Start_Template_CSharp.Api;
using Start_Template_CSharp.Api.Extensions;

#pragma warning disable CA1305
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console(outputTemplate: "{Timestamp:HH:mm} [{Level}] ({ThreadId}) {Message}{NewLine}{Exception}")
#pragma warning restore CA1305
    .CreateBootstrapLogger();
try
{
    Log.Information("Сервер успешно запущен!");

    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

    //Здесь мы подключаем все DI из всех слоёв, там же задаем настройки подключения к БД
    builder.AddMyBuilder();

    WebApplication app = builder.Build();
    // Настройка Application
    app.AddApplicationDi( );

   await app.RunAsync().ConfigureAwait(false);

}
catch (Exception ex)
{
    Log.Fatal(ex, "Неожиданное завершение работы сервера!");
    Console.WriteLine("Сообщение об ошибке = " + ex.Message);
}
finally
{
   await Log.CloseAndFlushAsync().ConfigureAwait(false);
}
