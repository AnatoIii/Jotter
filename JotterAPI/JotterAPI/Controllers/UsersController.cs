using JotterAPI.Model.DTOs.User;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace JotterAPI.Controllers
{
	[ApiController]
	[Route("")]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[AllowAnonymous]
		[HttpPost("login")]
		public Response<TokenResponse> Login([FromBody]UserLoginCredentials userLoginCredential)
		{
			return _userService.Login(userLoginCredential);
		}

		[AllowAnonymous]
		[HttpPost("register")]
		public Task<Response<TokenResponse>> Register([FromBody]UserRegisterCredentials userRegisterCredential)
		{
			return _userService.Register(userRegisterCredential);
		}

		[HttpGet("user/{id}")]
		public Response<UserDataResult> GetById([FromRoute] Guid id)
		{
			return _userService.GetById(id);
		}
	}
}
