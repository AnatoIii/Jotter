using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace JotterAPI.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class NotesController : ControllerBase
	{
		private readonly INoteService _noteService;

		public NotesController(INoteService noteService)
		{
			_noteService = noteService;
		}
	}
}
