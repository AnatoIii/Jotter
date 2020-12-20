using System;
using System.ComponentModel.DataAnnotations;

namespace JotterAPI.Model.DTOs.Categories
{
	public class NewCategory
	{

		[Required]
		public string Name { get; set; }

		public string? Password { get; set; }
	}
}
