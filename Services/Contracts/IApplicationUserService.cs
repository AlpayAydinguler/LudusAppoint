using Entities.Dtos;
using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IApplicationUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDtoForUpdate> GetUserForUpdateAsync(string id);
        Task<string> IsUserActiveAsync(string phoneNumber);
    }
}