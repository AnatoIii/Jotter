using JotterAPI.DAL.Model;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace JotterAPI.DAL
{
	public class JotterDbContext : DbContext
	{
		public JotterDbContext(DbContextOptions options) : base(options) { }

		public DbSet<User> Users { get; set; }

		public DbSet<Category> Categories { get; set; }

		public DbSet<File> Files { get; set; }

		public DbSet<Note> Notes { get; set; }
	}
}
