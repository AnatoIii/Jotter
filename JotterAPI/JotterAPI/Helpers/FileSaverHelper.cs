using JotterAPI.Helpers.Abstractions;
using JotterAPI.Model;
using System;

namespace JotterAPI.Helpers
{
	public class FileSaverHelper : IFileWorker
	{
		public string SaveFile(FileSaveData fileData)
		{
			throw new System.NotImplementedException();
		}

		public string LoadFile(string path)
		{
			throw new System.NotImplementedException();
		}

		public void DeleteFile(string path)
		{
			throw new System.NotImplementedException();
		}

		private string GenerateFileName(string extension)
		{
			var date = DateTime.Now;

			var dateString = $"{date.Year}_{date.Month}_{date.Day}_{date.Hour}_{date.Minute}_{date.Millisecond}";

			var extensionString = extension.Substring(extension.LastIndexOf('.'));

			return $"{Guid.NewGuid()}_{dateString}.{extensionString}";
		}
	}
}
