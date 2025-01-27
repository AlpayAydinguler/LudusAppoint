using Entities.Models;

namespace Repositories.Contracts
{
    public interface IAgeGroupRepository : IRepositoryBase<AgeGroup>
    {
        void CreateAgeGroup(AgeGroup ageGroup);
        AgeGroup GetAgeGroup(int id, bool trackChanges);
        void UpdateAgeGroup(AgeGroup model);
    }
}
