using Application.Common.Interfaces;
using Domain.Entities;

namespace Application.CQRS.Departments.Commands.Create;

public class DepartmentCreateCommandHandler : IRequestHandler<DepartmentCreateCommand, BaseResponseVm>
{
    private readonly IApplicationDbContext _dbContext;

    public DepartmentCreateCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BaseResponseVm> Handle(DepartmentCreateCommand request, 
        CancellationToken cancellationToken)
    {
        var entity = new Department
        {
            Name = request.Name
        };

        await _dbContext.Departments.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new BaseResponseVm();
    }
}
