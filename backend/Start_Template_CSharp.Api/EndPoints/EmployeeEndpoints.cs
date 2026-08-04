using Start_Template_CSharp.Api.EndPoints.Extensions;
using Start_Template_CSharp.Application.Interfaces;
using Start_Template_CSharp.Core.Dto;
using Start_Template_CSharp.Core.Entities;
using Start_Template_CSharp.Core.models;
using Start_Template_CSharp.Core.Responses;


namespace Start_Template_CSharp.Api.EndPoints;

// Endpoints для сущности EmployeeEntity
// ReSharper disable once ClassNeverInstantiated.Global
// ReSharper disable once UnusedType.Global
internal sealed class EmployeeEndpoints : IEndpoint
{ 
     
    public void MapEndPointCreate(IEndpointRouteBuilder builder)
    {
        // Группа конечной точки 
        var groupEndpoints = builder.MapGroup("/api/employee");
        
        // Конечная точка: Возвращает все сущности EmployeeEntity из бд 
        groupEndpoints.MapGet("/getAll", GetAllEmployeeAsync);
        // Конечная точка: Возвращает сущность по id EmployeeEntity из бд 
        groupEndpoints.MapGet("/get/{id:guid}", GetEmployeeAsync);
        // Конечная точка: Добавление сущности в бд
        groupEndpoints.MapPost("/add", AddEmployeeAsync);
        // Конечная точка: Обновление сущности по id
        groupEndpoints.MapPut("/update/{id:guid}", UpdateEmployeeAsync);
        // Конечная точка: Удаление сущности по id
        groupEndpoints.MapDelete("/delete/{id:guid}", DeleteEmployeeAsync);
        // Конечная точка: Удаление всех таблиц сущности
        groupEndpoints.MapDelete("/fullDelete", DeleteAllEmployeeAsync);
    }

    // Функция возвращает все сущности из бд
    private static async Task<IResult> GetAllEmployeeAsync(
        string? searchQuery,
        int? page,
        int? limit,
        IEmployeeServices employeeServices)
    {
      var (employees, totalCount) = await employeeServices.GetAllEmployees( page ?? 1, limit ?? 2, searchQuery);
         var data = ApiResponse<List<EmployeeEntity>>.MyResponseApi(
              data: employees, totalCount: totalCount, message: "success");
        return totalCount > 0 ? TypedResults.Ok(data) 
                                : TypedResults.BadRequest("Данные не найдены!");
    }
    // Функция возвращает сущность из бд по id
    private static async Task<IResult> GetEmployeeAsync(Guid id, IEmployeeServices employeeServices)
    {
       var find = await employeeServices.GetEmployeeAsync(id);
       return find is not null ? TypedResults.Ok(find) 
                               : TypedResults.BadRequest("Данные не найдены!");
    }
    
    // Функция создаёт сущность в бд и возвращает её как результат
    private static async Task<IResult> AddEmployeeAsync(EmployeeDto entity, IEmployeeServices employeeServices)
    {
       List<ResponseErrors> validateResult = await employeeServices.ValidateEmployee(entity);
       if (validateResult.Count > 0) return Results.BadRequest(validateResult);
        // Если при проверке входных данных ошибок нет, то создаём новую сущность в бд 
        var create = await employeeServices.CreateEmployee(entity);
       // Возвращаем ответ с созданной сущностью и status 200
       return TypedResults.Ok(ApiResponse<EmployeeEntity>.MyResponseApi(data: create, message: "Успешно создан!") );
    }
    
    // Функция обновляет сущность в бд и возвращает её как результат
    private static async Task<IResult> UpdateEmployeeAsync(Guid id, EmployeeDto entity, IEmployeeServices employeeServices )
    {
        // Проверяем входные данные от пользователя на обновления полей
        List<ResponseErrors> validateResult = await employeeServices.ValidateEmployee(entity);
        if (validateResult.Count > 0) return Results.BadRequest(validateResult);
        // После успешной проверке обновляем сущность в бд
      var update = await employeeServices.UpdateEmployee(id, entity);
      return update is not null ? 
              TypedResults.Ok(ApiResponse<EmployeeEntity>.MyResponseApi(data: update, message: "Данные успешно обновлены!")) 
              : TypedResults.BadRequest("Пользователь не найден!");   
    }
    
    // Функция удаляет сущность из бд и возвращает строку как результат успешного удаления
    private static async Task<IResult> DeleteEmployeeAsync(Guid id, IEmployeeServices employeeServices) =>
        TypedResults.Ok(ApiResponse<EmployeeEntity>.MyResponseApi(data: null, message: await employeeServices.DeleteEmployee(id)));
     
    // Функция удаляет все таблицы сущности из бд и возвращает строку как результат успешного удаления
    private static async Task<IResult> DeleteAllEmployeeAsync(IEmployeeServices employeeServices) =>
        TypedResults.Ok(ApiResponse<string>.MyResponseApi(data:null, message: await employeeServices.DeleteAllEmployees()));
}