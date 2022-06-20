using Application.Common.Messages;

namespace Application.CQRS.Departments.Commands.Create;

public class DepartmentCreateCommandValidator: AbstractValidator<DepartmentCreateCommand>
{
    public DepartmentCreateCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(ValidationMessage.EnterName)
            .MaximumLength(100)
            .WithMessage(ValidationMessage.EnterNameLessThan100Symbol);
    }
}
