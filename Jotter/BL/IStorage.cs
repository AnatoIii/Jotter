using BL.Response;
using BL.Response.Responses;
using Model;
using Model.DTO;
using System;

namespace BL
{
	public interface IStorage
	{
		bool IsUserLoggedIn();
		User GetUser();
		Result<RegisterResponse> Register(UserRegisterModel loginData);
		Result<LoginResponse> LogIn(UserLoginModel loginData);

		Result<CategoryResponse> SaveCategory(Category category);
		Result<CategoriesResponse> GetCategories();

		Result<NotesResponse> GetNotesByCategoryId(Guid categoryId);
		Result<NoteResponse> SaveNote(NoteData noteData);
		Result<NoteResponse> DeleteNote(Guid noteId);
	}
}
