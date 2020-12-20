using JotterAPI.Model.DTOs.Files;
using JotterAPI.Model.Reponses;
using System;
using System.Threading.Tasks;

namespace JotterAPI.Services.Abstractions
{
	public interface IFileService
	{
		Task<Response<FileResult>> AddFile(FileToSaveData fileToSave, Guid userId);
		Task<Response<FileDataResult>> GetFileById(Guid fileId, Guid userId);
		Task<Response<ResponseResult>> DeleteFile(Guid fileId, Guid userId);
	}
}
