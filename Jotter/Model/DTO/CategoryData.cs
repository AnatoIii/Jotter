using System;

namespace Model.DTO
{
	public class CategoryData
	{
		public Guid CategoryId { get; set; }

		public Guid UserId { get; set; }

		public string CategoryPassword { get; set; }
	}
}
