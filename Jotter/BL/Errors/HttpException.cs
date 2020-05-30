using System;

namespace BL.Errors
{
	public class HttpException : Exception
	{
		public HttpException(string path) : base($"Error while sending http request to {path}")
		{
		}
	}
}
