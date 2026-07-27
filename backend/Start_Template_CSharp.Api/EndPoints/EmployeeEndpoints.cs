using System.Text.RegularExpressions;
using FluentValidation;
using Start_Template_CSharp.Api.EndPoints.Extensions;
using Start_Template_CSharp.Core.Dto;
using Start_Template_CSharp.Core.Entities;
using Start_Template_CSharp.Core.Interfaces;
using Start_Template_CSharp.Core.models;
using Start_Template_CSharp.Core.Responses;


namespace Start_Template_CSharp.Api.EndPoints;

// Endpoints для сущности EmployeeEntity
// ReSharper disable once ClassNeverInstantiated.Global
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
        IRepository<EmployeeEntity> repository)
    {
        // Текущая страница с frontend next js
        int currentPage = page ?? 1;
        
        // Количество элементов на странице, frontend next js
        int pageSize = limit ?? 2;
        
         string query = string.IsNullOrWhiteSpace(searchQuery) ? string.Empty : searchQuery;
          
        var result = (await repository.GetAllAsync (q =>
            q.Name.Contains(query.ToLower(), StringComparison.CurrentCultureIgnoreCase)
            || q.Email.Contains(query.ToLower(), StringComparison.CurrentCultureIgnoreCase),
            r => r.Name
            )).ToList();
        
        var totalCount = (int)Math.Ceiling((double)result.Count / pageSize);
        currentPage = currentPage >  totalCount ? totalCount : currentPage;
       
             var pageResult = result.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList();
             
            var data = ApiResponse<List<EmployeeEntity>>.MyResponseApi(
              data: pageResult, totalCount: result.Count, message: "success");
        return result.Count > 0 ? TypedResults.Ok(data) 
                                : TypedResults.BadRequest("Данные не найдены!");
    }
    // Функция возвращает сущность из бд по id
    private static async Task<IResult> GetEmployeeAsync(Guid id, IRepository<EmployeeEntity> repository)
    {
       var find = await repository.GetByIdAsync(id);
       return find.Data is not null ? TypedResults.Ok(find.Data) 
                               : TypedResults.BadRequest(find.Message);
    }
    
    // Функция создаёт сущность в бд и возвращает её как результат
    private static async Task<IResult> AddEmployeeAsync(
        IRepository<EmployeeEntity> repository, EmployeeDto entity, IValidator<EmployeeDto> validator)
    {
        // Создаём новый экземпляр Dto и убираем всё кроме цифр и знака +
        EmployeeDto newDto = entity with { Phone = Regex.Replace(entity.Phone, @"[^0-9+]", "") };
         
       var validateResult =  await validator.ValidateAsync(newDto);
        
       if (!validateResult.IsValid)
       {
           /* 
           var errors = new ValidationProblemDetails( )
           { 
            Status = StatusCodes.Status400BadRequest,
            Title = "Ошибка заполнения формы",
            Detail = validateResult.Errors.First().ErrorMessage,
            Instance = "api/employee/add"
           };
            return Results.Problem(errors);*/
           
            return Results.BadRequest(validateResult.Errors); 
       }
       
       // После проверки входных данных создаём новый экземпляр сущности
       var newEntity = new EmployeeEntity()
       {
           Name = newDto.Name,
           Email = newDto.Email,
           Phone = Regex.Replace(entity.Phone, @"[^0-9+]", "")  
       };
       // Создаём сущность в БД
       var createEntity =  await repository.CreateAsync(newEntity);
       // Возвращаем ответ с созданной сущностью и status 200
       return TypedResults.Ok(createEntity);
    }
    
    // Функция обновляет сущность в бд и возвращает её как результат
    private static async Task<IResult> UpdateEmployeeAsync(Guid id, EmployeeDto entity,
        IRepository<EmployeeEntity> repository, IValidator<EmployeeDto> validator)
    {
        // Ищем данные в бд по id и если не находим то возвращаем BadRequest
        var find = await repository.GetByIdAsync(id);
        if (find.Data == null) return TypedResults.BadRequest(find.Message);
        // Создаём новый экземпляр Dto и убираем всё кроме цифр и знака +
        EmployeeDto newDto = entity with { Phone = Regex.Replace(entity.Phone, @"[^0-9+]", "") };
        // Перед обновлением производим проверку с помощью библиотеки Fluent Validator
        var validateResult =  await validator.ValidateAsync(newDto);  
        // Если не верно заполнены поля возращаем ответ BadRequest c описанием ошибок
        if (!validateResult.IsValid)
        {
            List<ResponseErrors> err = [];
            err.AddRange(validateResult.Errors.Select(error =>
                new ResponseErrors { 
                    PropertyName = error.PropertyName,
                    ErrorMessage = error.ErrorMessage,
                    }));
            return Results.BadRequest(err); 
        }
        // При успешной проверке обновляем поля сущности
        find.Data.Name = newDto.Name;  
        find.Data.Email = newDto.Email;
        find.Data.Phone = newDto.Phone;
        // Обновляем сущность в бд
        var update = await repository.UpdateAsync(id, find.Data);
        // Возвращаем ответ с изменённой сущностью и status 200
        return TypedResults.Ok(ApiResponse<EmployeeEntity>.MyResponseApi(
            data: update, message: "Данные успешно обновлены!"));   
    }
    
    // Функция удаляет сущность из бд и возвращает строку как результат успешного удаления
    private static async Task<IResult> DeleteEmployeeAsync(Guid id,
                                                           IRepository<EmployeeEntity> repository)
    {
       var deleteEntity = await repository.DeleteAsync(id);


       return
           TypedResults.Ok(ApiResponse<EmployeeEntity>.MyResponseApi(data: null, message: deleteEntity.Message));

    }
    // Функция удаляет все таблицы сущности из бд и возвращает строку как результат успешного удаления
    private static async Task<IResult> DeleteAllEmployeeAsync(IRepository<EmployeeEntity> repository)
    {
       var  deleteAllEntities = await repository.DeleteAllAsync();
       return TypedResults.Ok(deleteAllEntities);
    }

    
}