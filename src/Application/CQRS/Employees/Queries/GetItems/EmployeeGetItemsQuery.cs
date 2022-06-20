namespace Application.CQRS.Employees.Queries.GetItems;

public class EmployeeGetItemsQuery : PaginationQuery, IRequest<BaseDataResponseVm<EmployeeGetItemsQueryVm>>
{
    public string? Name { get; set; }
    public int? DepartmentId { get; set; }
}
