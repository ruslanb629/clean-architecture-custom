using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.CQRS.Employees.Commands.Update;

public class EmployeeUpdateCommand : IRequest<BaseResponseVm>, IMapFrom<Employee>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }
    public int DepartmentId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<EmployeeUpdateCommand, Employee>();
    }
}
