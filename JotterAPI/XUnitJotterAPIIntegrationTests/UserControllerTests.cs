using JotterAPI;
using JotterAPI.DAL.Model;
using JotterAPI.Model.DTOs.User;
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
	public class UserControllerTests : IClassFixture<WebApplicationFactory<Startup>>
	{
        private HttpClient _client;
        private User _dbUser;

        public UserControllerTests(WebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _dbUser = new User {
                Id = Guid.Parse("72FBC0E3-A81F-4036-A54D-9A1B42FC48A1"),
                Name = "Test User", 
                Email = "test@gmail.com",
                Password = "RandomPassword"
            };
        }

        [Fact]
        public async Task Login_When_UserCredentialsAreCorrect_Then_User()
        {
            var userLoginCredential = new UserLoginCredentials {
                Email = _dbUser.Email,
                Password = _dbUser.Password
            };
            string json = JsonConvert.SerializeObject(userLoginCredential);

            var httpResponse = await _client.PostAsync("login", new StringContent(json, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<Response<UserDataResult>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(userResponse.IsSuccessful);
            Assert.Null(userResponse.Error);
            Assert.Equal(_dbUser.Id, userResponse.ResponseResult.Id);
        }

        [Fact]
        public async Task Login_When_UserCredentialsAreIncorrect_Then_Error()
        {
            var userLoginCredential = new UserLoginCredentials {
                Email = _dbUser.Email,
                Password = "askdhafadas"
            };
            string json = JsonConvert.SerializeObject(userLoginCredential);

            var httpResponse = await _client.PostAsync("login", new StringContent(json, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<Response<UserDataResult>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.False(userResponse.IsSuccessful);
            Assert.NotNull(userResponse.Error);
            Assert.Null(userResponse.ResponseResult);
        }

        [Fact]
        public async Task GetById_When_CorrectId_Then_GetUser()
        {
            var httpResponse = await _client.GetAsync($"user/{_dbUser.Id}");

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<Response<UserDataResult>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(userResponse.IsSuccessful);
            Assert.Null(userResponse.Error);
            Assert.Equal(_dbUser.Name,userResponse.ResponseResult.Name);
        }

        [Fact]
        public async Task GetById_When_InCorrectId_Then_Error()
        {
            var httpResponse = await _client.GetAsync($"user/{Guid.NewGuid()}");

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<Response<UserDataResult>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.False(userResponse.IsSuccessful);
            Assert.NotNull(userResponse.Error);
            Assert.Null(userResponse.ResponseResult);
        }
    }
}
