using JotterAPI.Model;

namespace JotterAPI.Helpers.Abstractions
{
	public interface IFileWorker
	{
		string SaveFile(FileSaveData fileData);
		string LoadFile(string path);
		void DeleteFile(string path);
	}
}
