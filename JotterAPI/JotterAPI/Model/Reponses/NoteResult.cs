using JotterAPI.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JotterAPI.Model.Reponses
{
	public class FileData
	{
		public Guid Id { get; set; }
		public string Name { get; set; }

		public FileData(File file)
		{
			Id = file.Id;
			Name = file.Name;
		}
	}

	public class NoteResult : ResponseResult
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public IEnumerable<FileData> Files { get; set; }

		public NoteResult(Note note)
		{
			Id = note.Id;
			Name = note.Name;
			Description = note.Description;
			Files = note.Files.Select(file => new FileData(file));
		}
	}

	public class NotesResult : ResponseResult
	{
		public IEnumerable<NoteResult> Notes { get; set; }

		public NotesResult(IEnumerable<Note> notes)
		{
			Notes = notes.Select(note => new NoteResult(note));
		}
	}
}
