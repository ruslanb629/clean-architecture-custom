using Application.Common.Interfaces;
using AutoMapper;
using Domain.Entities;

namespace Application.CQRS.Employees.Commands.Create;

public class EmployeeCreateCommandHandler : IRequestHandler<EmployeeCreateCommand, BaseResponseVm>
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IMapper _mapper;

    public EmployeeCreateCommandHandler(IApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }

    public async Task<BaseResponseVm> Handle(EmployeeCreateCommand request, 
        CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Employee>(request);

        await _dbContext.Employees.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new BaseResponseVm();
    }
}
