using JotterAPI.Model.DTOs.Categories;
using JotterAPI.Model.Reponses;
using System;
using System.Threading.Tasks;

namespace JotterAPI.Services.Abstractions
{
	public interface ICategoriesService
	{
		Task<Response<CategoryResult>> AddCategory(NewCategory newCategory);
		Response<CategoriesResult> GetByUser(Guid userId);
	}
}
