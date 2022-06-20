using Application.Common.DTOs.Departments;
using Application.Common.DTOs.Employees;
using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Employees.Queries.GetItems;

public class EmployeeGetItemsQueryHandler : IRequestHandler<EmployeeGetItemsQuery, BaseDataResponseVm<EmployeeGetItemsQueryVm>>
{
    private readonly IApplicationDbContext _dbContext;

    public EmployeeGetItemsQueryHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BaseDataResponseVm<EmployeeGetItemsQueryVm>> Handle(EmployeeGetItemsQuery request,
        CancellationToken cancellationToken)
    {
        var result = new BaseDataResponseVm<EmployeeGetItemsQueryVm>
        {
            Data = new EmployeeGetItemsQueryVm()
        };

        var query = from emp in _dbContext.Employees
                    join dept in _dbContext.Departments on emp.DepartmentId equals dept.Id
                    where
                       (string.IsNullOrEmpty(request.Name) || emp.Name == request.Name)
                       && (!request.DepartmentId.HasValue || emp.DepartmentId == request.DepartmentId.Value)
                    select new EmployeeDto
                    {
                        Id = emp.Id,
                        Name = emp.Name,
                        Surname = emp.Surname,
                        BirthDate = emp.BirthDate.Date,
                        IsActive = emp.IsActive,
                        Department = new DepartmentDto
                        {
                            Id = dept.Id,
                            Name = dept.Name
                        },
                        Created = emp.Created,
                        CreatedBy = emp.CreatedBy
                    };

        var count = await query.Select(x => x.Id).CountAsync();
        result.Data.Items = await query
            .OrderByDescending(x => x.Created)
            .Skip(request.Skip())
            .Take(request.Take())
            .ToListAsync();

        result.Data.Page = new(count, request.PageNumber, request.PageSize);

        return result;
    }
}
