using Entities.Models;
using Repositories.Contracts;

namespace Repositories
{
    public class TenantRepository : RepositoryBase<Tenant>, ITenantRepository
    {
        public TenantRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<Tenant?> GetTenantByHostnameAsync(string hostname, bool trackChanges = false)
        {
            return await FindByConditionAsync(t => t.Hostname == hostname, trackChanges);
        }
    }
}
