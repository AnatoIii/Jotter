using JotterAPI.DAL;
using JotterAPI.DAL.Model;
using JotterAPI.Model.DTOs.Categories;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JotterAPI.Services
{
	public class CategoriesService : BaseService, ICategoriesService
	{
		public CategoriesService(JotterDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<Response<CategoryResult>> AddCategory(NewCategory newCategory)
		{
			var user = GetUser(newCategory.UserId);
			if (user == null) {
				return new Response<CategoryResult>("Such user doesn't exist");
			}

			var category = new Category {
				Name = newCategory.Name,
				UserId = newCategory.UserId,
				Password = newCategory.Password // Add password hashing
			};

			_dbContext.Categories.Add(category);
			await _dbContext.SaveChangesAsync();

			return new Response<CategoryResult>(new CategoryResult(category));
		}

		public Response<CategoriesResult> GetByUser(Guid userId) // Get it from token
		{
			var categories = _dbContext.Categories.Where(category => category.UserId == userId);

			return new Response<CategoriesResult>(new CategoriesResult(categories));
		}
	}
}
