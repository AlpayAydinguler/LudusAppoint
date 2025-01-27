using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IAgeGroupService
    {
        IEnumerable<AgeGroupDto> GetAllAgeGroups(bool trackChanges);
        AgeGroup? GetAgeGroup(int id, bool trackChanges);
        void CreateAgeGroup(AgeGroupDtoForInsert ageGroupDtoForInsert);
        void UpdateAgeGroup(AgeGroupDtoForUpdate ageGroupDtoForUpdate);
        AgeGroupDtoForUpdate GetAgeGroupForUpdate(int id, bool trackChanges);
        void DeleteAgeGroup(int id);
    }
}
