using JotterAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace XUnitJotterAPITests
{
	public class PasswordHasherTests
	{
		[Fact]
		public void PasswordHasher_When_HashingPassword_Then_CheckingIsTrue()
		{
			var password = "This is very strong password";
			var passwordHasher = new PasswordHasher();

			var (hash, salt) = passwordHasher.HashPassword(password);

			var result = passwordHasher.ArePasswordsTheSame(password, salt, hash);

			Assert.True(result);
		}
	}
}
