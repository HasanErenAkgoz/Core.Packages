using Core.Packages.Application.Features.Files.Commands.Delete;
using Core.Packages.Application.Features.Files.Commands.Upload;
using Core.Packages.Application.Features.Files.Queries.Download;
using Core.Packages.Application.Features.Files.Queries.Get;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Controllers;

namespace Core.Packages.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : BaseApiController
    {
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadCommand fileUploadCommand)
        {
            var result = await Mediator.Send(fileUploadCommand);
            return Ok(new { Path = result });
        }

        [HttpDelete("delete")]

        public async Task<IActionResult> DeleteFile([FromQuery] string fileName, [FromQuery] string containerName)
        {
            var command = new FileDeleteCommand(fileName, containerName);
            var result = await Mediator.Send(command);
            return result ? Ok("Dosya silindi.") : NotFound("Dosya bulunamadı.");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetFileInfo(int id)
        {
            var query = new FileGetQuery(id);
            var file = await Mediator.Send(query);
            return file != null ? Ok(file) : NotFound("Dosya bulunamadı.");
        }

        [HttpGet("download")]
        public async Task<IActionResult> DownloadFile([FromQuery] string fileName, [FromQuery] string containerName)
        {
            var query = new FileDownloadQuery(fileName, containerName);
            var fileStream = await Mediator.Send(query);
            return fileStream != null ? File(fileStream, "application/octet-stream", fileName) : NotFound("Dosya bulunamadı.");
        }
    }
}
