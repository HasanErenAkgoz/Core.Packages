using Core.Packages.Application.Features.Permission.Queries.GetAll;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace Core.Packages.WebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : BaseApiController
    {

        [Produces("application/json", "text/plain")]
        [HttpGet("getall")]
        public async Task<IActionResult> GetAll()
        {
            var result = await Mediator.Send(new GetAllPermissionQuery());
            return GetResponse(result);
        }
    }
}
