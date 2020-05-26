using System;

namespace JotterAPI.Model.DTOs.Notes
{
	public class NoteToEdit
	{
		public Guid Id { get; set; }

		public Guid UserId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }
	}
}
