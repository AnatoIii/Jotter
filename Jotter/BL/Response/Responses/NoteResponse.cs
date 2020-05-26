using Model;
using System.Collections.Generic;

namespace BL.Response.Responses
{
	public class NoteResponse : Response
	{
		public IEnumerable<Note> Notes { get; set; }
	}
}
