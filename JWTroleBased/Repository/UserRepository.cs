using JWTroleBased.Context;
using JWTroleBased.Dto;
using Microsoft.EntityFrameworkCore;

namespace JWTroleBased.Repository
{
    public class UserRepository:IUserRepository
    {
        readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> UpdateUserAsync(UserDto userDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == userDto.UserId);

            if (user == null)
            {
                return null; // User not found
            }

            // Update user properties with values from userDto
            user.Name = userDto.Name;
            user.Email = userDto.Email;
            user.Password = userDto.Password;
            user.RoleId = userDto.RoleId;

            //_context.Entry(user).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return userDto;
        }
    }
}
