using Application.Common.Attribute;

namespace Application.CQRS.Departments.Commands.Create;

public class DepartmentCreateCommand: IRequest<BaseResponseVm>
{
    public string? Name { get; set; }

    //[IgnoreLog]
    //public string? Description { get; set; }
}

