using Application.Common.DTOs.Employees;

namespace Application.CQRS.Employees.Queries.GetItems;

public class EmployeeGetItemsQueryVm
{
    public List<EmployeeDto> Items { get; set; }
    public PaginatedDto Page { get; set; }
}
