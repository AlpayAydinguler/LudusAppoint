using Entities.Dtos;
using Entities.Models;

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
