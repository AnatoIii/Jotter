using System;
using System.Collections.Generic;
using System.Text;

namespace JotterAPI.DAL.Model
{
	public class File
	{
		public Guid Id { get; set; }

		public Guid NoteId { get; set; }

		public string Name { get; set; }
		
		public string Path { get; set; }
	}
}
