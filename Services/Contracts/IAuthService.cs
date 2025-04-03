using Entities.Dtos;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string phoneNumber, string password);
        Task<bool> LogoutAsync();
        Task<IEnumerable<RoleDto>> GetRolesAsync();
        Task CreateRoleAsync(RoleDtoForInsert roleDtoForInsert);
        Task<RoleDtoForUpdate> GetRoleByIdAsync(string id);
        Task UpdateRoleAsync(RoleDtoForUpdate roleDtoForInsert);
        Task DeleteRoleAsync(string id);
    }
}
