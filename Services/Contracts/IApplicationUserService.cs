using Entities.Dtos;

namespace Services.Contracts
{
    public interface IApplicationUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDtoForUpdate> GetUserForUpdateAsync(string id);
        Task<string> IsUserActiveAsync(string phoneNumber);
    }
}