using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Common.Messages;
using AutoMapper;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.CQRS.Employees.Commands.Update;

public class EmployeeUpdateCommandHandler : IRequestHandler<EmployeeUpdateCommand, BaseResponseVm>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public EmployeeUpdateCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<BaseResponseVm> Handle(EmployeeUpdateCommand request, 
        CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Employees.FirstOrDefaultAsync(x => x.Id == request.Id);

        if (entity == null) 
            throw new DataNotFoundException(ExceptionMessage.DataNotFound);

        _mapper.Map(request, entity);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new BaseResponseVm();
    }
}
