using JWTroleBased.Dto;
using JWTroleBased.Models;

namespace JWTroleBased.Repository
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleDto>> GetRolesAsync();
        Task<bool> CreateRoleAsync(RoleDto role);
        Task<bool> DeleteRoleAsync(int id);
    }

}
