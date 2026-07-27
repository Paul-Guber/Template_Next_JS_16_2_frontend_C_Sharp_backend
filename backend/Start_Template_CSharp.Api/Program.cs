using Start_Template_CSharp.Api;

var builder = WebApplication.CreateBuilder(args);

//Здесь мы подключаем все DI из всех слоёв, там же задаем настройки подключения к БД
builder.AddMyBuilder();

var app = builder.Build();

app.AddApplicationDi(app.Configuration);
 
app.Run();

 