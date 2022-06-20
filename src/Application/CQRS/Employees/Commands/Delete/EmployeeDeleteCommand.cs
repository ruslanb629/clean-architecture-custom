namespace Application.CQRS.Employees.Commands.Delete;

public class EmployeeDeleteCommand : IRequest<BaseResponseVm>
{
    public Guid Id { get; set; }
}
