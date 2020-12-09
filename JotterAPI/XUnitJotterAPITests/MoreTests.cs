using JotterAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using Xunit;
using XUnitJotterAPITests.Helpers;

namespace XUnitJotterAPITests
{
	[CollectionDefinition("Tests without parallelization", DisableParallelization = true)]
	public class MoreTests
    {
		[Fact(Timeout = 1)]
		public void PasswordHasher_TenTimesTimeout_LessThan1Second()
		{
			TimeoutHelper.CompletesIn(10000, () =>
			{
				var password = "This is very strong password";
				var passwordHasher = new PasswordHasher();

				for (int i = 0; i < 10; i++)
				{
					var (hash, salt) = passwordHasher.HashPassword(password);

					var result = passwordHasher.ArePasswordsTheSame(password, salt, hash);
				}
			});
		}

		[Fact]
		public void TestThatFails()
        {
			Assert.Throws<TimeoutException>(() => TimeoutHelper.CompletesIn(100, () => Thread.Sleep(1000)));
		}
	}
}
