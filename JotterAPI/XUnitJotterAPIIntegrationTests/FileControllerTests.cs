using JotterAPI;
using JotterAPI.DAL.Model;
using JotterAPI.Model.DTOs.Files;
using JotterAPI.Model.Reponses;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace XUnitJotterAPIIntegrationTests
{
    public class FileControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private User _dbUser;
        private Guid _noteId;

        public FileControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _dbUser = new User {
                Id = Guid.Parse("72FBC0E3-A81F-4036-A54D-9A1B42FC48A1"),
                Name = "Test User",
                Email = "test@gmail.com",
                Password = "RandomPassword"
            };
            _noteId = Guid.Parse("E9C6B5E4-1C8D-4DFD-B382-F9C4387D7E8F");
        }

        [Fact]
        public async Task AddFileDeleteFile_When_CorrectData_Then_NoErrors()
        {
            var fileContent = Convert.ToBase64String(Encoding.ASCII.GetBytes("Some very userful image"));

            var fileToSaveData = new FileToSaveData {
                Base64File = fileContent,
                FileName = "testfile.jpeg",
                NoteId = _noteId,
                UserId = _dbUser.Id
            };
            string json = JsonConvert.SerializeObject(fileToSaveData);

            var httpResponse = await _client.PostAsync("files", new StringContent(json, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var createFileResponse = JsonConvert.DeserializeObject<Response<FileResult>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(createFileResponse.IsSuccessful);
            Assert.Null(createFileResponse.Error);
            Assert.NotNull(createFileResponse.ResponseResult);

            var httpResponseGet = await _client.GetAsync($"files?UserId={_dbUser.Id}&FileId={createFileResponse.ResponseResult.Id}");
            var stringResponseGet = await httpResponseGet.Content.ReadAsStringAsync();
            var getFileResponse = JsonConvert.DeserializeObject<Response<FileDataResult>>(stringResponseGet);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(createFileResponse.IsSuccessful);
            Assert.Null(createFileResponse.Error);
            Assert.Equal(fileContent, getFileResponse.ResponseResult.Base64File);

            var httpResponseDelete = await _client.DeleteAsync($"files?UserId={_dbUser.Id}&FileId={createFileResponse.ResponseResult.Id}");
            var stringResponseDelete = await httpResponseDelete.Content.ReadAsStringAsync();
            var deleteFileResponse = JsonConvert.DeserializeObject<Response<FileDataResult>>(stringResponseDelete);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(deleteFileResponse.IsSuccessful);
            Assert.Null(deleteFileResponse.Error);
        }

        [Fact]
        public async Task AddFile_When_IncorrectUser_Then_Error()
        {
            var fileContent = Convert.ToBase64String(Encoding.ASCII.GetBytes("Some very userful image"));

            var fileToSaveData = new FileToSaveData {
                Base64File = fileContent,
                FileName = "testfile.jpeg",
                NoteId = _noteId,
                UserId = Guid.NewGuid()
            };
            string json = JsonConvert.SerializeObject(fileToSaveData);

            var httpResponse = await _client.PostAsync("files", new StringContent(json, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var createFileResponse = JsonConvert.DeserializeObject<Response<FileResult>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.False(createFileResponse.IsSuccessful);
            Assert.NotNull(createFileResponse.Error);
            Assert.Null (createFileResponse.ResponseResult);
        }
    }
}
