using JotterAPI.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace JotterAPI.DAL
{
	public class JotterDbContext : DbContext
	{
		public JotterDbContext(DbContextOptions options) : base(options) 
		{
			if (Database.GetPendingMigrations().Count() > 0) {
				Database.Migrate();
			}
		}

		public DbSet<User> Users { get; set; }

		public DbSet<Category> Categories { get; set; }

		public DbSet<File> Files { get; set; }

		public DbSet<Note> Notes { get; set; }
	}
}
