using System;

namespace Model.DTO
{
	public class FileToSaveData
	{
		public string FileName { get; set; }

		public string Base64File { get; set; }

		public Guid NoteId { get; set; }

		public Guid UserId { get; set; }
	}
}
