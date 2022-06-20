using Application.Common.DTOs.Employees;
using Application.Common.Models;
using Application.CQRS.Employees.Commands.Create;
using Application.CQRS.Employees.Commands.Delete;
using Application.CQRS.Employees.Commands.Update;
using Application.CQRS.Employees.Queries.GetItem;
using Application.CQRS.Employees.Queries.GetItems;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class EmployeeController : ApiControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<BaseDataResponseVm<EmployeeDto>>> GetItem(Guid id)
    {
        return await Mediator.Send(new EmployeeGetItemQuery(id));
    }

    [HttpGet]
    public async Task<ActionResult<BaseDataResponseVm<EmployeeGetItemsQueryVm>>> GetItems([FromQuery]EmployeeGetItemsQuery query)
    {
        return await Mediator.Send(query);
    }

    [HttpPost]
    public async Task<ActionResult<BaseResponseVm>> Create(EmployeeCreateCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpPut]
    public async Task<ActionResult<BaseResponseVm>> Update(EmployeeUpdateCommand command)
    {
        return await Mediator.Send(command);
    }

    [HttpDelete]
    public async Task<ActionResult<BaseResponseVm>> Delete(EmployeeDeleteCommand command)
    {
        return await Mediator.Send(command);
    }
}

