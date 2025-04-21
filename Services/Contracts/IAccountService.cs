using Entities.Dtos;

namespace Services.Contracts
{
    public interface IAccountService
    {
        Task CreateUserAsync(UserDtoForInsert registerDto);
        Task DeleteUserAsync(string id);
        Task UpdateUserAsync(UserDtoForUpdate userDtoForUpdate);
    }
}
