using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace JotterAPI.DAL.Model
{
	public class Entity
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public Guid Id { get; set; }
	}
}
