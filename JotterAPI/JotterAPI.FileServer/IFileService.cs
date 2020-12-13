using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace JotterAPI.FileServer
{
	public interface IFileService
	{
		string AddFile(string fileToSave, string relativePath);
		string GetFileById(string path);
		void DeleteFile(string path);
	}
}
