using Application.Common.Mappings;
using AutoMapper;
using Domain.Entities;

namespace Application.CQRS.Employees.Commands.Create;

public class EmployeeCreateCommand : IRequest<BaseResponseVm>, IMapFrom<Employee>
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }
    public int DepartmentId { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<EmployeeCreateCommand, Employee>();
    }
}
