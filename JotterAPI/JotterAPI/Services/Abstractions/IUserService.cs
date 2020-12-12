using JotterAPI.Model.DTOs.User;
using JotterAPI.Model.Reponses;
using System;
using System.Threading.Tasks;

namespace JotterAPI.Services.Abstractions
{
	public interface IUserService
	{
		Response<TokenResponse> Login(UserLoginCredentials userLoginCredential);
		Task<Response<TokenResponse>> Register(UserRegisterCredentials userRegisterCredential);
		Response<UserDataResult> GetById(Guid id);
	}
}
