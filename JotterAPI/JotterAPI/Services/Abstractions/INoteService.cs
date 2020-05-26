using JotterAPI.Model.DTOs.Notes;
using JotterAPI.Model.Reponses;
using System.Threading.Tasks;

namespace JotterAPI.Services.Abstractions
{
	public interface INoteService
	{
		Task<Response<NoteResult>> CreateNote(NoteToCreate noteToCreate);
		Task<Response<NoteResult>> EditNote(NoteToEdit noteToEdit);
		Task<Response<ResponseResult>> DeleteNote(NoteId noteId);
		Response<NotesResult> GetByCategory(CategoryData categoryData);
		Response<NoteResult> GetById(NoteId noteId);
	}
}
