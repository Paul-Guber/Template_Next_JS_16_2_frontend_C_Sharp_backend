using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;
using Start_Template_CSharp.Application.Interfaces;
using Start_Template_CSharp.Core.Dto;
using Start_Template_CSharp.Core.Entities;
using Start_Template_CSharp.Core.models;

namespace Start_Template_CSharp.Infrastructure.Services;

public sealed class EmployeeServices(IRepository<EmployeeEntity> repository, IValidator<EmployeeDto> validator) : IEmployeeServices
{
    public async Task<(List<EmployeeEntity> employees, int totalCount)> GetAllEmployees(int currentPage, int pageSize, string? searchQuery)
    {
        string query = string.IsNullOrWhiteSpace(searchQuery) ? string.Empty : searchQuery;
        var result = await repository.SearchByFilterAsync(filter =>
            filter.Where(employee =>
                employee.Name.Contains(query.ToLower() ) ||
                employee.Email.Contains(query.ToLower())));
        var totalCount = (int)Math.Ceiling((double)result.Count / pageSize);
        currentPage = currentPage >  totalCount ? totalCount : currentPage;
        return (employees: result.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList(), totalCount: result.Count);
    }

    public async Task<List<ResponseErrors>> ValidateEmployee(EmployeeDto employeeDto)
    {
        // Создаём новый экземпляр Dto и убираем всё кроме цифр и знака +
        EmployeeDto newDto = employeeDto with { Phone = Regex.Replace(employeeDto.Phone, @"[^0-9+]", "") };
      var validateResult = await validator.ValidateAsync(newDto);
      List<ResponseErrors> err = [];
      if (!validateResult.IsValid)
        {
            err.AddRange(validateResult.Errors.Select(error =>
                new ResponseErrors { 
                    PropertyName = error.PropertyName,
                    ErrorMessage = error.ErrorMessage,
                }));
        }
        return err;
    }
    
    public async Task<EmployeeEntity> CreateEmployee(EmployeeDto employee)
    {
       
        var newEntity = new EmployeeEntity()
        {
            Name = employee.Name,
            Email = employee.Email,
            Phone = Regex.Replace(employee.Phone, @"[^0-9+]", "")  
        };
        // Создаём сущность в БД
       return await repository.CreateAsync(newEntity);
    }

    public async Task<EmployeeEntity?> GetEmployeeAsync(Guid id) =>
        await repository.GetAsync(x=>x.Id == id);

    public async Task<string> DeleteEmployee(Guid id) =>
        await repository.DeleteAsync(x => x.Id == id) ? "Данные успешно удалены!" : "Ошибка при удалении!";
     
    public async Task<EmployeeEntity?> UpdateEmployee(Guid id, EmployeeDto employeeDto)
    {
        EmployeeEntity? findEmployee = await GetEmployeeAsync(id);
        if (findEmployee is null) return null;
        findEmployee.Name = employeeDto.Name;  
        findEmployee.Email = employeeDto.Email;
        findEmployee.Phone = employeeDto.Phone;
        // Обновляем сущность в бд
       return await repository.UpdateAsync(id, findEmployee);
    }

    public async Task<string> DeleteAllEmployees()
    {
          await repository.DeleteAllAsync();
          return "Все данные успешно удалены!";
    }
    
}