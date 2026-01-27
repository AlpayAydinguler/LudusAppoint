using Entities.Models;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
