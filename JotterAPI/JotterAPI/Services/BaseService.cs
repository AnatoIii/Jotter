using JotterAPI.DAL;
using JotterAPI.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JotterAPI.Services
{
	public class BaseService
	{
		protected JotterDbContext _dbContext;

		public BaseService(JotterDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public User GetUser(Guid id)
		{
			return _dbContext.Users.FirstOrDefault(user => user.Id == id);
		}
	}
}
