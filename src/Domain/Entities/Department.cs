namespace Domain.Entities;

public class Department: AuditableEntity
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public ICollection<Employee> Employees { get; set; }
}