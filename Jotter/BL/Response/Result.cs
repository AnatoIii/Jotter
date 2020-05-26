using System;
using System.Collections.Generic;
using System.Text;

namespace BL.Response
{
	public class Result<T> where T : Response
	{
		public T Response { get; set; }
		public bool IsSuccessful { get; set; }
		public string ErrorMessage { get; set; }
	}
}
