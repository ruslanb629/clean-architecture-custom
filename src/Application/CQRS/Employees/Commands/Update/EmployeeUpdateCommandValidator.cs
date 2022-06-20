using Application.Common.Messages;

namespace Application.CQRS.Employees.Commands.Update;

public class EmployeeUpdateCommandValidator: AbstractValidator<EmployeeUpdateCommand>
{
    public EmployeeUpdateCommandValidator()
    {
        RuleFor(x => x.Id)
            .Must(id => id != Guid.Empty)
            .WithMessage(ValidationMessage.EnterId);

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
