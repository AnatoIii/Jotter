using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace JotterAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}
	}
}
