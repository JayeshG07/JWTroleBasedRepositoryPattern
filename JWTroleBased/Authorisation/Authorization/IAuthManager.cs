using JWTroleBased.Dto;

namespace JWTroleBased.Authorisation.Authorization
{
    public interface IAuthManager
    {
        Task<bool> RegisterUser(RegisterUserDto userDto);
        Task<string> Login(LoginDto loginDto);
    }
}
