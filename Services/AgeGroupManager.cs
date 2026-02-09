using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Localization;
using Repositories.Contracts;
using Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class AgeGroupManager : IAgeGroupService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IStringLocalizer<AgeGroupManager> _localizer;
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService;

        public AgeGroupManager(
            IRepositoryManager repositoryManager,
            IStringLocalizer<AgeGroupManager> localizer,
            IMapper mapper,
            ITenantService tenantService)
        {
            _repositoryManager = repositoryManager;
            _localizer = localizer;
            _mapper = mapper;
            _tenantService = tenantService;
        }

        public async Task CreateAgeGroupAsync(AgeGroupDtoForInsert ageGroupDtoForInsert)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            await ValidateAgeGroupAsync(ageGroupDtoForInsert, null, currentTenant.Id);

            var ageGroup = _mapper.Map<AgeGroup>(ageGroupDtoForInsert);
            ageGroup.TenantId = currentTenant.Id; // Set tenant ID

            await _repositoryManager.AgeGroupRepository.CreateAsync(ageGroup);
            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<AgeGroupDto>> GetAllAgeGroupsAsync(bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<AgeGroupDto>();
            }

            // Get all age groups for current tenant (or all for admin)
            var adminTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            IEnumerable<AgeGroup> ageGroups;

            if (currentTenant.Id == adminTenantId)
            {
                // Admin sees all age groups
                ageGroups = await _repositoryManager.AgeGroupRepository.GetAllAsync(trackChanges);
            }
            else
            {
                // Regular tenant sees only their age groups
                ageGroups = await _repositoryManager.AgeGroupRepository
                    .GetAllByConditionAsync(x => x.TenantId == currentTenant.Id, trackChanges);
            }

            return _mapper.Map<IEnumerable<AgeGroupDto>>(ageGroups);
        }

        public async Task<AgeGroup?> GetAgeGroupAsync(int id, bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return null;
            }

            var adminTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");

            if (currentTenant.Id == adminTenantId)
            {
                // Admin can get any age group
                return await _repositoryManager.AgeGroupRepository
                    .FindByConditionAsync(x => x.AgeGroupId == id, trackChanges);
            }
            else
            {
                // Regular tenant can only get their own age groups
                return await _repositoryManager.AgeGroupRepository
                    .FindByConditionAsync(x => x.AgeGroupId == id && x.TenantId == currentTenant.Id, trackChanges);
            }
        }

        public async Task UpdateAgeGroupAsync(AgeGroupDtoForUpdate ageGroupDtoForUpdate)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            await ValidateAgeGroupAsync(null, ageGroupDtoForUpdate, currentTenant.Id);

            // First get the existing age group
            var existingAgeGroup = await GetAgeGroupAsync(ageGroupDtoForUpdate.AgeGroupId, true);
            if (existingAgeGroup == null)
            {
                throw new ValidationException(_localizer["AgeGroupNotFound"]);
            }

            // Ensure the age group belongs to current tenant (unless admin)
            var adminTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            if (currentTenant.Id != adminTenantId && existingAgeGroup.TenantId != currentTenant.Id)
            {
                throw new ValidationException(_localizer["CannotUpdateAgeGroupFromAnotherTenant"]);
            }

            _mapper.Map(ageGroupDtoForUpdate, existingAgeGroup);

            await _repositoryManager.AgeGroupRepository.UpdateAsync(existingAgeGroup);
            await _repositoryManager.SaveAsync();
        }

        private async Task ValidateAgeGroupAsync(
            AgeGroupDtoForInsert? ageGroupDtoForInsert,
            AgeGroupDtoForUpdate? ageGroupDtoForUpdate,
            Guid currentTenantId)
        {
            var dtoToValidate = (object?)ageGroupDtoForInsert ?? ageGroupDtoForUpdate;
            if (dtoToValidate == null)
                throw new ArgumentNullException(nameof(dtoToValidate), _localizer["DtoCannotBeNull"] + ".");

            var validationException = new List<ValidationException>();

            var ageGroupId = dtoToValidate.GetType().GetProperty("AgeGroupId")?.GetValue(dtoToValidate) as int? ?? 0;
            var minAge = (int)dtoToValidate.GetType().GetProperty("MinAge")?.GetValue(dtoToValidate);
            var maxAge = (int)dtoToValidate.GetType().GetProperty("MaxAge")?.GetValue(dtoToValidate);

            // Check for duplicate age range within the same tenant
            var existingAgeGroup = await _repositoryManager.AgeGroupRepository
                .FindByConditionAsync(
                    ag => ag.MinAge == minAge &&
                          ag.MaxAge == maxAge &&
                          ag.TenantId == currentTenantId,
                    false);

            if (existingAgeGroup != null && existingAgeGroup.AgeGroupId != ageGroupId)
            {
                validationException.Add(new ValidationException(
                    _localizer["AnAgeGroupWithTheSameMinAgeAndMaxAgeAlreadyExists"] + ".",
                    new Exception() { Source = "Model" }));
            }

            if (minAge < 0)
            {
                validationException.Add(new ValidationException(
                    _localizer["AgeCannotBeNegative"] + ".",
                    new Exception() { Source = "MinAge" }));
            }
            if (maxAge < 0)
            {
                validationException.Add(new ValidationException(
                    _localizer["AgeCannotBeNegative"] + ".",
                    new Exception() { Source = "MaxAge" }));
            }
            if (minAge > maxAge)
            {
                validationException.Add(new ValidationException(
                    _localizer["MinimumAgeCannotBeGreaterThanMaximumAge"] + ".",
                    new Exception() { Source = "MaxAge" }));
            }
            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }

        public async Task<AgeGroupDtoForUpdate> GetAgeGroupForUpdateAsync(int id, bool trackChanges)
        {
            var ageGroup = await GetAgeGroupAsync(id, trackChanges);
            if (ageGroup == null)
            {
                throw new ValidationException(_localizer["AgeGroupNotFound"]);
            }
            return _mapper.Map<AgeGroupDtoForUpdate>(ageGroup);
        }

        public async Task DeleteAgeGroupAsync(int id)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var ageGroup = await GetAgeGroupAsync(id, false);
            if (ageGroup == null)
            {
                throw new ValidationException(_localizer["AgeGroupNotFound"]);
            }

            // Ensure the age group belongs to current tenant (unless admin)
            var adminTenantId = Guid.Parse("00000000-0000-0000-0000-000000000001");
            if (currentTenant.Id != adminTenantId && ageGroup.TenantId != currentTenant.Id)
            {
                throw new ValidationException(_localizer["CannotDeleteAgeGroupFromAnotherTenant"]);
            }

            try
            {
                await _repositoryManager.AgeGroupRepository.DeleteAsync(ageGroup);
                await _repositoryManager.SaveAsync();
            }
            catch (Exception exception)
            {
                if (exception.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    throw new ValidationException(
                        _localizer["AgeGroupCannotBeDeletedBecauseItIsUsedInAnotherEntity"] + ".",
                        new Exception() { Source = "Model" });
                }
                throw;
            }
        }
    }
}