using Model;
using System.Collections.Generic;

namespace BL.Response.Responses
{
	public class CategoryResponse : Response
	{
		public Category Category { get; set; }
	}

	public class CategoriesResponse: Response
	{
		public IEnumerable<Category> Categories { get; set; }
	}
}
