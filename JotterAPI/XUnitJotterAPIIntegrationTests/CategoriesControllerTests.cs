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

namespace XUnitJotterAPIIntegrationTests
{
    public class CategoriesControllerTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private HttpClient _client;
        private User _dbUser;
        private Guid _categoryId;

        public CategoriesControllerTests(WebApplicationFactory<Startup> factory)
        {
            _categoryId = Guid.Parse("C7709E39-4FF9-4DC3-B976-260097B74C5C");
            _client = factory.CreateClient();
            _dbUser = new User {
                Id = Guid.Parse("72FBC0E3-A81F-4036-A54D-9A1B42FC48A1"),
                Name = "Test User",
                Email = "test@gmail.com",
                Password = "RandomPassword"
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
