using JotterAPI.Helpers;
using JotterAPI.Helpers.Abstractions;
using JotterAPI.Model.DTOs.User;
using JotterAPI.Services;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace XUnitJotterAPITests
{
	public class UserServiceTests : JotterTestDbContext
	{
		[Fact]
		public void UserServiceLogin_When_CorrectUser_Then_GetUser()
		{
			var passwordHasher = new PasswordHasher();
			var tokenConfig = Microsoft.Extensions.Options.Options.Create(new TokenConfig()
			{
				Issuer = "Issuer",
				JWTLifetime = 60,
				Secret = "Secret auyuiosdyurytasvbdjkast7fvajksdhgyotuasidctd"
			});

			var userService = new UserService(_dbContext, passwordHasher, tokenConfig);

			var userLoginCredentials = new UserLoginCredentials {
				Email = "test.user@gmail.com",
				Password = "12345678",
			};

			var userLoginResponse = userService.Login(userLoginCredentials);

			Assert.True(userLoginResponse.IsSuccessful);
			Assert.Null(userLoginResponse.Error);
			Assert.NotNull(userLoginResponse.ResponseResult.AccessToken);
		}

		[Fact]
		public void UserServiceLogin_When_IncorrectEmail_Then_Error()
		{
			var passwordHasherMock = new Mock<IPasswordHasher>();
			var tokenConfig = Microsoft.Extensions.Options.Options.Create(new TokenConfig()
			{
				Issuer = "Issuer",
				JWTLifetime = 60,
				Secret = "Secret auyuiosdyurytasvbdjkast7fvajksdhgyotuasidctd"
			});

			var userService = new UserService(_dbContext, passwordHasherMock.Object, tokenConfig);

			var userLoginCredentials = new UserLoginCredentials {
				Email = "incorrect@gmail.com",
				Password = "12345678",
			};

			var userLoginResponse = userService.Login(userLoginCredentials);

			Assert.False(userLoginResponse.IsSuccessful);
			Assert.NotNull(userLoginResponse.Error);
			Assert.Null(userLoginResponse.ResponseResult);
		}

		[Fact]
		public void UserServiceLogin_When_WrongPassword_Then_Error()
		{
			var passwordHasherMock = new Mock<IPasswordHasher>();
			var tokenConfig = Microsoft.Extensions.Options.Options.Create(new TokenConfig()
			{
				Issuer = "Issuer",
				JWTLifetime = 60,
				Secret = "Secret auyuiosdyurytasvbdjkast7fvajksdhgyotuasidctd"
			});

			var userService = new UserService(_dbContext, passwordHasherMock.Object, tokenConfig);

			var userLoginCredentials = new UserLoginCredentials {
				Email = "test.user@gmail.com",
				Password = "Incorrect password",
			};

			var userLoginResponse = userService.Login(userLoginCredentials);

			Assert.False(userLoginResponse.IsSuccessful);
			Assert.NotNull(userLoginResponse.Error);
			Assert.Null(userLoginResponse.ResponseResult);
		}

		[Fact]
		public void UserServiceRegister_When_EmailExists_Then_Error()
		{
			var passwordHasherMock = new Mock<IPasswordHasher>();
			var tokenConfig = Microsoft.Extensions.Options.Options.Create(new TokenConfig()
			{
				Issuer = "Issuer",
				JWTLifetime = 60,
				Secret = "Secret auyuiosdyurytasvbdjkast7fvajksdhgyotuasidctd"
			});

			var userService = new UserService(_dbContext, passwordHasherMock.Object, tokenConfig);

			var userRegisterCredentials = new UserRegisterCredentials {
				Email = "test.user@gmail.com",
				Password = "Some password",
				Name = "Some test name"
			};

			var userRegisterResponse = userService.Register(userRegisterCredentials).Result;

			Assert.False(userRegisterResponse.IsSuccessful);
			Assert.NotNull(userRegisterResponse.Error);
			Assert.Null(userRegisterResponse.ResponseResult);
		}

		[Fact]
		public void UserServiceRegister_When_NewUser_Then_GetUser()
		{
			var passwordHasherMock = new Mock<IPasswordHasher>();
			var tokenConfig = Microsoft.Extensions.Options.Options.Create(new TokenConfig()
			{
				Issuer = "Issuer",
				JWTLifetime = 60,
				Secret = "Secret auyuiosdyurytasvbdjkast7fvajksdhgyotuasidctd"
			});

			var userService = new UserService(_dbContext, passwordHasherMock.Object, tokenConfig);

			var userRegisterCredentials = new UserRegisterCredentials {
				Email = Guid.NewGuid() + "test.user@gmail.com",
				Password = "Some password",
				Name = "Some test name"
			};

			var userRegisterResponse = userService.Register(userRegisterCredentials).Result;

			var dbUser = _dbContext.Users.FirstOrDefault(user => user.Email == userRegisterCredentials.Email);

			Assert.True(userRegisterResponse.IsSuccessful);
			Assert.Null(userRegisterResponse.Error);
			Assert.NotNull(userRegisterResponse.ResponseResult.AccessToken);
		}

		[Fact]
		public void UserServiceGetById_When_UserNotExist_Then_Error()
		{
			var tokenConfig = Microsoft.Extensions.Options.Options.Create(new TokenConfig()
			{
				Issuer = "Issuer",
				JWTLifetime = 60,
				Secret = "Secret auyuiosdyurytasvbdjkast7fvajksdhgyotuasidctd"
			});

			var passwordHasherMock = new Mock<IPasswordHasher>();
			var userService = new UserService(_dbContext, passwordHasherMock.Object, tokenConfig);

			var userResponse = userService.GetById(Guid.NewGuid());

			Assert.False(userResponse.IsSuccessful);
			Assert.NotNull(userResponse.Error);
			Assert.Null(userResponse.ResponseResult);
		}

		[Fact]
		public void UserServiceGetById_When_UserExists_Then_GetUser()
		{
			var tokenConfig = Microsoft.Extensions.Options.Options.Create(new TokenConfig()
			{
				Issuer = "Issuer",
				JWTLifetime = 60,
				Secret = "Secret auyuiosdyurytasvbdjkast7fvajksdhgyotuasidctd"
			});

			var passwordHasherMock = new Mock<IPasswordHasher>();
			var userService = new UserService(_dbContext, passwordHasherMock.Object, tokenConfig);

			var userResponse = userService.GetById(Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C"));

			Assert.True(userResponse.IsSuccessful);
			Assert.Null(userResponse.Error);
			Assert.NotNull(userResponse.ResponseResult.Name);
		}
	}
}
