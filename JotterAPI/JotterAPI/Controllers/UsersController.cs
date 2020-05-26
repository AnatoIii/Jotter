using JotterAPI.Model.DTOs.User;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JotterAPI.Controllers
{
	[ApiController]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpPost("login")]
		public Response<UserDataResult> Login(UserLoginCredentials userLoginCredential)
		{
			return _userService.Login(userLoginCredential);
		}

		[HttpPost("register")]
		public Task<Response<UserDataResult>> Register(UserRegisterCredentials userRegisterCredential)
		{
			return _userService.Register(userRegisterCredential);
		}

		[HttpGet("user")]
		public Response<UserDataResult> GetById(Guid id)
		{
			return _userService.GetById(id);
		}
	}
}
