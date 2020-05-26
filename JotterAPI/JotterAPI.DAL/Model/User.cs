using System;
using System.Collections.Generic;
using System.Text;

namespace JotterAPI.DAL.Model
{
	public class User
	{
		public Guid Id { get; set; }

		public string Name { get; set; }
		
		public string Email { get; set; }
		
		public string Password { get; set; }
		
		public string PasswordSalt { get; set; }

		public List<Note> Notes { get; set; }

		public List<Category> Categories { get; set; }
	}
}
