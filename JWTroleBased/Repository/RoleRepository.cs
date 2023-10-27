using AutoMapper;
using JWTroleBased.Context;
using JWTroleBased.Dto;
using JWTroleBased.Models;
using Microsoft.EntityFrameworkCore;

namespace JWTroleBased.Repository
{
    public class RoleRepository : IRoleRepository
    {
        readonly AppDbContext _context;
        readonly IMapper _mapper;
        public RoleRepository(AppDbContext appDbContext, IMapper mapper)
        {
            _context = appDbContext;
            _mapper = mapper;

        }
        public async Task<bool> CreateRoleAsync(RoleDto role)
        {
            var input = _mapper.Map<Role>(role);
            await _context.Roles.AddAsync(input);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {

                return true;
            }
            else
            {
                return false;
            }
        }


        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role!=null)
            {
                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }



        public async Task<IEnumerable<RoleDto>> GetRolesAsync()
        {
            var roles = await _context.Roles.ToListAsync();
            var roleDtos = _mapper.Map<IEnumerable<RoleDto>>(roles);
            return roleDtos;

        }

      
    }
}
