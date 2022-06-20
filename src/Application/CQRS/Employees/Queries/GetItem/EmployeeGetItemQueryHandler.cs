using Application.Common.DTOs.Departments;
using Application.Common.DTOs.Employees;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Messages;

namespace Application.CQRS.Employees.Queries.GetItem;

public class EmployeeGetItemQueryHandler : IRequestHandler<EmployeeGetItemQuery, BaseDataResponseVm<EmployeeDto>>
{
    private readonly ICachedData _cachedData;

    public EmployeeGetItemQueryHandler(ICachedData cachedData)
    {
        _cachedData = cachedData;
    }

    public async Task<BaseDataResponseVm<EmployeeDto>> Handle(EmployeeGetItemQuery request,
        CancellationToken cancellationToken)
    {
        var result = new BaseDataResponseVm<EmployeeDto>
        {
            Data = new EmployeeDto()
        };

        result.Data = (from emp in (await _cachedData.Employees())
                       join dept in (await _cachedData.Departments()) on emp.DepartmentId equals dept.Id
                       where emp.Id == request.Id
                       select new EmployeeDto
                       {
                           Id = emp.Id,
                           Name = emp.Name,
                           Surname = emp.Surname,
                           BirthDate = emp.BirthDate,
                           IsActive = emp.IsActive,
                           Department = new DepartmentDto
                           {
                               Id = dept.Id,
                               Name = dept.Name
                           }
                       }).FirstOrDefault();

        if (result.Data == null)
            throw new DataNotFoundException(ExceptionMessage.DataNotFound);

        return result;
    }
}
