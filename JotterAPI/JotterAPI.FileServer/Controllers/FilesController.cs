using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JotterAPI.FileServer.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class FilesController : ControllerBase
	{
		private readonly IFileService _fileService;

		public FilesController(IFileService fileService)
		{
			_fileService = fileService;
		}

		[HttpGet("ping")]
		public string Ping()
        {
			return "pong";
        }

		[HttpPost]
		public IActionResult AddFile([FromBody] SaveFileRequest request)
		{
			return Ok(_fileService.AddFile(request.File, request.RelativePath));
		}

		[HttpGet]
		public IActionResult GetFileById([FromQuery] string path)
		{
			return Ok(_fileService.GetFileById(path));
		}

		[HttpDelete]
		public IActionResult DeleteFile([FromQuery] string path)
		{
			_fileService.DeleteFile(path);
			return Ok();
		}
	}
}
