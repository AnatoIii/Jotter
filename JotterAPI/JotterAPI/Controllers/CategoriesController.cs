using JotterAPI.Model.DTOs.Categories;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JotterAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CategoriesController : ControllerBase
	{
		private readonly ICategoriesService _categoriesService;

		public CategoriesController(ICategoriesService categoriesService)
		{
			_categoriesService = categoriesService;
		}

		[HttpPost]
		public Task<Response<CategoryResult>> AddCategory([FromBody]NewCategory newCategory)
		{
			return _categoriesService.AddCategory(newCategory, GetUserId());
		}

		[HttpGet]
		public Response<CategoriesResult> GetByUser()
		{
			return _categoriesService.GetByUser(GetUserId());
		}

		[HttpDelete("{categoryId}")]
		public Task<Response<ResponseResult>> DeleteCategory(Guid categoryId)
        {
			return _categoriesService.DeleteCategory(categoryId, GetUserId());
        }

		private Guid GetUserId()
		{
			return Guid.Parse(User.Identity.Name);
		}
	}
}
