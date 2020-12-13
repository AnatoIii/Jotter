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

		[HttpPost]
		public JsonResult AddFile([FromBody] SaveFileRequest request)
		{
			return new JsonResult(_fileService.AddFile(request.File, request.RelativePath));
		}

		[HttpGet]
		public JsonResult GetFileById([FromBody] string path)
		{
			return new JsonResult(_fileService.GetFileById(path));
		}

		[HttpDelete]
		public IActionResult DeleteFile([FromBody] string path)
		{
			_fileService.DeleteFile(path);
			return Ok();
		}
	}
}
