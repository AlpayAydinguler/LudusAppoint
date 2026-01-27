using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface ITenantRepository : IRepositoryBase<Tenant>
    {
        // Add any custom Tenant queries or commands you need, for example:
        Task<Tenant?> GetTenantByHostnameAsync(string hostname, bool trackChanges = false);
    }
}
