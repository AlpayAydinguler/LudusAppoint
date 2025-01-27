using Entities.Models;
using Repositories.Contracts;

namespace Repositories
{
    public class AgeGroupRepository : RepositoryBase<AgeGroup>, IAgeGroupRepository
    {
        public AgeGroupRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateAgeGroup(AgeGroup ageGroup)
        {
            Create(ageGroup);
        }

        public AgeGroup GetAgeGroup(int id, bool trackChanges)
        {
            return FindByCondition(x => x.AgeGroupId.Equals(id), trackChanges) ?? new AgeGroup();
        }

        public void UpdateAgeGroup(AgeGroup model)
        {
            Update(model);
        }
    }
}
