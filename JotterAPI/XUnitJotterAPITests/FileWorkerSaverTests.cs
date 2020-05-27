using JotterAPI.Helpers;
using JotterAPI.Model;
using JotterAPI.Model.Reponses;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace XUnitJotterAPITests
{
	public class FileWorkerSaverTests
	{
		private string _basePath;
		private string _fileContent = "It's the best image you have ever seen";

		public FileWorkerSaverTests()
		{
			_basePath = "D:/JotterData/";
			if (!Directory.Exists(_basePath)) {
				Directory.CreateDirectory(_basePath);
			}
		}

		[Fact]
		public void SaveFile_When_CorrectFile_Then_GetNewFile()
		{
			var fileSaverHelper = new FileSaverHelper();

			var fileData = new FileSaveData {
				Base64File = Convert.ToBase64String(Encoding.ASCII.GetBytes(_fileContent)),
				FileName = "123.data"
			};

			var filePath = fileSaverHelper.SaveFile(fileData);

			Assert.True(File.Exists(filePath));
		}

		[Fact]
		public void ReadFile_When_CorrectFile_Then_FileDataTheSame()
		{
			var fileSaverHelper = new FileSaverHelper();

			var fileData = new FileSaveData {
				Base64File = Convert.ToBase64String(Encoding.ASCII.GetBytes(_fileContent)),
				FileName = "123.data"
			};

			var filePath = fileSaverHelper.SaveFile(fileData);
			var fileContent = fileSaverHelper.LoadFile(filePath);

			Assert.True(File.Exists(filePath));
			Assert.True(_fileContent == Encoding.ASCII.GetString(Convert.FromBase64String(fileContent)));
		}

		[Fact]
		public void ReadFile_When_InCorrectFile_Then_EmptyString()
		{
			var fileSaverHelper = new FileSaverHelper();

			var fileName = Guid.NewGuid().ToString("N");
			var filePath = $"{_basePath}{fileName}";

			var fileContent = fileSaverHelper.LoadFile(filePath);

			Assert.False(File.Exists(filePath));
			Assert.True(string.Empty == fileContent);
		}

		[Fact]
		public void DeleteFile_When_FileExist_Then_FileDeleted()
		{
			var fileSaverHelper = new FileSaverHelper();

			var fileName = Guid.NewGuid().ToString("N");
			var filePath = $"{_basePath}{fileName}";

			File.Create(filePath).Close();

			fileSaverHelper.DeleteFile(filePath);

			Assert.False(File.Exists(filePath));
		}

		[Fact]
		public void DeleteFile_When_FileNotExist_Then_NoError()
		{
			var fileSaverHelper = new FileSaverHelper();

			var fileName = Guid.NewGuid().ToString("N");
			var filePath = $"{_basePath}{fileName}";

			Assert.False(File.Exists(filePath));

			fileSaverHelper.DeleteFile(filePath);

			Assert.False(File.Exists(filePath));
		}
	}
}
