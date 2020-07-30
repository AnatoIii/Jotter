using BL.Response;
using BL.Response.Responses;
using Model;
using Model.DTO;
using Model.Responses;
using System;
using System.Threading.Tasks;

namespace BL
{
	public interface IStorage
	{
		bool IsUserLoggedIn();
		User GetUser();
		Task<Response<UserDataResult>> Register(UserRegisterModel loginData);
		Task<Response<UserDataResult>> LogIn(UserLoginModel loginData);

		Task<Result<CategoryResponse>> SaveCategory(Category category);
		Task<Result<CategoriesResponse>> GetCategories();

		Task<Result<NotesResponse>> GetNotesByCategory(Guid categoryId, string categoryPassword = null);
		Task<Result<NoteResponse>> SaveNote(NoteData noteData);
		Task<Result<NoteResponse>> GetNoteById(Guid noteId);
		Result<NoteResponse> DeleteNote(Guid noteId);

		Task<Result<EmptyResponse>> SaveFile(string path, Guid noteId);
		Task<Result<EmptyResponse>> OpenFile(Guid fileId);
		Task<Result<EmptyResponse>> DeleteFile(Guid FileId);
	}
}
