using Core.Packages.Application.Features.Roles.Commands.Create;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace Core.Packages.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseApiController
    {


        [HttpPost("Role")]
        public async Task<IActionResult> Create([FromBody] CreateRoleCommand createRoleCommand)
        {
            var result = await Mediator.Send(createRoleCommand);
            return GetResponse(result);
        }
    }
}
