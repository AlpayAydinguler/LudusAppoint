using Entities.Models;

namespace Repositories.Contracts
{
    public interface ITenantRepository : IRepositoryBase<Tenant>
    {
        // Add any custom Tenant queries or commands you need, for example:
        Task<Tenant?> GetTenantByHostnameAsync(string hostname, bool trackChanges = false);
    }
}
