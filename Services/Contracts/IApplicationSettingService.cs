using Entities.Dtos;
using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IApplicationSettingService
    {
        Task CreateApplicationSettingAsync(ApplicationSettingDtoForInsert dto);
        Task<IEnumerable<ApplicationSettingDto>> GetAllApplicationSettingsAsync(bool trackChanges);
        Task<ApplicationSettingDtoForUpdate> GetApplicationSettingForUpdateAsync(string key, bool trackChanges);
        Task UpdateApplicationSettingAsync(ApplicationSettingDtoForUpdate dto);
        Task DeleteApplicationSettingAsync(string key);
        Task<ApplicationSetting> GetSettingEntityAsync(string key, bool trackChanges);

        // Optional: Add tenant-specific methods
        Task<ApplicationSetting?> GetSettingByKeyAndTenantAsync(string key, Guid tenantId, bool trackChanges);
        Task<IEnumerable<ApplicationSetting>> GetAllSettingsForTenantAsync(Guid tenantId, bool trackChanges);
    }
}