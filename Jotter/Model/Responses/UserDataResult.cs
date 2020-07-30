using System;

namespace Model.Responses
{
	public class UserDataResult : ResponseResult
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }

		public UserDataResult() { }
	}
}
