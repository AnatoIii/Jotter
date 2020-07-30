using System;
using System.Collections.Generic;
using System.Text;

namespace Model.DTO
{
	public class NoteToCreate
	{
		public Guid UserId { get; set; }

		public Guid CategoryId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
	}
}
