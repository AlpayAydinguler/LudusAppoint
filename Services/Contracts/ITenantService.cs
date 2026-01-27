using Entities.Dtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface ITenantService
    {
        Task CreateTenantAsync(TenantDtoForInsert tenantDto);
        Task DeleteTenantAsync(Guid id);
        Task UpdateTenantAsync(TenantDtoForUpdate tenantDto);
        Task<TenantDto?> GetTenantByIdAsync(Guid id);
        Task<Tenant?> GetTenantByHostnameAsync(string hostname);
        Task<IEnumerable<TenantDto>> GetAllTenantsAsync(bool trackChanges = false);
        Task<Tenant?> GetCurrentTenantAsync();
    }
}
