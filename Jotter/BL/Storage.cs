using BL.Errors;
using BL.Helpers;
using BL.Response;
using BL.Response.Responses;
using BL.Serializer;
using BL.Serializer.Factory;
using BL.Serializer.Model;
using Model;
using Model.DTO;
using Model.ModelData;
using Model.Responses;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
	public class Storage : IStorage
	{
		private readonly string _defaultFilePath;

		private readonly ISerializer<UserCredentials> _userSerializer;
		private readonly string _userSerializerDataFile;
		private readonly ISerializer<IEnumerable<Note>> _notesSerializer;
		private readonly string _notesSerializerDataFile;
		private readonly ISerializer<IEnumerable<Category>> _categoriesSerializer;
		private readonly string _categoriesSerializerDataFile;
		private readonly JotterHttpClient _jotterHttpClient;

		private UserCredentials _user;
		private List<Category> _categories;
		private readonly List<Note> _notes;

		string lastFile; // DELETE IT!

		public Storage(
			ISerializersFactory<UserCredentials> userSerializerFactory,
			ISerializersFactory<IEnumerable<Note>> noteSerializerFactory,
			ISerializersFactory<IEnumerable<Category>> categoriesSerializerFactory,
			JotterHttpClient jotterHttpClient,
			string defaultFilePath
		)
		{
			if (!Directory.Exists(defaultFilePath)) {
				Directory.CreateDirectory(defaultFilePath);
			}
			_defaultFilePath = defaultFilePath;
			_jotterHttpClient = jotterHttpClient;

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

		public async Task<Response<UserDataResult>> LogIn(UserLoginModel loginData)
		{
			var userLoginCredentials = new UserLoginModel {
				Email = loginData.Email,
				Password = loginData.Password
			};

			var response  = await _jotterHttpClient.Post<Response<UserDataResult>, UserLoginModel>("login", userLoginCredentials);

			if (!response.IsSuccessful) {
				return response;
			}

			_user = new UserCredentials {
				User = new User {
					Id = response.ResponseResult.Id,
					Email = response.ResponseResult.Email,
					Name = response.ResponseResult.Name
				},
				LoggedInTime = DateTime.Now
			};

			SaveUserCredentials();

			return response;
		}


		public async Task<Response<UserDataResult>> Register(UserRegisterModel registerData) // Change it!
		{
			var userRegisterData = new UserRegisterModel {
				Email = registerData.Email,
				Name = registerData.Name,
				Password = registerData.Password
			};			

			var response = await _jotterHttpClient.Post<Response<UserDataResult>, UserRegisterModel>("register", userRegisterData);

			if (!response.IsSuccessful) {
				return response;
			}

			_user = new UserCredentials {
				User = new User {
					Id = response.ResponseResult.Id,
					Email = response.ResponseResult.Email,
					Name = response.ResponseResult.Name
				},
				LoggedInTime = DateTime.Now
			};

			SaveUserCredentials();

			return response;
		}

		#endregion

		#region Notes

		public async Task<Result<NotesResponse>> GetNotesByCategory(Guid categoryId, string password = null)
		{

			var categoryData = new CategoryData {
				CategoryId = categoryId,
				CategoryPassword = password,
				UserId = _user.User.Id
			};

			try {
				var response = await _jotterHttpClient.Get<Response<NotesResult>>($"notes/category?CategoryId={categoryId}&UserId={_user.User.Id}&CategoryPassword={password}");
				if (!response.IsSuccessful) {
					return new Result<NotesResponse> {
						IsSuccessful = response.IsSuccessful,
						ErrorMessage = response.Error
					};
				}
				var notes = response.ResponseResult.Notes.Select(note => new Note {
					Id = note.Id,
					Name = note.Name,
					Description = note.Description
				});

				_notes.AddRange(notes);

				SaveNotes();

				return new Result<NotesResponse> {
					IsSuccessful = true,
					Response = new NotesResponse {
						Notes = notes
					}
				};
			} catch (HttpException) {
				var notes = _notes.Where(note => note.CategoryId == categoryId);

				return new Result<NotesResponse> {
					IsSuccessful = true,
					Response = new NotesResponse {
						Notes = notes
					}
				};
			}
		}

		public async Task<Result<NoteResponse>> SaveNote(NoteData noteData)
		{
			if (noteData.Id != new Guid()) {
				var currentNote = _notes.FirstOrDefault(note => note.Id == noteData.Id);

				currentNote.Name = noteData.Name ?? currentNote.Name;
				currentNote.Description = noteData.Description ?? currentNote.Description;
				SaveNotes();

				var noteForEditing = new NoteToEdit {
					Description = currentNote.Description,
					Id = currentNote.Id,
					Name = currentNote.Name,
					UserId = _user.User.Id
				};

				var response = await _jotterHttpClient.Put<Response<NoteResult>, NoteToEdit>("notes", noteForEditing);
				return new Result<NoteResponse> {
					ErrorMessage = response.Error,
					IsSuccessful = response.IsSuccessful,
					Response = new NoteResponse {
						Note = new Note {
							Id = response.ResponseResult.Id,
							Name = response.ResponseResult.Name,
							Description = response.ResponseResult.Description
						}
					}
				};
			}

			var newNote = new NoteToCreate { 
				UserId = _user.User.Id,
				Name = noteData.Name,
				Description = noteData.Description,
				CategoryId = noteData.CategoryId
			};

			var newNoteResponse = await _jotterHttpClient.Post<Response<NoteResult>, NoteToCreate>("notes", newNote);
			if (!newNoteResponse.IsSuccessful) {
				return new Result<NoteResponse> {
					IsSuccessful = newNoteResponse.IsSuccessful,
					ErrorMessage = newNoteResponse.Error
				};
			}

			var note = new Note {
				Id = newNoteResponse.ResponseResult.Id,
				Name = newNoteResponse.ResponseResult.Name,
				Description = newNoteResponse.ResponseResult.Description
			};

			_notes.Add(note);
			SaveNotes();

			return new Result<NoteResponse> {
				IsSuccessful = true,
				Response = new NoteResponse {
					Note = note
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


		public async Task<Result<NoteResponse>> GetNoteById(Guid noteId)
		{
			try {
				var response = await _jotterHttpClient.Get<Response<NoteResult>>($"notes?Id={noteId}&UserId={_user.User.Id}");
				if (!response.IsSuccessful) {
					return new Result<NoteResponse> {
						IsSuccessful = response.IsSuccessful,
						ErrorMessage = response.Error
					};
				}

				var noteData = response.ResponseResult;

				return new Result<NoteResponse> {
					IsSuccessful = true,
					Response = new NoteResponse {
						Note = new Note {
							Id = noteData.Id,
							Name = noteData.Name,
							Description = noteData.Description,
							Files = noteData.Files.Select(file => new Model.File {
								Id = file.Id,
								Name = file.Name
							}).ToList()
						}
					}
				};
			} catch (HttpException e) {
				return new Result<NoteResponse> {
					IsSuccessful = false,
					ErrorMessage = e.Message
				};
			}
		}

		#endregion

		#region Categories 

		public async Task<Result<CategoryResponse>> SaveCategory(Category category)
		{
			try {
				var newCategory = new NewCategory {
					Name = category.Name,
					Password = category.Password,
					UserId = _user.User.Id
				};

				var response = await _jotterHttpClient.Post<Response<CategoryResult>, NewCategory>("categories", newCategory);
				if (!response.IsSuccessful) {
					return new Result<CategoryResponse> {
						IsSuccessful = response.IsSuccessful,
						ErrorMessage = response.Error
					};
				}

				var createdCategory = new Category {
					Id = response.ResponseResult.Id,
					IsPrivate = response.ResponseResult.IsLocked,
					Name = response.ResponseResult.Name
				};

				_categories.Add(createdCategory);
				SaveCategories();

				return new Result<CategoryResponse> {
					IsSuccessful = response.IsSuccessful,
					Response = new CategoryResponse {
						Category = createdCategory
					}
				};
			} catch (HttpException e) {
				return new Result<CategoryResponse> {
					IsSuccessful = false,
					ErrorMessage = e.Message
				};
			}
		}

		public async Task<Result<CategoriesResponse>> GetCategories()
		{
			try {
				var response = await _jotterHttpClient.Get<Response<CategoriesResult>>($"categories/{_user.User.Id}");
				if (!response.IsSuccessful) {
					return new Result<CategoriesResponse> {
						IsSuccessful = response.IsSuccessful,
						ErrorMessage = response.Error
					};
				}

				_categories = response.ResponseResult.Categories.Select(c => new Category { Id = c.Id, IsPrivate = c.IsLocked, Name = c.Name }).ToList();
				SaveCategories();

				return new Result<CategoriesResponse> {
					IsSuccessful = response.IsSuccessful,
					Response = new CategoriesResponse {
						Categories = _categories
					}
				}; 
			} catch (HttpException e) {
				return new Result<CategoriesResponse> {
					IsSuccessful = true,
					ErrorMessage = e.Message,
					Response = new CategoriesResponse {
						Categories = _categories
					}
				};
			}

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

		public async Task<Result<EmptyResponse>> SaveFile(string path, Guid noteId)
		{
			var bytes = System.IO.File.ReadAllBytes(path);
			var base64string = Convert.ToBase64String(bytes);
			lastFile = base64string;
			var file = new FileToSaveData() {
				Base64File = base64string,
				FileName = GetFileNameFromPath(path),
				NoteId = noteId,
				UserId = _user.User.Id
			};

			var response = await _jotterHttpClient.Post<Response<FileResult>, FileToSaveData>("files", file);

			return new Result<EmptyResponse> {
				ErrorMessage = response.Error,
				IsSuccessful = response.IsSuccessful
			};
		}

		public Task<Result<EmptyResponse>> DeleteFile(Guid FileId)
		{
			throw new NotImplementedException();
		}

		private string GetFileNameFromPath(string path)
		{
			return path.Substring(path.LastIndexOf("\\") + 1);
		}

		private string GetFileExtension(string fileName)
		{
			return fileName.Substring(fileName.LastIndexOf('.') + 1);
		}

		public async Task<Result<EmptyResponse>> OpenFile(Guid fileId)
		{
			var filePath = Directory.GetFiles(_defaultFilePath).FirstOrDefault(file => file.Contains(fileId.ToString()));
			if (filePath != null) {
				return TryStartProcess(filePath); 
			}

			var fileResponse = await _jotterHttpClient.Get<Response<FileDataResult>>($"files?userId={_user.User.Id}&fileId={fileId}");
			if (!fileResponse.IsSuccessful) {
				return new Result<EmptyResponse> {
					IsSuccessful = false,
					ErrorMessage = fileResponse.Error
				};
			}

			var fileData = fileResponse.ResponseResult;

			var path = $"{_defaultFilePath}\\{fileId}.{GetFileExtension(fileData.FileName)}";
			
			var bytes = Convert.FromBase64String(fileData.Base64File);
			System.IO.File.WriteAllBytes(path, bytes);

			return TryStartProcess(path);
		}

		private Result<EmptyResponse> TryStartProcess(string path)
		{
			try {
				System.Diagnostics.Process.Start("explorer.exe", path);
			} catch (Exception e) {
				return new Result<EmptyResponse> {
					IsSuccessful = false,
					ErrorMessage = e.Message
				};
			}
			return new Result<EmptyResponse> {
				IsSuccessful = true
			};
		}
	}
}
