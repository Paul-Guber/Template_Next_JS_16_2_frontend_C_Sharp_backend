using Start_Template_CSharp.Core.Dto;
using Start_Template_CSharp.Core.Entities;
using Start_Template_CSharp.Core.models;

namespace Start_Template_CSharp.Application.Interfaces;

public interface IEmployeeServices
{
    Task<(IList<EmployeeEntity> employees, int totalCount)> GetAllEmployees(int currentPage, int pageSize, string? searchQuery);
    Task<IList<ResponseErrors>> ValidateEmployee(EmployeeDto employeeDto, CancellationToken  cancellation = default);
    Task<EmployeeEntity> CreateEmployee(EmployeeDto employee);
    Task<EmployeeEntity?> GetEmployeeAsync(Guid id);
    Task<string> DeleteEmployee(Guid id);
    Task<string> DeleteAllEmployees();
    Task<EmployeeEntity?> UpdateEmployee(Guid id, EmployeeDto employeeDto);
}
