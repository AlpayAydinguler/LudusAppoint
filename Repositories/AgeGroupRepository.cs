using Entities.Models;
using Repositories.Contracts;

namespace Repositories
{
    public class AgeGroupRepository : RepositoryBase<AgeGroup>, IAgeGroupRepository
    {
        public AgeGroupRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateAgeGroupByTenantAsync(AgeGroup ageGroup)
            => await CreateAsync(ageGroup);

        public async Task<AgeGroup?> GetAgeGroupByTenantAsync(int id, Guid tenantId, bool trackChanges)
            => await FindByConditionAsync(x => x.AgeGroupId.Equals(id) && x.TenantId == tenantId, trackChanges);

        public async Task UpdateAgeGroupByTenantAsync(AgeGroup model)
        {
            Update(model);
            await SaveAsync();
        }

        public async Task<IEnumerable<AgeGroup>> GetAllAgeGroupsByTenantAsync(Guid tenantId, bool trackChanges)
            => await GetAllByConditionAsync(x => x.TenantId == tenantId, trackChanges);
    }
}
