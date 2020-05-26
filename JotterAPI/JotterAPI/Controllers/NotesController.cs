using JotterAPI.Model.DTOs.Notes;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
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
			return _noteService.CreateNote(noteToCreate);
		}

		[HttpPut]
		public Task<Response<NoteResult>> EditNote([FromBody]NoteToEdit noteToEdit)
		{
			return _noteService.EditNote(noteToEdit);
		}
		
		[HttpDelete]
		public Task<Response<ResponseResult>> DeleteNote([FromQuery]NoteId noteId)
		{
			return _noteService.DeleteNote(noteId);
		}
		
		[HttpGet("category")]
		public Response<NotesResult> GetByCategory([FromQuery]CategoryData categoryData)
		{
			return _noteService.GetByCategory(categoryData);
		}
		
		[HttpGet]
		public Response<NoteResult> GetById([FromQuery]NoteId noteId)
		{
			return _noteService.GetById(noteId);
		}
	}
}
