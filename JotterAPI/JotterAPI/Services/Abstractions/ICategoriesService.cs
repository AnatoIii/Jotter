using JotterAPI.Model.DTOs.Categories;
using JotterAPI.Model.Reponses;
using System;
using System.Threading.Tasks;

namespace JotterAPI.Services.Abstractions
{
	public interface ICategoriesService
	{
		Task<Response<CategoryResult>> AddCategory(NewCategory newCategory, Guid userId);
		Response<CategoriesResult> GetByUser(Guid userId);
		Task<Response<ResponseResult>> DeleteCategory(Guid categoryId, Guid userId);
	}
}
