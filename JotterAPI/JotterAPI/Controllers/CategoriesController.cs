using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

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
	}
}
