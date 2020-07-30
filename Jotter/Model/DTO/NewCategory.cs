using System;

namespace Model.DTO
{
	public class NewCategory
	{
		public Guid UserId { get; set; }

		public string Name { get; set; }

		public string Password { get; set; }
	}
}
