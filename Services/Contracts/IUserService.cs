using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDtoForUpdate> GetUserForUpdateAsync(string id);
        Task<string> IsUserActive(string phoneNumber);
    }
}
