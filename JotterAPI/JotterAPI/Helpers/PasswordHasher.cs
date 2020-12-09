using JotterAPI.Helpers.Abstractions;
using System;
using System.Security.Cryptography;

namespace JotterAPI.Helpers
{
	public class PasswordHasher : IPasswordHasher
	{
		public (string hash, string salt) HashPassword(string password)
		{
			byte[] salt;
			new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
			byte[] hash = pbkdf2.GetBytes(20);

			return (Convert.ToBase64String(hash), Convert.ToBase64String(salt));
		}

		public bool ArePasswordsTheSame(string password, string saltString, string hashString)
		{
			var salt = Convert.FromBase64String(saltString);

			var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
			byte[] hash = pbkdf2.GetBytes(20);
			Console.WriteLine(Convert.ToBase64String(hash));

			return Convert.ToBase64String(hash) == hashString;
		}
	}
}
