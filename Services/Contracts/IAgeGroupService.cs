using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IAgeGroupService
    {
        Task<IEnumerable<AgeGroupDto>> GetAllAgeGroupsAsync(bool trackChanges);
        Task<AgeGroup?> GetAgeGroupAsync(int id, bool trackChanges);
        Task CreateAgeGroupAsync(AgeGroupDtoForInsert ageGroupDtoForInsert);
        Task UpdateAgeGroupAsync(AgeGroupDtoForUpdate ageGroupDtoForUpdate);
        Task<AgeGroupDtoForUpdate> GetAgeGroupForUpdateAsync(int id, bool trackChanges);
        Task DeleteAgeGroupAsync(int id);
    }
}
