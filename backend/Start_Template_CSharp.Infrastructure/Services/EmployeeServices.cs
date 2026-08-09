using System.Text.RegularExpressions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Start_Template_CSharp.Application.Interfaces;
using Start_Template_CSharp.Core.Dto;
using Start_Template_CSharp.Core.Entities;
using Start_Template_CSharp.Core.models;

namespace Start_Template_CSharp.Infrastructure.Services;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class EmployeeServices(
    IRepository<EmployeeEntity> repository,
    IValidator<EmployeeDto> validator,
    ILogger<EmployeeServices> logger) : IEmployeeServices
{
    private const string ErrorNameService = "Ошибка в сервисе {EmployeeServices}, " ;

    public async Task<(IList<EmployeeEntity> employees, int totalCount)> GetAllEmployees(int currentPage, int pageSize,
        string? searchQuery )
    {
        string query = string.IsNullOrWhiteSpace(searchQuery) ? string.Empty : searchQuery.ToUpperInvariant();
         IList<EmployeeEntity> result = await repository.SearchByFilterAsync(filter =>
            filter.Where(employee =>
                employee.Name.Contains(query) || employee.Email.Contains(query))).ConfigureAwait(false);
        int totalCount = (int)Math.Ceiling((double)result.Count / pageSize);
        currentPage = currentPage >  totalCount ? totalCount : currentPage;
        return (employees: result.Skip((currentPage - 1) * pageSize).Take(pageSize).ToList(), totalCount: result.Count);
    }

    public async Task<IList<ResponseErrors>> ValidateEmployee(EmployeeDto employeeDto,  CancellationToken  cancellation = default)
    {
        // Создаём новый экземпляр Dto и убираем всё кроме цифр и знака +
        EmployeeDto newDto = employeeDto with { Phone = Regex.Replace(employeeDto.Phone, "[^0-9+]", "",
                                                  RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture,
                                                  TimeSpan.FromSeconds(1)) };
      ValidationResult validateResult = await validator.ValidateAsync(newDto, cancellation).ConfigureAwait(false);
      List<ResponseErrors> err = [];
      if (!validateResult.IsValid)
      {

          err.AddRange(validateResult.Errors.Select(error =>
          {
              string propertyName = error.PropertyName;
              logger.LogWarning
                  (ErrorNameService
                   + "метода {ValidateEmployee} {PropertyName} = {ErrorMessage}", nameof(EmployeeServices), nameof(ValidateEmployee), propertyName, error.ErrorMessage);

              return new ResponseErrors
                     {
                         PropertyName = error.PropertyName,
                         ErrorMessage = error.ErrorMessage,
                     };
          }));
      }
        return err;
    }

    public Task<EmployeeEntity> CreateEmployee(EmployeeDto employee)
    {

        var newEntity = new EmployeeEntity()
        {
            Name = employee.Name,
            Email = employee.Email,
            Phone = Regex.Replace(employee.Phone, @"[^0-9+]", "",
                RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture,
                TimeSpan.FromSeconds(1))
        };

        // Создаём сущность в БД
       return repository.CreateAsync(newEntity);
    }

    public async Task<EmployeeEntity?> GetEmployeeAsync(Guid id)
    {
      EmployeeEntity? find = await repository.GetAsync(x => x.Id == id).ConfigureAwait(false);
      if (find is null)
      {
          logger.LogError(ErrorNameService +
                   "в методе {Get}. Сотрудник не найден! Ошибка . {Find}"
                   ,nameof(EmployeeServices), nameof(GetEmployeeAsync),find);
      }
      return find;
    }


    public async Task<string> DeleteEmployee(Guid id) =>
        await repository.DeleteAsync(x => x.Id == id).ConfigureAwait(false) ? "Сотрудник успешно удалён!" : "Ошибка при удалении!";

    public async Task<EmployeeEntity?> UpdateEmployee(Guid id,EmployeeDto employeeDto)
    {
        EmployeeEntity? findEmployee = await repository.GetAsync(x=>
                 x.Id == id).ConfigureAwait(false);

        if (findEmployee is null)
        {
            logger.LogError(ErrorNameService +
                            "в методе {Get}. Сотрудник не найден! {Find}"
                ,nameof(EmployeeServices), nameof(UpdateEmployee),findEmployee);
            return null;
        }
        findEmployee.Name = employeeDto.Name;
        findEmployee.Email = employeeDto.Email;
        findEmployee.Phone = employeeDto.Phone;
        try
        {
            // Обновляем сущность в бд
            return await repository.UpdateAsync(findEmployee).ConfigureAwait(true);
        }
        catch (DbUpdateException ex)
        {
            logger.LogError(ex, ErrorNameService + "в методе {Update} "
                , nameof(EmployeeServices), nameof(UpdateEmployee));
            Console.WriteLine("Catch = " + ex);
            if (ex.InnerException is SqlException { Number: 2601 } innerException)
            {
                logger.LogError(ex, "Ошибка ");
                Console.WriteLine("InnerException Message = " + innerException);
            }
            return null;
        }
    }

    public async Task<string> DeleteAllEmployees()
    {
          await repository.DeleteAllAsync().ConfigureAwait(false);
          return "Все сотрудники успешно удалены!";
    }

}
