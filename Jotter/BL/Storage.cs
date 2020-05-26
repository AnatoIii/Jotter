using BL.Response;
using BL.Response.Responses;
using BL.Serializer;
using BL.Serializer.Factory;
using BL.Serializer.Model;
using Model;
using Model.DTO;
using Model.ModelData;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BL
{
	public class Storage : IStorage
	{
		private readonly ISerializer<UserCredentials> _userSerializer;
		private readonly string _userSerializerDataFile;
		private readonly ISerializer<IEnumerable<Note>> _notesSerializer;
		private readonly string _notesSerializerDataFile;
		private readonly ISerializer<IEnumerable<Category>> _categoriesSerializer;
		private readonly string _categoriesSerializerDataFile;

		private UserCredentials _user;
		private List<Category> _categories;
		private List<Note> _notes;

		public Storage(
			ISerializersFactory<UserCredentials> userSerializerFactory,
			ISerializersFactory<IEnumerable<Note>> noteSerializerFactory,
			ISerializersFactory<IEnumerable<Category>> categoriesSerializerFactory
		)
		{
			_userSerializer = userSerializerFactory.GetSerializer(SerializerType.Json);
			_userSerializerDataFile = userSerializerFactory.GetSerializerData(SerializerType.Json);

			_notesSerializer = noteSerializerFactory.GetSerializer(SerializerType.Json);
			_notesSerializerDataFile = noteSerializerFactory.GetSerializerData(SerializerType.Json);

			_categoriesSerializer = categoriesSerializerFactory.GetSerializer(SerializerType.Json);
			_categoriesSerializerDataFile = categoriesSerializerFactory.GetSerializerData(SerializerType.Json);

			_user = _userSerializer.Deserialize(_userSerializerDataFile);
			_notes = _notesSerializer.Deserialize(_notesSerializerDataFile)?.ToList() ?? new List<Note>();
			_categories = _categoriesSerializer.Deserialize(_categoriesSerializerDataFile)?.ToList() ?? new List<Category>();
		}

		#region User

		public bool IsUserLoggedIn()
		{
			return _user != null && _user.User != null;
		}

		public User GetUser()
		{
			return _user.User;
		}

		public Result<LoginResponse> LogIn(UserLoginModel loginData) // Change it!
		{
			_user = new UserCredentials {
				User = new User {
					Password = "123",
					Email = "123@gmail.com",
					Name = "123 123"
				},
				LoggedInTime = DateTime.Now
			};

			SaveUserCredentials();

			return new Result<LoginResponse> {
				IsSuccessful = true,
				Response = new LoginResponse {
					User = _user.User
				}
			};
		}


		public Result<RegisterResponse> Register(UserRegisterModel loginData) // Change it!
		{
			_user = new UserCredentials {
				User = new User {
					Password = "123",
					Email = "123@gmail.com",
					Name = "123 123"
				},
				LoggedInTime = DateTime.Now
			};

			SaveUserCredentials();

			return new Result<RegisterResponse> {
				IsSuccessful = true,
				Response = new RegisterResponse {
					User = _user.User
				}
			};
		}

		#endregion

		#region Notes

		public Result<NoteResponse> GetNotesByCategoryId(Guid categoryId)
		{
			var notes = _notes.Where(note => note.Category.Id == categoryId);


			return new Result<NoteResponse> {
				IsSuccessful = true,
				Response = new NoteResponse {
					Notes = notes
				}
			};
		}

		public Result<NoteResponse> SaveNote(NoteData noteData)
		{
			if (noteData.Id != null) {
				var currentNote = _notes.FirstOrDefault(note => note.Id == noteData.Id);

				currentNote.Name = noteData.Name ?? currentNote.Name;
				currentNote.Description = noteData.Description ?? currentNote.Description;

				return new Result<NoteResponse> {
					IsSuccessful = true,
					Response = new NoteResponse {
					}
				};
			}

			var newNote = new Note { 
				Name = noteData.Name,
				Description = noteData.Description,
				Category = noteData.Category
			};

			_notes.Add(newNote);
			SaveNotes();

			return new Result<NoteResponse> {
				IsSuccessful = true,
				Response = new NoteResponse {
				}
			};
		}

		public Result<NoteResponse> DeleteNote(Guid noteId)
		{
			var noteToDelete = _notes.FirstOrDefault(note => note.Id == noteId);

			_notes.Remove(noteToDelete);
			SaveNotes();

			return new Result<NoteResponse> {
				IsSuccessful = true,
				Response = new NoteResponse {
				}
			};
		}

		#endregion

		#region Categories 

		public Result<CategoryResponse> SaveCategory(Category category)
		{
			var newCategory = new Category {
				Name = category.Name
			};

			_categories.Add(newCategory);
			SaveCategories();

			return new Result<CategoryResponse> {
				IsSuccessful = true,
				Response = new CategoryResponse {
				}
			};
		}

		#endregion

		private void SaveCategories()
		{
			_categoriesSerializer.Serialize(_categories, _categoriesSerializerDataFile);
		}

		private void SaveNotes()
		{
			_notesSerializer.Serialize(_notes, _notesSerializerDataFile);
		}

		private void SaveUserCredentials()
		{
			_userSerializer.Serialize(_user, _userSerializerDataFile);
		}
	}
}
