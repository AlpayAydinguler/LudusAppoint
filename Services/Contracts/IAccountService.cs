using Entities.Dtos;

namespace Services.Contracts
{
    public interface IAccountService
    {
        Task CreateUserAsync(UserDtoForInsert registerDto);
        Task UpdateUserAsync(UserDtoForUpdate userDtoForUpdate);
    }
}
