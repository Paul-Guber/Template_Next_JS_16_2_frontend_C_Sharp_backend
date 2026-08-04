using Serilog;
using Start_Template_CSharp.Api;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();
try
{
    Log.Information("Сервер успешно запущен!");
    
    var builder = WebApplication.CreateBuilder(args);

//Здесь мы подключаем все DI из всех слоёв, там же задаем настройки подключения к БД
    builder.AddMyBuilder();

    var app = builder.Build();

    app.AddApplicationDi(app.Configuration);
 
    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "Неожиданное завершение работы сервера!");
    Console.WriteLine("Сообщение об ошибке = " + ex.Message);
}
finally
{
    Log.CloseAndFlush();
}
 