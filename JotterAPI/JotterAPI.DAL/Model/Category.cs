using System;
using System.Collections.Generic;

namespace JotterAPI.DAL.Model
{
	public class Category : Entity
	{
		public string Name { get; set; }
		
		public string Password { get; set; }

		public Guid UserId { get; set; }

		public List<Note> Notes { get; set; }

		public User User { get; set; }
	}
}
