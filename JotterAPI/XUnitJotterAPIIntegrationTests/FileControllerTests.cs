﻿using JotterAPI;
using JotterAPI.DAL.Model;
using JotterAPI.Model.DTOs.Files;
using JotterAPI.Model.DTOs.User;
using JotterAPI.Model.Reponses;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using XUnitJotterAPIIntegrationTests.WebApplicationFactory;

namespace XUnitJotterAPIIntegrationTests
{
    public class FileControllerTests : IClassFixture<JotterWebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private User _dbUser;
        private Guid _noteId;
        private string _token;

        public FileControllerTests(JotterWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _dbUser = new User
            {
                Id = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C"),
                Email = "test.user@gmail.com",
                Name = "Test User"
            };
            _noteId = Guid.Parse("2E20F240-ADD6-496F-A2BD-794043D94940");
            _token = GetTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
        }

        private string GetTokenAsync()
        {
            var userLoginCredential = new UserLoginCredentials
            {
                Email = _dbUser.Email,
                Password = "12345678"
            };
            string json = JsonConvert.SerializeObject(userLoginCredential);

            var httpResponse = _client.PostAsync("login", new StringContent(json, Encoding.UTF8, "application/json")).Result;

            var stringResponse = httpResponse.Content.ReadAsStringAsync().Result;
            var userResponse = JsonConvert.DeserializeObject<Response<TokenResponse>>(stringResponse);

            return userResponse.ResponseResult.AccessToken;
        }

        [Fact]
        public async Task AddFileDeleteFile_When_CorrectData_Then_NoErrors()
        {
            var fileContent = Convert.ToBase64String(Encoding.ASCII.GetBytes("Some very userful image"));

            var fileToSaveData = new FileToSaveData {
                Base64File = fileContent,
                FileName = "testfile.jpeg",
                NoteId = _noteId
            };
            string json = JsonConvert.SerializeObject(fileToSaveData);

            var httpResponse = await _client.PostAsync("files", new StringContent(json, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var createFileResponse = JsonConvert.DeserializeObject<Response<FileResult>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(createFileResponse.IsSuccessful);
            Assert.Null(createFileResponse.Error);
            Assert.NotNull(createFileResponse.ResponseResult);

            var httpResponseGet = await _client.GetAsync($"files/{createFileResponse.ResponseResult.Id}");
            var stringResponseGet = await httpResponseGet.Content.ReadAsStringAsync();
            var getFileResponse = JsonConvert.DeserializeObject<Response<FileDataResult>>(stringResponseGet);

            Assert.Equal(HttpStatusCode.OK, httpResponseGet.StatusCode);
            Assert.True(getFileResponse.IsSuccessful);
            Assert.Null(getFileResponse.Error);
            Assert.Equal(fileContent, getFileResponse.ResponseResult.Base64File);

            var httpResponseDelete = await _client.DeleteAsync($"files/{createFileResponse.ResponseResult.Id}");
            var stringResponseDelete = await httpResponseDelete.Content.ReadAsStringAsync();
            var deleteFileResponse = JsonConvert.DeserializeObject<Response<FileDataResult>>(stringResponseDelete);

            Assert.Equal(HttpStatusCode.OK, httpResponseDelete.StatusCode);
            Assert.True(deleteFileResponse.IsSuccessful);
            Assert.Null(deleteFileResponse.Error);
        }

        [Fact]
        public async Task AddFile_When_IncorrectNote_Then_Error()
        {
            var fileContent = Convert.ToBase64String(Encoding.ASCII.GetBytes("Some very userful image"));

            var fileToSaveData = new FileToSaveData {
                Base64File = fileContent,
                FileName = "testfile.jpeg",
                NoteId = Guid.NewGuid()
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
