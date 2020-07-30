using System;
using System.Collections.Generic;
using System.Text;

namespace Model.Responses
{
	public class FileResult : ResponseResult
	{
		public Guid Id { get; set; }

		public string FileName { get; set; }

		public FileResult(File file)
		{
			Id = file.Id;

			FileName = file.Name;
		}

		public FileResult() { }
	}

	public class FileDataResult : ResponseResult
	{
		public Guid Id { get; set; }

		public string FileName { get; set; }

		public string Base64File { get; set; }

		public FileDataResult(File file, string base64File)
		{
			Id = file.Id;
			FileName = file.Name;
			Base64File = base64File;
		}

		public FileDataResult() { }
	}
}
