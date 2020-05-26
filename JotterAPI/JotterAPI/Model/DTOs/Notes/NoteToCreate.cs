using System;

namespace JotterAPI.Model.DTOs.Notes
{
	public class NoteToCreate
	{
		public Guid UserId { get; set; }
		
		public Guid CategoryId { get; set; }
		
		public string Name { get; set; }
		
		public string Description { get; set; }
	}
}
