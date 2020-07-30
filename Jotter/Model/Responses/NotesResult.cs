using System;
using System.Collections.Generic;
using System.Linq;

namespace Model.Responses
{
	public class FileData
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public FileData() { }
	}

	public class NoteResult : ResponseResult
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public IEnumerable<FileData> Files { get; set; }
	}

	public class NotesResult : ResponseResult
	{
		public IEnumerable<NoteResult> Notes { get; set; }
	}
}
