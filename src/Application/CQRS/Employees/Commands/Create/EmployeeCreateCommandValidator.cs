using Application.Common.Messages;

namespace Application.CQRS.Employees.Commands.Create;

public class EmployeeCreateCommandValidator: AbstractValidator<EmployeeCreateCommand>
{
    public EmployeeCreateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ValidationMessage.EnterName)
            .MaximumLength(100)
            .WithMessage(ValidationMessage.EnterNameLessThan100Symbol);

        RuleFor(x => x.Surname)
            .NotEmpty()
            .WithMessage(ValidationMessage.EnterSurname)
            .MaximumLength(100)
            .WithMessage(ValidationMessage.EnterSurnameLessThan100Symbol);

        RuleFor(x => x.BirthDate)
            .LessThanOrEqualTo(DateTime.Now.Date)
            .WithMessage(ValidationMessage.SelectCorrectBirthDate)
            .GreaterThanOrEqualTo(DateTime.Now.Date.AddYears(-122))
            .WithMessage(ValidationMessage.SelectCorrectBirthDate);

        RuleFor(x => x.DepartmentId)
            .Must(departmentId => departmentId > 0)
            .WithMessage(ValidationMessage.SelectDepartment);
    }
}
