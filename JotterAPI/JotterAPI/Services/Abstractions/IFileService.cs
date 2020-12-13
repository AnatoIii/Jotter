using JotterAPI.Model.DTOs.Files;
using JotterAPI.Model.Reponses;
using System.Threading.Tasks;

namespace JotterAPI.Services.Abstractions
{
	public interface IFileService
	{
		Task<Response<FileResult>> AddFile(FileToSaveData fileToSave);
		Task<Response<FileDataResult>> GetFileById(FileIds fileIds);
		Task<Response<ResponseResult>> DeleteFile(FileIds fileIds);
	}
}
