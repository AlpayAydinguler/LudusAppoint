using Entities.Models;

namespace Repositories.Contracts
{
    public interface IAgeGroupRepository : IRepositoryBase<AgeGroup>
    {
        Task CreateAgeGroupByTenantAsync(AgeGroup ageGroup);
        Task<AgeGroup?> GetAgeGroupByTenantAsync(int id, Guid tenantId, bool trackChanges);
        Task UpdateAgeGroupByTenantAsync(AgeGroup model);
        Task<IEnumerable<AgeGroup>> GetAllAgeGroupsByTenantAsync(Guid tenantId, bool trackChanges);
    }
}
