using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JotterAPI.Helpers.Abstractions
{
	public interface IPasswordHasher
	{
		(string hash, string salt) HashPassword(string password);
		bool ArePasswordsTheSame(string password, string saltString, string hashString);
	}
}
