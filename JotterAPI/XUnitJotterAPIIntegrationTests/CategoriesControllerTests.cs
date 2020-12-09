using JotterAPI;
using JotterAPI.DAL.Model;
using JotterAPI.Model.DTOs.Categories;
using JotterAPI.Model.Reponses;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore.Internal;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using XUnitJotterAPIIntegrationTests.WebApplicationFactory;

namespace XUnitJotterAPIIntegrationTests
{
    public class CategoriesControllerTests : IClassFixture<JotterWebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private User _dbUser;
        private Guid _categoryId;

        public CategoriesControllerTests(JotterWebApplicationFactory<Startup> factory)
        {
            _categoryId = Guid.Parse("F9A27BE9-3771-4AAE-89C8-444D4D5F828F");
            _client = factory.CreateClient();
            _dbUser = new User
            {
                Id = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C"),
                Email = "test.user@gmail.com",
                Password = "12345678",
                Name = "Test User"
            };
        }

        [Fact]
        public async Task AddCategory_When_CorrectNewCategory_Then_CategoryCreated()
        {
            var newCategory = new NewCategory {
                Name = "Test category",
                UserId = _dbUser.Id
            };
            string json = JsonConvert.SerializeObject(newCategory);

            var httpResponse = await _client.PostAsync("categories", new StringContent(json, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var categoriesResponse = JsonConvert.DeserializeObject<Response<CategoryResult>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(categoriesResponse.IsSuccessful);
            Assert.Null(categoriesResponse.Error);
            Assert.NotNull(categoriesResponse.ResponseResult);
        }

        [Fact]
        public async Task AddCategory_When_IncorrectUser_Then_Error()
        {
            var newCategory = new NewCategory {
                Name = "Test category",
                UserId = Guid.NewGuid()
            };
            string json = JsonConvert.SerializeObject(newCategory);

            var httpResponse = await _client.PostAsync("categories", new StringContent(json, Encoding.UTF8, "application/json"));

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var categoriesResponse = JsonConvert.DeserializeObject<Response<CategoryResult>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.False(categoriesResponse.IsSuccessful);
            Assert.NotNull(categoriesResponse.Error);
            Assert.Null(categoriesResponse.ResponseResult);
        }

        [Fact]
        public async Task GetCategoriesByUser_When_CorrectUser_Then_GetAtLeastOneCategory()
        {
            var httpResponse = await _client.GetAsync($"categories/{_dbUser.Id}");

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var categoriesResponse = JsonConvert.DeserializeObject<Response<CategoriesResult>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(categoriesResponse.IsSuccessful);
            Assert.Null(categoriesResponse.Error);
            Assert.Contains(categoriesResponse.ResponseResult.Categories, c => c.Id == _categoryId);
        }

        [Fact]
        public async Task GetCategoriesByUser_When_InCorrectUser_Then_NoData()
        {
            var httpResponse = await _client.GetAsync($"categories/{Guid.NewGuid()}");

            var stringResponse = await httpResponse.Content.ReadAsStringAsync();
            var categoriesResponse = JsonConvert.DeserializeObject<Response<CategoriesResult>>(stringResponse);

            Assert.Equal(HttpStatusCode.OK, httpResponse.StatusCode);
            Assert.True(categoriesResponse.IsSuccessful);
            Assert.Null(categoriesResponse.Error);
            Assert.Empty(categoriesResponse.ResponseResult.Categories);
        }
    }
}
