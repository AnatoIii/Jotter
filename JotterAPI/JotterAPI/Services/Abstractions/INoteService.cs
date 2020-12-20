using JotterAPI.Model.DTOs.Notes;
using JotterAPI.Model.Reponses;
using System;
using System.Threading.Tasks;

namespace JotterAPI.Services.Abstractions
{
	public interface INoteService
	{
		Task<Response<NoteResult>> CreateNote(NoteToCreate noteToCreate, Guid userId);
		Task<Response<NoteResult>> EditNote(NoteToEdit noteToEdit, Guid userId);
		Task<Response<ResponseResult>> DeleteNote(Guid noteId, Guid userId);
		Response<NotesResult> GetByCategory(CategoryData categoryData, Guid userId);
		Response<NoteResult> GetById(Guid noteId, Guid userId);
	}
}
