using JotterAPI.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JotterAPI.Model.Reponses
{
	public class CategoriesResult : ResponseResult
	{
		public IEnumerable<CategoryResult> Categories { get; set; }

		public CategoriesResult(IEnumerable<Category> categories)
		{
			Categories = categories.Select(c => new CategoryResult(c));
		}

		public CategoriesResult() { }
	}

	public class CategoryResult : ResponseResult
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public bool IsLocked { get; set; }


		public CategoryResult(Category category)
		{
			Id = category.Id;
			Name = category.Name;
			IsLocked = !string.IsNullOrWhiteSpace(category.Password);
		}

		public CategoryResult() { }
	}
}
