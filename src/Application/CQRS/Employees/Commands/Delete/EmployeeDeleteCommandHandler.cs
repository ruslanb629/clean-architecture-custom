using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Messages;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Employees.Commands.Delete;

public class EmployeeDeleteCommandHandler : IRequestHandler<EmployeeDeleteCommand, BaseResponseVm>
{
    private readonly IApplicationDbContext _dbContext;

    public EmployeeDeleteCommandHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<BaseResponseVm> Handle(EmployeeDeleteCommand request, 
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null) 
            throw new DataNotFoundException(ExceptionMessage.DataNotFound);

        _dbContext.Employees.Remove(entity);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new BaseResponseVm();
    }
}
