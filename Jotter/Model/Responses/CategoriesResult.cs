using System;
using System.Collections.Generic;

namespace Model.Responses
{
	public class CategoriesResult : ResponseResult
	{
		public IEnumerable<CategoryResult> Categories { get; set; }

		public CategoriesResult() { }
	}

	public class CategoryResult : ResponseResult
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public bool IsLocked { get; set; }

		public CategoryResult() { }
	}
}
