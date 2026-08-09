
namespace Start_Template_CSharp.Core.Entities;

public sealed class EmployeeEntity
{
    public Guid Id { get; init; }
    public required string Name { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }

}