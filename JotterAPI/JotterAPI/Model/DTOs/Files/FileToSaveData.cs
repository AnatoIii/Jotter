using System;

namespace JotterAPI.Model.DTOs.Files
{
	public class FileToSaveData
	{
		public string FileName { get; set; }

		public string Base64File { get; set; }

		public Guid NoteId { get; set; }
	}
}
