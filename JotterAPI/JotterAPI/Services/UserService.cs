using JotterAPI.DAL;
using JotterAPI.Services.Abstractions;

namespace JotterAPI.Services
{
	public class UserService : BaseService, IUserService
	{
		public UserService(JotterDbContext dbContext) : base(dbContext)
		{
		}
	}
}
