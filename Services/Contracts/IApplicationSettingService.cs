using Entities.Dtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    }
}
