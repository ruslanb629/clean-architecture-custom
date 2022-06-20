using Application.Common.Models;
using Application.CQRS.Departments.Commands.Create;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class DepartmentController : ApiControllerBase
{
    [HttpPost]
    public async Task<ActionResult<BaseResponseVm>> Create(DepartmentCreateCommand command)
    {
        return await Mediator.Send(command);
    }
}
