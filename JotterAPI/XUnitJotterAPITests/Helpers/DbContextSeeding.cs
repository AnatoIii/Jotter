using JotterAPI.DAL;
using JotterAPI.DAL.Model;
using System;

namespace XUnitJotterAPITests.Helpers
{
	public static class DbContextSeeding
	{

		public static void Seed(this JotterDbContext jotterTestDbContext)
		{
			jotterTestDbContext.Users.Add(new User {
				Id = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C"),
				Email = "test.user@gmail.com",
				Password = "12345678",
				Name = "Test User"
			});

			jotterTestDbContext.Categories.Add(new Category {
				Id = Guid.Parse("F9A27BE9-3771-4AAE-89C8-444D4D5F828F"),
				Name = "Test category 1",
				UserId = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C")
			});

			jotterTestDbContext.Categories.Add(new Category {
				Id = Guid.Parse("2BF07B99-D5EA-4E9E-A19B-0BA108F5B65D"),
				Name = "Test category 2",
				UserId = Guid.Parse("8273A004-371D-48A5-B7DD-02145B8E4E3C")
			});

			jotterTestDbContext.Notes.Add(new Note {
				Id = Guid.Parse("D45C7AA0-1296-42D6-AE5D-3BF5D943E703"),
				CategoryId = Guid.Parse("2BF07B99-D5EA-4E9E-A19B-0BA108F5B65D"),
				Description = "First test note",
				Name = "Note 1"
			});
			jotterTestDbContext.Notes.Add(new Note {
				Id = Guid.Parse("2E20F240-ADD6-496F-A2BD-794043D94940"),
				CategoryId = Guid.Parse("2BF07B99-D5EA-4E9E-A19B-0BA108F5B65D"),
				Description = "Second test note",
				Name = "Note 2"
			});
			jotterTestDbContext.Notes.Add(new Note {
				Id = Guid.Parse("7493A1FA-BADE-458F-9784-B160C17873C6"),
				CategoryId = Guid.Parse("2BF07B99-D5EA-4E9E-A19B-0BA108F5B65D"),
				Description = "Third test note",
				Name = "Note 3"
			});

			jotterTestDbContext.Files.Add(new File {
				Id = Guid.Parse("E18B9E36-CC1A-4A7A-B996-B6FAD1D86232"),
				Name = "First test File",
				NoteId = Guid.Parse("7493A1FA-BADE-458F-9784-B160C17873C6"),
				Path = "path/to/test/file/1"
			});
			jotterTestDbContext.Files.Add(new File {
				Id = Guid.Parse("3BA7C149-0039-4A2C-B401-01AC0BF3FD14"),
				Name = "Second test File",
				NoteId = Guid.Parse("7493A1FA-BADE-458F-9784-B160C17873C6"),
				Path = "path/to/test/file/2"
			});

			jotterTestDbContext.SaveChanges();
		}
	}
}
