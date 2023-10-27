using AutoMapper;
using JWTroleBased.Authorization.AuthConfig;
using DGMarket.Authorization.Configuration;
using JWTroleBased.Context;
using JWTroleBased.Dto;
using JWTroleBased.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace JWTroleBased.Authorisation.Authorization
{
    public class AuthManager : IAuthManager
    {
        readonly IConfiguration _configuration;
        readonly AppDbContext _dbContext;
        readonly IMapper _mapper;

        public AuthManager(IConfiguration configuration, AppDbContext appDbContext, IMapper mapper)
        {
            _configuration = configuration;
            _dbContext = appDbContext;
            _mapper = mapper;
        }

        public async Task<string> Login(LoginDto loginDto)
        {
            var user = _mapper.Map<User>(loginDto);
            var response = await GetUserByEmail(user.Email);

            if (response == null || !PasswordHasher.VerifyPassword(loginDto.Password, response.Password))
            {
                return null;
            }

            var token = GenerateToken(response);
            //return new AuthResponse
            //{
            //    UserId = response.UserId,
            //    Token = token,
            //}; 
            return token;
        }

        private string GenerateToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTSettings:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var role = GetUserRole(user).Result;
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Name),
                new Claim("UserId", user.UserId.ToString()),
                new Claim(ClaimTypes.Name,user.Name.ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(ClaimTypes.Role, role.ToString()),
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["JWTSettings:Issuer"],
                audience: _configuration["JWTSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt64(_configuration["JWTSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private async Task<string> GetUserRole(User user)
        {
            var role = await _dbContext.Roles.FirstOrDefaultAsync(r => r.RoleId == user.RoleId);

            if (role != null)
            {
                return role.Name;
            }
            else
            {
                return null;
            }
        }


        private async Task<User> GetUserByEmail(string email)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> RegisterUser(RegisterUserDto usersDto)
        {

            string passwordHash = PasswordHasher.HashPassword(usersDto.Password);
            usersDto.Password = passwordHash;
            var user = _mapper.Map<User>(usersDto);
            var result = await CreateUser(user);
            if (result)
            {
                return true;
            }

            return false;
        }

        private async Task<bool> CreateUser(User user)
        {
            _dbContext.Users.Add(user);
            var result = await _dbContext.SaveChangesAsync();
            if (result > 0)
            {
                return true;
            }
            return false;
        }
    }
}
