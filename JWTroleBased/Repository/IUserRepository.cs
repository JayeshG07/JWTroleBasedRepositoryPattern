using JWTroleBased.Dto;

namespace JWTroleBased.Repository
{
    public interface IUserRepository
    {
        public Task<UserDto> UpdateUserAsync(UserDto userDto);
    }
}
