using JotterAPI.DAL;
using JotterAPI.DAL.Model;
using JotterAPI.Model.DTOs.Categories;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JotterAPI.Services
{
	public class CategoriesService : BaseService, ICategoriesService
	{
		public CategoriesService(JotterDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<Response<CategoryResult>> AddCategory(NewCategory newCategory, Guid userId)
		{
			var user = GetUser(userId);
			if (user == null) {
				return new Response<CategoryResult>("Such user doesn't exist");
			}

			var category = new Category {
				Name = newCategory.Name,
				UserId = userId,
				Password = newCategory.Password // Add password hashing
			};

			_dbContext.Categories.Add(category);
			await _dbContext.SaveChangesAsync();

			return new Response<CategoryResult>(new CategoryResult(category));
		}

        public async Task<Response<ResponseResult>> DeleteCategory(Guid categoryId, Guid userId)
		{
			var user = GetUser(userId);
			if (user == null)
			{
				return new Response<ResponseResult>("User with such Id doesn't exist");
			}

			var category = _dbContext.Categories
				.FirstOrDefault(category => category.Id == categoryId && category.UserId == userId);
			if (category == null)
			{
				return new Response<ResponseResult>("Such category doesn't exist");
			}

			_dbContext.Categories.Remove(category);
			await _dbContext.SaveChangesAsync();

			return new Response<ResponseResult>(new ResponseResult());
		}

        public Response<CategoriesResult> GetByUser(Guid userId) 
		{
			var categories = _dbContext.Categories.Where(category => category.UserId == userId);

			return new Response<CategoriesResult>(new CategoriesResult(categories));
		}
	}
}
