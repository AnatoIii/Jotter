using JotterAPI.DAL.Model;
using System;

namespace JotterAPI.Model.Reponses
{
	public class UserDataResult : ResponseResult
	{
		public Guid Id { get; set; }

		public string Name { get; set; }

		public string Email { get; set; }


		public UserDataResult(User user)
		{
			Id = user.Id;
			Name = user.Name;
			Email = user.Email;
		}

		public UserDataResult() { }
	}
}
