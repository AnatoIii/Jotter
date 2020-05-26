using System;

namespace JotterAPI.DAL.Model
{
	public class File : Entity
	{
		public Guid NoteId { get; set; }

		public string Name { get; set; }
		
		public string Path { get; set; }

		public Note Note { get; set; }
	}
}
