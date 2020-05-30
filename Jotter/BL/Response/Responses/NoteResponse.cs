using Model;
using System.Collections.Generic;

namespace BL.Response.Responses
{
	public class NotesResponse : Response
	{
		public IEnumerable<Note> Notes { get; set; }
	}

	public class NoteResponse : Response
	{
		public Note Note { get; set; }
	}
}
