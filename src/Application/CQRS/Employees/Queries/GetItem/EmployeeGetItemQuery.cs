using Application.Common.DTOs.Employees;

namespace Application.CQRS.Employees.Queries.GetItem;

public class EmployeeGetItemQuery : IRequest<BaseDataResponseVm<EmployeeDto>>
{
    public EmployeeGetItemQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
