namespace Application.Common.DTOs;

public abstract class AuditableEntityDto
{
    public DateTime? Created { get; set; }

    public Guid? CreatedBy { get; set; }
}
