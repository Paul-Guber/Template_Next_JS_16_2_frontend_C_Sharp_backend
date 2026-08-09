using FluentValidation;
using Start_Template_CSharp.Core.Dto;

namespace Start_Template_CSharp.Application.Validators;

public sealed class EmployeeDtoValidator : AbstractValidator<EmployeeDto>
{
    private const int MinLength = 3;
    private const int MaxLength = 50;
    public EmployeeDtoValidator()
    {
        RuleFor(x=>x.Name)
            .NotEmpty()
            .Matches(x => StringHelper.RemoveSpecialCharacters(x.Name))
            .WithMessage("Обязательно для заполнения!");

        RuleFor(x => x.Name).Length(MinLength, MaxLength)
            .WithMessage($"Поле должно быть не менее {MinLength} и не более {MaxLength} символов");

        RuleFor(x => x.Email)
            .NotEmpty()
            .Matches(x => StringHelper.RemoveSpecCharEmail(x.Email))
            .WithMessage("Обязательно для заполнения!");

        RuleFor(x => x.Email)
            .EmailAddress()
            .WithMessage("Неверный формат email адреса!")
            .WithName("Email адрес");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .NotNull()
            .WithMessage("Телефонный номер обязателен");

        RuleFor(x=>x.Phone)
            .Matches( StringHelper.ValidationPhone())
            .WithMessage("Неверный формат номера телефона");
    }
}