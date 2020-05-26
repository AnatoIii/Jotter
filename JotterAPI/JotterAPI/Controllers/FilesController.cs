using JotterAPI.Model.DTOs.Files;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using FileResult = JotterAPI.Model.Reponses.FileResult;

namespace JotterAPI.Controllers
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
		public Task<Response<FileResult>> AddFile([FromBody]FileToSaveData fileToSave)
		{
			return _fileService.AddFile(fileToSave);
		}

		[HttpGet]
		public Response<FileDataResult> GetFileById([FromQuery]FileIds fileIds)
		{
			return _fileService.GetFileById(fileIds);
		}

		[HttpDelete]
		public Task<Response<ResponseResult>> DeleteFile([FromQuery]FileIds fileIds)
		{
			return _fileService.DeleteFile(fileIds);
		}
	}
}
