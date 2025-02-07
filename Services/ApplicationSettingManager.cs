using Entities.Models;
using Microsoft.Extensions.Caching.Memory;
using Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Repositories.Contracts;
using Entities.Dtos;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Localization;

namespace Services
{
    public class ApplicationSettingManager : IApplicationSettingService
    {
        private const string CacheKeyPrefix = "ApplicationSetting_";
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<ApplicationSettingManager> _localizer;

        public ApplicationSettingManager(IRepositoryManager repositoryManager, IMemoryCache memoryCache, IMapper mapper, IStringLocalizer<ApplicationSettingManager> localizer)
        {
            _repositoryManager = repositoryManager;
            _cache = memoryCache;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task CreateApplicationSettingAsync(ApplicationSettingDtoForInsert applicationSettingDtoForInsert)
        {
            var existing = await _repositoryManager.ApplicationSettingRepository.FindByConditionAsync(x => x.Key == applicationSettingDtoForInsert.Key, trackChanges: false);

            if (existing != null)
            {
                throw new ValidationException(_localizer["SettingKey{0}AlreadyExists", applicationSettingDtoForInsert.Key]);
            }

            var applicationSetting = _mapper.Map<ApplicationSetting>(applicationSettingDtoForInsert);
            _repositoryManager.ApplicationSettingRepository.Create(applicationSetting);
            await _repositoryManager.ApplicationSettingRepository.SaveAsync();
            ClearCache(applicationSettingDtoForInsert.Key);
        }

        public async Task<IEnumerable<ApplicationSettingDto>> GetAllApplicationSettingsAsync(bool trackChanges)
        {
            var cacheKey = $"{CacheKeyPrefix}All";
            return await _cache.GetOrCreateAsync(cacheKey, async entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);
                var entities = await _repositoryManager.ApplicationSettingRepository
                    .GetAll(trackChanges)
                    .ToListAsync();
                return _mapper.Map<IEnumerable<ApplicationSettingDto>>(entities);
            });
        }

        public async Task<ApplicationSettingDtoForUpdate> GetApplicationSettingForUpdateAsync(string key, bool trackChanges)
        {
            var entity = await GetSettingEntityAsync(key, trackChanges: false);
            return _mapper.Map<ApplicationSettingDtoForUpdate>(entity);
        }

        public async Task UpdateApplicationSettingAsync(ApplicationSettingDtoForUpdate applicationSettingDtoForUpdate)
        {
            var entity = await GetSettingEntityAsync(applicationSettingDtoForUpdate.Key, trackChanges: true);
            _mapper.Map(applicationSettingDtoForUpdate, entity);
            await _repositoryManager.ApplicationSettingRepository.SaveAsync();
            ClearCache(applicationSettingDtoForUpdate.Key);
        }

        public async Task DeleteApplicationSettingAsync(string key)
        {
            var entity = await GetSettingEntityAsync(key, trackChanges: false);

            try
            {
                _repositoryManager.ApplicationSettingRepository.Delete(entity);
                await _repositoryManager.ApplicationSettingRepository.SaveAsync();
                ClearCache(key);
            }
            catch (Exception exception)
            {
                if (exception.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    throw new ValidationException(_localizer["ApplicationSettingCannotBeDeletedBecauseItIsUsedInAnotherEntity"] + ".", new Exception() { Source = "Model" });
                }
                throw;
            }
        }
        public async Task<ApplicationSetting> GetSettingEntityAsync(string key, bool trackChanges)
        {
            var entity = await _repositoryManager.ApplicationSettingRepository.FindByConditionAsync(x => x.Key == key, trackChanges);

            return entity ?? throw new ValidationException(
                _localizer["SettingNotFoundError", key]
            );
        }

        private void ClearCache(string key)
        {
            _cache.Remove($"{CacheKeyPrefix}{key}");
            _cache.Remove($"{CacheKeyPrefix}All");
        }
    }
}
