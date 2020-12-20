using JotterAPI.DAL;
using JotterAPI.DAL.Model;
using JotterAPI.Model.DTOs.Notes;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JotterAPI.Services
{
	public class NotesService : BaseService, INoteService
	{
		public NotesService(JotterDbContext dbContext) : base(dbContext)
		{
		}

		public async Task<Response<NoteResult>> CreateNote(NoteToCreate noteToCreate, Guid userId)
		{
			var user = GetUser(userId);
			if (user == null) {
				return new Response<NoteResult>("User with such Id doesn't exist");
			}

			var category = _dbContext.Categories
				.FirstOrDefault(category => category.UserId == userId && category.Id == noteToCreate.CategoryId);
			if (category == null) {
				return new Response<NoteResult>("User don't have such category");
			}

			var note = new Note {
				Name = noteToCreate.Name,
				Description = noteToCreate.Description,
				CategoryId = noteToCreate.CategoryId
			};

			_dbContext.Notes.Add(note);
			await _dbContext.SaveChangesAsync();

			return new Response<NoteResult>(new NoteResult(note));
		}

		public async Task<Response<NoteResult>> EditNote(NoteToEdit noteToEdit, Guid userId)
		{
			var user = GetUser(userId);
			if (user == null) {
				return new Response<NoteResult>("User with such Id doesn't exist");
			}

			var note = _dbContext.Notes
				.Include(note => note.Files)
				.Include(note => note.Category)
				.FirstOrDefault(note => note.Id == noteToEdit.Id && note.Category.UserId == userId);
			if (note == null) {
				return new Response<NoteResult>("Such note doesn't exist");
			}

			note.Name = noteToEdit.Name ?? note.Name;
			note.Description = noteToEdit.Description ?? note.Description;

			await _dbContext.SaveChangesAsync();

			return new Response<NoteResult>(new NoteResult(note));
		}

		public async Task<Response<ResponseResult>> DeleteNote(Guid noteId, Guid userId)
		{
			var user = GetUser(userId);
			if (user == null) {
				return new Response<ResponseResult>("User with such Id doesn't exist");
			}

			var note = _dbContext.Notes
				.Include(note => note.Category)
				.FirstOrDefault(note => note.Id == noteId && note.Category.UserId == userId);
			if (note == null) {
				return new Response<ResponseResult>("Such note doesn't exist");
			}

			_dbContext.Notes.Remove(note);
			await _dbContext.SaveChangesAsync();

			return new Response<ResponseResult>(new ResponseResult());
		}

		public Response<NotesResult> GetByCategory(CategoryData categoryData, Guid userId)
		{
			var user = GetUser(userId);
			if (user == null) {
				return new Response<NotesResult>("User with such Id doesn't exist");
			}

			var category = _dbContext.Categories
				.FirstOrDefault(category => category.UserId == userId && category.Id == categoryData.CategoryId);
			if (category == null) {
				return new Response<NotesResult>("Such category doesn't exist");
			}
			if (!string.IsNullOrWhiteSpace(category.Password) && category.Password != categoryData.CategoryPassword) { // Password hashing
				return new Response<NotesResult>("Incorrect category password");
			}

			var notes = _dbContext.Notes
				.Include(note => note.Category)
				.Where(note => note.CategoryId == categoryData.CategoryId && note.Category.UserId == userId);
			return new Response<NotesResult>(new NotesResult(notes));
		}

		public Response<NoteResult> GetById(Guid noteId, Guid userId)
		{
			var user = GetUser(userId);
			if (user == null) {
				return new Response<NoteResult>("User with such Id doesn't exist");
			}

			var note = _dbContext.Notes
				.Include(note => note.Files)
				.Include(note => note.Category)
				.FirstOrDefault(note => note.Id == noteId && note.Category.UserId == userId);

			if (note == null) {
				return new Response<NoteResult>("Such note doesn't exist");
			}

			return new Response<NoteResult>(new NoteResult(note));
		}
	}
}
