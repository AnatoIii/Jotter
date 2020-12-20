using JotterAPI.DAL;
using JotterAPI.DAL.Model;
using JotterAPI.Helpers;
using JotterAPI.Helpers.Abstractions;
using JotterAPI.Model.DTOs.User;
using JotterAPI.Model.Reponses;
using JotterAPI.Services.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace JotterAPI.Services
{
	public class UserService : BaseService, IUserService
	{
		private readonly IPasswordHasher _passwordHasher;
		private readonly TokenConfig _tokenConfig;
		private readonly SigningCredentials _signingCredentials;

		public UserService(JotterDbContext dbContext, IPasswordHasher passwordHasher, IOptions<TokenConfig> options) : base(dbContext)
		{
			_passwordHasher = passwordHasher;
			_tokenConfig = options.Value;
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfig.Secret));
			_signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
		}

		public Response<TokenResponse> Login(UserLoginCredentials userLoginCredential)
		{
			var userRequest = from u in _dbContext.Users
						where u.Email == userLoginCredential.Email
						select u;
			var user = userRequest.FirstOrDefault();

			if (user == null) {
				return new Response<TokenResponse>("User with such email doesn't exist");
			}
			if (!_passwordHasher.ArePasswordsTheSame(userLoginCredential.Password, user.PasswordSalt, user.Password)) {
				return new Response<TokenResponse>("Incorrect password");
			}
			var token = GenerateAccessToken(user);

			return new Response<TokenResponse>(new TokenResponse(token));
		}

		public async Task<Response<TokenResponse>> Register(UserRegisterCredentials userRegisterCredential)
		{
			var userRequest = from u in _dbContext.Users
							  where u.Email == userRegisterCredential.Email
							  select u;
			if (userRequest.FirstOrDefault() != null) {
				return new Response<TokenResponse>("User with such email already registered");
			}

			var (passwordHash, salt) = _passwordHasher.HashPassword(userRegisterCredential.Password);

			var user = new User {
				Email = userRegisterCredential.Email,
				Password = passwordHash,
				Name = userRegisterCredential.Name,
				PasswordSalt = salt
			};

			_dbContext.Users.Add(user);
			await _dbContext.SaveChangesAsync();
			var token = GenerateAccessToken(user);

			return new Response<TokenResponse>(new TokenResponse(token));
		}

		public Response<UserDataResult> GetById(Guid id)
		{
			var user = GetUser(id);

			return user == null
				? new Response<UserDataResult>("User doesn't exist")
				: new Response<UserDataResult>(new UserDataResult(user));
		}

		private string GenerateAccessToken(User user)
        {
			var claims = new[]
			{
				new Claim(ClaimTypes.NameIdentifier, user.Name),
				new Claim(ClaimTypes.Name, user.Id.ToString())
			};

			var token = new JwtSecurityToken(
				issuer: _tokenConfig.Issuer,
				audience: _tokenConfig.Audience,
				claims: claims,
				expires: DateTime.Now.AddMinutes(_tokenConfig.JWTLifetime),
				signingCredentials: _signingCredentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}
