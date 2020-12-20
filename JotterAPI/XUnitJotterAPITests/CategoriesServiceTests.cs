using JotterAPI.DAL;
using JotterAPI.Model.DTOs.Categories;
using JotterAPI.Services;
using System;
using System.Linq;
using Xunit;

namespace XUnitJotterAPITests
{
	public class CategoriesServiceTests : JotterTestDbContext
	{
		[Fact]
		public void CategoriesServiceAddCategory_When_AddGoodCategory_Then_CategoryAdded()
		{
			var categoriesService = new CategoriesService(_dbContext);

			var category = new NewCategory { 
				Name = "My perfect category"
			};
			var userId = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C");

			var createdCategoryResponse = categoriesService.AddCategory(category, userId).Result;

			Assert.True(createdCategoryResponse.IsSuccessful);
			Assert.Null(createdCategoryResponse.Error);
			Assert.NotEqual(createdCategoryResponse.ResponseResult.Id, new Guid());
		}

		[Fact]
		public void CategoriesServiceAddCategory_When_AddGoodCategory_Then_CategoryExistInDb()
		{
			var categoriesService = new CategoriesService(_dbContext);

			var category = new NewCategory {
				Name = "My perfect category which exists"
			};
			var userId = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C");
			
			var createdCategoryResponse = categoriesService.AddCategory(category, userId).Result;
			var savedCategory = _dbContext.Categories.FirstOrDefault(c => c.Id == createdCategoryResponse.ResponseResult.Id);

			Assert.True(createdCategoryResponse.IsSuccessful);
			Assert.NotEqual(createdCategoryResponse.ResponseResult.Id, new Guid());
			Assert.NotNull(savedCategory);
		}

		[Fact]
		public void CategoriesServiceAddCategory_When_AddCategoryForUnexistingUser_Then_FailureResult()
		{
			var categoriesService = new CategoriesService(_dbContext);

			var category = new NewCategory {
				Name = "My perfect category which exists"
			};
			var userId = Guid.Parse("34F0D89E-98B3-4910-BF7B-FA5CBF1BA221");

			var createdCategoryResponse = categoriesService.AddCategory(category, userId).Result;

			Assert.False(createdCategoryResponse.IsSuccessful);
			Assert.NotNull(createdCategoryResponse.Error);
			Assert.Null(createdCategoryResponse.ResponseResult);
		}

		[Fact]
		public void CategoriesServiceGetByUser_When_GetUserCategories_Then_GetAllNeeded()
		{
			var userId = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C");

			var categoriesService = new CategoriesService(_dbContext);

			var userCategoriesResponse = categoriesService.GetByUser(userId);

			var dbCategoriesForUserCount = _dbContext.Categories.Where(c => c.UserId == userId).Count();

			Assert.True(userCategoriesResponse.IsSuccessful);
			Assert.Null(userCategoriesResponse.Error);
			Assert.Equal(userCategoriesResponse.ResponseResult.Categories.Count(), dbCategoriesForUserCount);
		}

		[Fact]
		public void CategoriesServiceGetByUser_When_GetUserCategoriesForUnexistingUser_Then_NoData()
		{
			var categoriesService = new CategoriesService(_dbContext);

			var userCategoriesResponse = categoriesService.GetByUser(Guid.Parse("34F0D89E-98B3-4910-BF7B-FA5CBF1BA221"));

			Assert.True(userCategoriesResponse.IsSuccessful);
			Assert.Null(userCategoriesResponse.Error);
			Assert.Empty(userCategoriesResponse.ResponseResult.Categories);
		}
	}
}
