using Application.Common.Messages;

namespace Application.CQRS.Employees.Commands.Delete;

public class EmployeeDeleteCommandValidator : AbstractValidator<EmployeeDeleteCommand>
{
    public EmployeeDeleteCommandValidator()
    {
        RuleFor(x => x.Id)
            .Must(id => id != Guid.Empty)
            .WithMessage(ValidationMessage.EnterId);
    }
}
