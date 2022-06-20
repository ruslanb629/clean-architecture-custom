using Application.Common.DTOs.Departments;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Common;
using Domain.Entities;

namespace Application.Common.DTOs.Employees;

public class EmployeeDto: AuditableEntityDto
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }

    public DepartmentDto Department { get; set; }
}
