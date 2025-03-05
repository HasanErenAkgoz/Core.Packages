using Core.Packages.Application.Features.Translate.Commands.Create;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace Core.Packages.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TranslateController : BaseApiController
    {

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTranslateCommand createTranslateCommand)
        {
            var result = await Mediator.Send(createTranslateCommand);
            return result.Success ? Ok(result) : BadRequest(result);
        }

    }
}
