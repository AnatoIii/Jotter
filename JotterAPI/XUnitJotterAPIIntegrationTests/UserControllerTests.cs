using JotterAPI;
using JotterAPI.DAL.Model;
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
	public class UserControllerTests : IClassFixture<JotterWebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private HttpClient _clientWithoutAuth;
        private User _dbUser;

        public UserControllerTests(JotterWebApplicationFactory<Startup> factory)
        {
            _client = factory.CreateClient();
            _clientWithoutAuth = factory.CreateClient();
            _dbUser = new User
            {
                Id = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C"),
                Email = "test.user@gmail.com",
                Password = "12345678",
                Name = "Test User"
            };
            string token = GetTokenAsync();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        public async Task Login_When_UserCredentialsAreCorrect_Then_User()
        {
            var userLoginCredential = new UserLoginCredentials {
                Email = _dbUser.Email,
                Password = _dbUser.Password
            };
            string json = JsonConvert.SerializeObject(userLoginCredential);

            var httpResponse = await _clientWithoutAuth.PostAsync("login", new StringContent(json, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<Response<TokenResponse>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(userResponse.IsSuccessful);
            Assert.Null(userResponse.Error);
            Assert.NotNull(userResponse.ResponseResult.AccessToken);
        }

        [Fact]
        public async Task Login_When_UserCredentialsAreIncorrect_Then_Error()
        {
            var userLoginCredential = new UserLoginCredentials {
                Email = _dbUser.Email,
                Password = "askdhafadas"
            };
            string json = JsonConvert.SerializeObject(userLoginCredential);

            var httpResponse = await _clientWithoutAuth.PostAsync("login", new StringContent(json, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var userResponse = JsonConvert.DeserializeObject<Response<TokenResponse>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.False(userResponse.IsSuccessful);
            Assert.NotNull(userResponse.Error);
            Assert.Null(userResponse.ResponseResult);
        }

        [Fact]
        public async Task GetById_When_CorrectId_Then_GetUser()
        {
            var httpResponse = await _client.GetAsync($"user");

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
            var httpResponse = await _clientWithoutAuth.GetAsync($"user");

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            //var userResponse = JsonConvert.DeserializeObject<Response<UserDataResult>>(stringResponse);

            Assert.Equal("", stringResponse);

            Assert.Equal(HttpStatusCode.Unauthorized, httpResponse.StatusCode);
            //Assert.False(userResponse.IsSuccessful);
            //Assert.NotNull(userResponse.Error);
            //Assert.Null(userResponse.ResponseResult);
        }
    }
}
