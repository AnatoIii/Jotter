using JotterAPI.DAL;
using JotterAPI.DAL.Model;
using JotterAPI.Helpers.Abstractions;
using JotterAPI.Model;
using JotterAPI.Model.DTOs.Files;
using JotterAPI.Model.DTOs.Notes;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JotterAPI.Services
{
	public class FileService : BaseService, IFileService
	{
		private readonly IFileServerClient _fileServerClient;
		public FileService(JotterDbContext dbContext, IFileServerClient client) : base(dbContext)
		{
			_fileServerClient = client;
		}

		public async Task<Response<FileResult>> AddFile(FileToSaveData fileToSave, Guid userId)
		{
			var user = GetUser(userId);
			if (user == null) {
				return new Response<FileResult>("Such user doesn't exist");
			}
			var note = _dbContext.Notes
				.Include(note => note.Category)
				.FirstOrDefault(note => note.Id == fileToSave.NoteId && note.Category.UserId == userId);
			if (note == null) {
				return new Response<FileResult>("Such note doesn't exist");
			}

			var fileSaveData = new FileSaveData {
				Base64File = fileToSave.Base64File,
				FileName = fileToSave.FileName
			};
			var path = await _fileServerClient.AddFile(fileSaveData.Base64File, fileSaveData.FileName);

			var file = new File {
				NoteId = fileToSave.NoteId,
				Name = fileToSave.FileName,
				Path = path
			};

			_dbContext.Files.Add(file);
			await _dbContext.SaveChangesAsync();

			return new Response<FileResult>(new FileResult(file));
		}
		
		public async Task<Response<FileDataResult>> GetFileById(Guid fileId, Guid userId)
		{
			var user = GetUser(userId);
			if (user == null) {
				return new Response<FileDataResult>("Such user doesn't exist");
			}

			var file = _dbContext.Files
				.Include(file => file.Note)
					.ThenInclude(note => note.Category)
				.FirstOrDefault(file => file.Id == fileId && file.Note.Category.UserId == userId);

			if (file == null) {
				return new Response<FileDataResult>("Such file doesn't exist");
			}

			var fileData = await _fileServerClient.ReadFile(file.Path);

			return new Response<FileDataResult>(new FileDataResult(file, fileData));
		}

		public async Task<Response<ResponseResult>> DeleteFile(Guid fileId, Guid userId)
		{
			var user = GetUser(userId);
			if (user == null) {
				return new Response<ResponseResult>("Such user doesn't exist");
			}

			var file = _dbContext.Files
				.Include(file => file.Note)
				.FirstOrDefault(file => file.Id == fileId && file.Note.Category.UserId == userId);

			if (file == null) {
				return new Response<ResponseResult>("Such file doesn't exist");
			}

			await _fileServerClient.DeleteFile(file.Path);
			_dbContext.Files.Remove(file);
			await _dbContext.SaveChangesAsync();

			return new Response<ResponseResult>(new ResponseResult());
		}
	}
}
