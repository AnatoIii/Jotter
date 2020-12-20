using JotterAPI.Model.DTOs.Notes;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

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

		[HttpPost]
		public Task<Response<NoteResult>> CreateNote([FromBody]NoteToCreate noteToCreate)
		{
			return _noteService.CreateNote(noteToCreate, GetUserId());
		}

		[HttpPut]
		public Task<Response<NoteResult>> EditNote([FromBody]NoteToEdit noteToEdit)
		{
			return _noteService.EditNote(noteToEdit, GetUserId());
		}
		
		[HttpDelete("{noteId}")]
		public Task<Response<ResponseResult>> DeleteNote(Guid noteId)
		{
			return _noteService.DeleteNote(noteId, GetUserId());
		}
		
		[HttpGet("category")]
		public Response<NotesResult> GetByCategory([FromQuery]CategoryData categoryData)
		{
			return _noteService.GetByCategory(categoryData, GetUserId());
		}
		
		[HttpGet("{noteId}")]
		public Response<NoteResult> GetById(Guid noteId)
		{
			return _noteService.GetById(noteId, GetUserId());
		}
		
		private Guid GetUserId()
		{
			return Guid.Parse(User.Identity.Name);
		}
	}
}
