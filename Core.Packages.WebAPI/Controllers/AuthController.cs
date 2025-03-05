using Core.Packages.Application.Features.Auth.ForgotPassword.Commands;
using Core.Packages.Application.Features.Auth.Login.Commands;
using Core.Packages.Application.Features.Auth.Register.Commands;
using Core.Packages.Application.Features.Auth.ResetPassword;
using Core.Packages.Application.Features.Email.SendEmail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace Core.Packages.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginCommand loginCommand)
        {
            var result = await Mediator.Send(loginCommand);
            return GetResponse(result);
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterCommand registerCommand)
        {
            var result = await Mediator.Send(registerCommand);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPost("sendemail")]
        public async Task<IActionResult> SendEmail([FromBody] SendEmailCommand sendEmailCommand)
        {
            var result = await Mediator.Send(sendEmailCommand);
            return result.Success ? Ok(result) : BadRequest(result);

        }

        [HttpPost("forgot-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordCommand command)
        {
            var result = await Mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordCommand command)
        {
            var result = await Mediator.Send(command);
            return result.Success ? Ok(result.Message) : BadRequest(result.Message);

        }
    }
}
