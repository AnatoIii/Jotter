using JotterAPI.DAL;
using JotterAPI.DAL.Model;
using JotterAPI.Model.DTOs.User;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace JotterAPI.Services
{
	public class UserService : BaseService, IUserService
	{
		public UserService(JotterDbContext dbContext) : base(dbContext)
		{
		}

		public Response<UserDataResult> Login(UserLoginCredentials userLoginCredential)
		{
			var userRequest = from u in _dbContext.Users
						where u.Email == userLoginCredential.Email
						select u;
			var user = userRequest.FirstOrDefault();

			if (user == null) {
				return new Response<UserDataResult>("User with such email doesn't exist");
			}
			if (user.Password != userLoginCredential.Password) {
				new Response<UserDataResult>("Incorrect password");
			}

			return new Response<UserDataResult>(new UserDataResult(user));
		}

		public async Task<Response<UserDataResult>> Register(UserRegisterCredentials userRegisterCredential)
		{
			var userRequest = from u in _dbContext.Users
							  where u.Email == userRegisterCredential.Email
							  select u;
			if (userRequest.FirstOrDefault() != null) {
				return new Response<UserDataResult>("User with such email already registered");
			}

			var user = new User {
				Email = userRegisterCredential.Email,
				Password = userRegisterCredential.Password,
				Name = userRegisterCredential.Name
			};
			// Here should be password hashing

			_dbContext.Users.Add(user);
			await _dbContext.SaveChangesAsync();

			return new Response<UserDataResult>(new UserDataResult(user));
		}

		public Response<UserDataResult> GetById(Guid id)
		{
			var user = GetUser(id);

			return user == null
				? new Response<UserDataResult>("User doesn't exist")
				: new Response<UserDataResult>(new UserDataResult(user));
		}
	}
}
