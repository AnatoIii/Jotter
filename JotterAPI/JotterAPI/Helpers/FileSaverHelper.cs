using JotterAPI.Helpers.Abstractions;
using JotterAPI.Model;
using System;
using System.IO;

namespace JotterAPI.Helpers
{
	public class FileSaverHelper : IFileWorker
	{
		public readonly string _defaultPath;

		public FileSaverHelper()
		{
			_defaultPath = "D:/JotterData";
			if (!Directory.Exists(_defaultPath)) {
				Directory.CreateDirectory(_defaultPath);
			}
		}

		public string SaveFile(FileSaveData fileData)
		{
			var fileName = GenerateFileName(fileData.FileName);
			var path = $"{_defaultPath}/{fileName}";

			using (var streamWriter = new StreamWriter(path)) {
				streamWriter.Write(fileData.Base64File);
			}

			return path;
		}

		public string LoadFile(string path)
		{
			if (!File.Exists(path)) {
				return string.Empty;
			}

			using (var streamReader = new StreamReader(path)) {
				return streamReader.ReadToEnd();
			}
		}

		public void DeleteFile(string path)
		{
			if (File.Exists(path)) {
				File.Delete(path);
			}
		}

		private string GenerateFileName(string extension)
		{
			var date = DateTime.Now;

			var dateString = $"{date.Year}_{date.Month}_{date.Day}_{date.Hour}_{date.Minute}_{date.Millisecond}";

			var dotIndex = extension.LastIndexOf('.');
			var extensionString = dotIndex == -1 ? "uf" : extension.Substring(dotIndex);

			return $"{Guid.NewGuid()}_{dateString}{extensionString}";
		}
	}
}
