namespace Domain.Entities;

public class Employee: AuditableEntity
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public DateTime BirthDate { get; set; }
    public bool IsActive { get; set; }

    public int DepartmentId { get; set; }
    public Department Department { get; set; }
}