using JotterAPI.Helpers.Abstractions;
using JotterAPI.Model;
using JotterAPI.Model.DTOs.Files;
using JotterAPI.Services;
using JotterAPI.Services.Abstractions;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace XUnitJotterAPITests
{
	public class FileServiceTests : JotterTestDbContext
	{
		private Task<T> GetCompletedTask<T>(T valueToReturn) 
        {
			var tcs = new TaskCompletionSource<T>();
			tcs.SetResult(valueToReturn);
			return tcs.Task;
		}

		[Fact]
		public async Task AddFile_When_NoteExists_Then_GetNewFileData()
		{
			var fileSaverHelperMock = new Mock<IFileServerClient>();
			fileSaverHelperMock.Setup(fileHelper => fileHelper.AddFile(It.IsAny<string>(), It.IsAny<string>())).Returns(GetCompletedTask("Good path for file"));

			var fileService = new FileService(_dbContext, fileSaverHelperMock.Object);

			var file = new FileToSaveData {
				Base64File = "BASE64STRING",
				FileName = "MyTestFile.app",
				NoteId = Guid.Parse("2E20F240-ADD6-496F-A2BD-794043D94940")
			};

			var userId = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C");

			var addFileResponse = await fileService.AddFile(file, userId);

			Assert.NotEqual(new Guid(), addFileResponse.ResponseResult.Id);
			Assert.Null(addFileResponse.Error);
			Assert.True(addFileResponse.IsSuccessful);
			fileSaverHelperMock.Verify(fileHelper => fileHelper.AddFile(It.IsAny<string>(), It.IsAny<string>()), Times.Once());
		}

		[Fact]
		public void AddFile_When_IncorrectUser_Then_Error()
		{
			var fileSaverHelperMock = new Mock<IFileServerClient>();

			var fileService = new FileService(_dbContext, fileSaverHelperMock.Object);

			var file = new FileToSaveData {
				Base64File = "BASE64STRING",
				FileName = "MyTestFile.app",
				NoteId = Guid.Parse("2E20F240-ADD6-496F-A2BD-794043D94940")
			};
			var userId = Guid.Parse("2E20F240-371D-496F-B7DD-794043D94940");

			var addFileResponse = fileService.AddFile(file, userId).Result;

			Assert.Null(addFileResponse.ResponseResult);
			Assert.NotNull(addFileResponse.Error);
			Assert.False(addFileResponse.IsSuccessful);
			fileSaverHelperMock.Verify(fileHelper => fileHelper.AddFile(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
		}

		[Fact]
		public void DeleteFile_When_FileExists_Then_FileDeleted()
		{
			var fileSaverHelperMock = new Mock<IFileServerClient>();
			fileSaverHelperMock.Setup(fileHelper => fileHelper.DeleteFile(It.IsAny<string>()));

			var fileService = new FileService(_dbContext, fileSaverHelperMock.Object);

			var fileId = Guid.Parse("E18B9E36-CC1A-4A7A-B996-B6FAD1D86232");
			var userId = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C");

			var deleteFileResponse = fileService.DeleteFile(fileId, userId).Result;

			var dbFile = _dbContext.Files.FirstOrDefault(f => f.Id == fileId);

			Assert.Null(dbFile);
			Assert.Null(deleteFileResponse.Error);
			Assert.True(deleteFileResponse.IsSuccessful);
			fileSaverHelperMock.Verify(fileHelper => fileHelper.DeleteFile(It.IsAny<string>()), Times.Once());
		}

		[Fact]
		public void DeleteFile_When_IncorrectFile_Then_Error()
		{
			var fileSaverHelperMock = new Mock<IFileServerClient>();

			var fileService = new FileService(_dbContext, fileSaverHelperMock.Object);

			var userId = Guid.Parse("E18B9E36-CC1A-4A7A-B996-B6FAD1D86232");
			var fileId = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C");

			var deleteFileResponse = fileService.DeleteFile(fileId, userId).Result;

			Assert.NotNull(deleteFileResponse.Error);
			Assert.False(deleteFileResponse.IsSuccessful);
			fileSaverHelperMock.Verify(fileHelper => fileHelper.DeleteFile(It.IsAny<string>()), Times.Never());
		}

		[Fact]
		public async void GetFileById_When_CorrectFile_Then_GetFileData()
		{
			var fileData = "Let's decide that it is base64 string";
			var fileSaverHelperMock = new Mock<IFileServerClient>();
			fileSaverHelperMock.Setup(fileHelper => fileHelper.ReadFile(It.IsAny<string>())).Returns(GetCompletedTask(fileData));

			var fileService = new FileService(_dbContext, fileSaverHelperMock.Object);

			var fileId = Guid.Parse("3BA7C149-0039-4A2C-B401-01AC0BF3FD14");
			var userId = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C");

			var getFileResponse = await fileService.GetFileById(fileId, userId);

			Assert.Null(getFileResponse.Error);
			Assert.True(getFileResponse.IsSuccessful);
			Assert.Equal(getFileResponse.ResponseResult.Base64File, fileData);
			fileSaverHelperMock.Verify(fileHelper => fileHelper.ReadFile(It.IsAny<string>()), Times.Once());
		}

		[Fact]
		public async void GetById_When_IncorrectUser_Then_GetError()
		{
			var fileSaverHelperMock = new Mock<IFileServerClient>();

			var fileService = new FileService(_dbContext, fileSaverHelperMock.Object);

			var userId = Guid.Parse("3BA7C149-0039-4A2C-B401-01AC0BF3FD14");
			var fileId = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C");

			var getFileResponse = await fileService.GetFileById(fileId, userId);

			Assert.NotNull(getFileResponse.Error);
			Assert.False(getFileResponse.IsSuccessful);
			fileSaverHelperMock.Verify(fileHelper => fileHelper.ReadFile(It.IsAny<string>()), Times.Never());
		}
	}
}
