using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Repositories.Contracts;
using Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class ApplicationSettingManager : IApplicationSettingService
    {
        private const string CacheKeyPrefix = "ApplicationSetting_";
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ApplicationSettingManager> _localizer;
        private readonly ITenantService _tenantService;

        public ApplicationSettingManager(
            IRepositoryManager repositoryManager,
            IMemoryCache memoryCache,
            IMapper mapper,
            IStringLocalizer<ApplicationSettingManager> localizer,
            ITenantService tenantService)
        {
            _repositoryManager = repositoryManager;
            _cache = memoryCache;
            _mapper = mapper;
            _localizer = localizer;
            _tenantService = tenantService;
        }

        public async Task CreateApplicationSettingAsync(ApplicationSettingDtoForInsert dto)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            // Check for duplicate key within the same tenant
            var existing = await _repositoryManager.ApplicationSettingRepository
                .FindByConditionAsync(x => x.Key == dto.Key && x.TenantId == currentTenant.Id,
                                     trackChanges: false);

            if (existing != null)
            {
                throw new ValidationException(
                    string.Format(_localizer["SettingKey{0}AlreadyExists"], dto.Key));
            }

            var applicationSetting = _mapper.Map<ApplicationSetting>(dto);
            applicationSetting.TenantId = currentTenant.Id; // Set tenant ID

            await _repositoryManager.ApplicationSettingRepository.CreateAsync(applicationSetting);
            await _repositoryManager.SaveAsync();
            ClearCache(dto.Key, currentTenant.Id);
        }

        public async Task<IEnumerable<ApplicationSettingDto>> GetAllApplicationSettingsAsync(bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<ApplicationSettingDto>();
            }

            var adminTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            var cacheKey = $"{CacheKeyPrefix}{currentTenant.Id}_All";

            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

                IEnumerable<ApplicationSetting> entities;

                if (currentTenant.Id == adminTenantId)
                {
                    // Admin can see all settings
                    // Remove ToListAsync() since GetAll() returns IQueryable
                    entities = await _repositoryManager.ApplicationSettingRepository
                        .GetAll(trackChanges)
                        .ToListAsync();
                }
                else
                {
                    // Regular tenant sees only their settings
                    // Remove ToListAsync() since GetAllByConditionAsync already returns Task<IEnumerable<T>>
                    entities = await _repositoryManager.ApplicationSettingRepository
                        .GetAllByConditionAsync(x => x.TenantId == currentTenant.Id, trackChanges);
                }

                return _mapper.Map<IEnumerable<ApplicationSettingDto>>(entities);
            });
        }

        public async Task<ApplicationSettingDtoForUpdate> GetApplicationSettingForUpdateAsync(string key, bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var entity = await GetSettingEntityAsync(key, trackChanges);

            // Security check: Ensure the setting belongs to the current tenant (unless admin)
            var adminTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            if (currentTenant.Id != adminTenantId && entity.TenantId != currentTenant.Id)
            {
                throw new ValidationException(_localizer["CannotAccessSettingFromAnotherTenant"]);
            }

            return _mapper.Map<ApplicationSettingDtoForUpdate>(entity);
        }

        public async Task UpdateApplicationSettingAsync(ApplicationSettingDtoForUpdate dto)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            // Get the entity with tenant check
            var entity = await GetSettingEntityAsync(dto.Key, trackChanges: true);

            // Security check
            var adminTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            if (currentTenant.Id != adminTenantId && entity.TenantId != currentTenant.Id)
            {
                throw new ValidationException(_localizer["CannotUpdateSettingFromAnotherTenant"]);
            }

            _mapper.Map(dto, entity);
            entity.LastModified = DateTime.UtcNow;

            await _repositoryManager.SaveAsync();
            ClearCache(dto.Key, entity.TenantId);
        }

        public async Task DeleteApplicationSettingAsync(string key)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var entity = await GetSettingEntityAsync(key, trackChanges: false);

            // Security check
            var adminTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            if (currentTenant.Id != adminTenantId && entity.TenantId != currentTenant.Id)
            {
                throw new ValidationException(_localizer["CannotDeleteSettingFromAnotherTenant"]);
            }

            try
            {
                await _repositoryManager.ApplicationSettingRepository.DeleteAsync(entity);
                await _repositoryManager.SaveAsync();
                ClearCache(key, entity.TenantId);
            }
            catch (Exception exception)
            {
                if (exception.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    throw new ValidationException(
                        _localizer["ApplicationSettingCannotBeDeletedBecauseItIsUsedInAnotherEntity"] + ".",
                        new Exception() { Source = "Model" });
                }
                throw;
            }
        }

        public async Task<ApplicationSetting> GetSettingEntityAsync(string key, bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var adminTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            ApplicationSetting? entity;

            if (currentTenant.Id == adminTenantId)
            {
                // Admin can get any setting by key (first match)
                entity = await _repositoryManager.ApplicationSettingRepository
                    .FindByConditionAsync(x => x.Key == key, trackChanges);
            }
            else
            {
                // Regular tenant can only get their own settings
                entity = await _repositoryManager.ApplicationSettingRepository
                    .FindByConditionAsync(x => x.Key == key && x.TenantId == currentTenant.Id,
                                         trackChanges);
            }

            return entity ?? throw new ValidationException(
                string.Format(_localizer["SettingNotFoundError"], key));
        }

        // Optional: Additional methods for specific tenant queries
        public async Task<ApplicationSetting?> GetSettingByKeyAndTenantAsync(string key, Guid tenantId, bool trackChanges)
        {
            return await _repositoryManager.ApplicationSettingRepository
                .FindByConditionAsync(x => x.Key == key && x.TenantId == tenantId, trackChanges);
        }

        public async Task<IEnumerable<ApplicationSetting>> GetAllSettingsForTenantAsync(Guid tenantId, bool trackChanges)
        {
            return await _repositoryManager.ApplicationSettingRepository
                .GetAllByConditionAsync(x => x.TenantId == tenantId, trackChanges);
        }

        private void ClearCache(string key, Guid tenantId)
        {
            // Clear tenant-specific cache
            _cache.Remove($"{CacheKeyPrefix}{tenantId}_{key}");
            _cache.Remove($"{CacheKeyPrefix}{tenantId}_All");

            // Also clear admin cache if this is an admin tenant
            var adminTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            if (tenantId == adminTenantId)
            {
                _cache.Remove($"{CacheKeyPrefix}All");
            }
        }
    }
}