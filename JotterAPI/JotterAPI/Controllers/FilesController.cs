using JotterAPI.Model.DTOs.Files;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
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
		public Task<Response<FileResult>> AddFile([FromBody] FileToSaveData fileToSave)
		{
			return _fileService.AddFile(fileToSave, GetUserId());
		}

		[HttpGet("{fileId}")]
		public async Task<Response<FileDataResult>> GetFileById(Guid fileId)
		{
			return await _fileService.GetFileById(fileId, GetUserId());
		}

		[HttpDelete("{fileId}")]
		public Task<Response<ResponseResult>> DeleteFile(Guid fileId)
		{
			return _fileService.DeleteFile(fileId, GetUserId());
		}

		private Guid GetUserId()
		{
			return Guid.Parse(User.Identity.Name);
		}
	}
}
