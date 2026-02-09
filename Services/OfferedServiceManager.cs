using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Entities.Models.Enums;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class OfferedServiceManager : IOfferedServiceService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IStringLocalizer<OfferedServiceManager> _localizer;
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService; // Added

        public OfferedServiceManager(
            IRepositoryManager repositoryManager,
            IStringLocalizer<OfferedServiceManager> localizer,
            IMapper mapper,
            ITenantService tenantService) // Added
        {
            _repositoryManager = repositoryManager;
            _localizer = localizer;
            _mapper = mapper;
            _tenantService = tenantService;
        }

        public async Task CreateOfferedServiceAsync(OfferedServiceDtoForInsert offeredServiceDtoForInsert)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            // Validate the DTO
            await ValidateOfferedServiceAsync(offeredServiceDtoForInsert, null, currentTenant.Id);

            // Map the DTO to the entity (without AgeGroups for now)
            var offeredService = _mapper.Map<OfferedService>(offeredServiceDtoForInsert);
            offeredService.TenantId = currentTenant.Id; // Set tenant ID from context

            // Fetch existing AgeGroups from the database for current tenant only
            var existingAgeGroups = await _repositoryManager.AgeGroupRepository
                .GetAllByConditionAsync(ag =>
                    offeredServiceDtoForInsert.AgeGroupIds.Contains(ag.AgeGroupId)
                    && ag.TenantId == currentTenant.Id, // Tenant filter
                    trackChanges: true);

            // Link existing AgeGroups to the OfferedService
            offeredService.AgeGroups = existingAgeGroups.ToList();

            // Handle translations (non-empty entries)
            if (offeredServiceDtoForInsert.Translations != null)
            {
                foreach (var translation in offeredServiceDtoForInsert.Translations)
                {
                    var culture = translation.Key;
                    var name = translation.Value?.Trim();

                    if (!string.IsNullOrWhiteSpace(name))
                    {
                        offeredService.OfferedServiceLocalizations.Add(new OfferedServiceLocalization
                        {
                            Language = culture,
                            OfferedServiceLocalizationName = name,
                            OfferedService = offeredService,
                            TenantId = currentTenant.Id // Set tenant ID for localization
                        });
                    }
                }
            }

            // Save the OfferedService (EF Core will handle relationships)
            await _repositoryManager.OfferedServiceRepository.CreateAsync(offeredService);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteOfferedServiceAsync(int id)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var offeredService = await GetOfferedServiceByIdAsync(id, false);

            if (offeredService == null)
            {
                throw new ValidationException(_localizer["OfferedServiceNotFound"]);
            }

            // Security check - ensure offered service belongs to current tenant
            if (offeredService.TenantId != currentTenant.Id)
            {
                throw new ValidationException(_localizer["CannotDeleteOfferedServiceFromAnotherTenant"]);
            }

            try
            {
                await _repositoryManager.OfferedServiceRepository.DeleteAsync(offeredService);
                await _repositoryManager.SaveAsync();
            }
            catch (Exception exception)
            {
                if (exception.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    throw new ValidationException(_localizer["OfferedServiceCannotBeDeletedBecauseItIsUsedInAnotherEntity"] + ".", new Exception() { Source = "Model" });
                }
                throw;
            }
        }

        public async Task<IEnumerable<OfferedServiceDto>> GetActiveOfferedServicesAsync(bool trackChanges, string language = "en-GB")
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<OfferedServiceDto>();
            }

            // Get active offered services for current tenant only
            var offeredServices = await _repositoryManager.OfferedServiceRepository
                .GetAllByConditionAsync(x =>
                    x.Status == true
                    && x.TenantId == currentTenant.Id, // Tenant filter
                    trackChanges);

            var offeredServicesDto = _mapper.Map<IEnumerable<OfferedServiceDto>>(offeredServices);
            return offeredServicesDto;
        }

        public async Task<IEnumerable<OfferedService>> GetAllForCustomerAppointmentAsync(Gender gender, int ageGroupId, bool trackChanges, string language = "en-GB")
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<OfferedService>();
            }

            // Get offered services for customer appointment for current tenant only
            return await _repositoryManager.OfferedServiceRepository
                .GetAllByConditionAsync(x =>
                    x.Genders.Contains(gender)
                    && x.AgeGroups.Any(ag => ag.AgeGroupId == ageGroupId)
                    && x.Status == true
                    && x.TenantId == currentTenant.Id, // Tenant filter
                    trackChanges);
        }

        public async Task<IEnumerable<OfferedServiceDto>> GetAllOfferedServicesAsync(bool trackChanges, string language = "en-GB")
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<OfferedServiceDto>();
            }

            // Get all offered services for current tenant only
            var offeredServices = await _repositoryManager.OfferedServiceRepository
                .GetAllByConditionAsync(x => x.TenantId == currentTenant.Id, // Tenant filter
                    trackChanges);

            var offeredServicesDto = _mapper.Map<IEnumerable<OfferedServiceDto>>(offeredServices);
            return offeredServicesDto;
        }

        public async Task<OfferedService?> GetOfferedServiceByIdAsync(int id, bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return null;
            }

            // Get offered service for current tenant only
            var offeredService = await _repositoryManager.OfferedServiceRepository
                .FindByConditionAsync(x =>
                    x.OfferedServiceId.Equals(id)
                    && x.TenantId == currentTenant.Id, // Tenant filter
                    trackChanges);

            return offeredService;
        }

        public async Task<OfferedServiceDtoForUpdate?> GetOfferedServiceForUpdateAsync(int id, bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            // Get offered service for update for current tenant only
            var offeredService = await _repositoryManager.OfferedServiceRepository
                .FindByConditionAsync(x =>
                    x.OfferedServiceId.Equals(id)
                    && x.TenantId == currentTenant.Id, // Tenant filter
                    trackChanges,
                    include: q => q.Include(os => os.AgeGroups)
                                  .Include(os => os.OfferedServiceLocalizations));

            if (offeredService == null)
            {
                return null;
            }

            var offeredServiceDtoForUpdate = _mapper.Map<OfferedServiceDtoForUpdate>(offeredService);
            return offeredServiceDtoForUpdate;
        }

        public async Task UpdateOfferedServiceAsync(OfferedServiceDtoForUpdate offeredServiceDtoForUpdate)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            // Get existing offered service with tenant check
            var existingOfferedService = await _repositoryManager.OfferedServiceRepository
                .FindByConditionAsync(x =>
                    x.OfferedServiceId.Equals(offeredServiceDtoForUpdate.OfferedServiceId)
                    && x.TenantId == currentTenant.Id, // Tenant filter
                    true,
                    include: q => q.Include(os => os.AgeGroups)
                                  .Include(os => os.OfferedServiceLocalizations));

            if (existingOfferedService == null)
            {
                throw new ValidationException(_localizer["OfferedServiceNotFound"]);
            }

            // Validate the DTO with tenant context
            await ValidateOfferedServiceAsync(null, offeredServiceDtoForUpdate, currentTenant.Id);

            // Map updates to existing entity
            _mapper.Map(offeredServiceDtoForUpdate, existingOfferedService);

            // Process translations
            ProcessTranslations(existingOfferedService, offeredServiceDtoForUpdate.Translations, currentTenant.Id);

            // Update AgeGroups for current tenant only
            await UpdateAgeGroupsAsync(existingOfferedService, offeredServiceDtoForUpdate.AgeGroupIds, currentTenant.Id);

            await _repositoryManager.SaveAsync();
        }

        private async Task UpdateAgeGroupsAsync(OfferedService offeredService, List<int> ageGroupIds, Guid tenantId)
        {
            offeredService.AgeGroups.Clear();

            // Get age groups for current tenant only
            var ageGroups = await _repositoryManager.AgeGroupRepository
                .GetAllByConditionAsync(ag =>
                    ageGroupIds.Contains(ag.AgeGroupId)
                    && ag.TenantId == tenantId, // Tenant filter
                    true);

            foreach (var ageGroup in ageGroups)
            {
                offeredService.AgeGroups.Add(ageGroup);
            }
        }

        private void ProcessTranslations(OfferedService model, Dictionary<string, string> translations, Guid tenantId)
        {
            // Process Translations
            if (translations != null)
            {
                foreach (var translation in translations)
                {
                    var culture = translation.Key;
                    var name = translation.Value?.Trim(); // Handle null and trim whitespace

                    var existingLocalization = model.OfferedServiceLocalizations?
                        .FirstOrDefault(l => l.Language == culture);

                    if (string.IsNullOrWhiteSpace(name))
                    {
                        // Delete the translation if the input is empty
                        if (existingLocalization != null)
                        {
                            // Remove from the collection (EF Core will track this deletion)
                            model.OfferedServiceLocalizations!.Remove(existingLocalization);
                        }
                    }
                    else
                    {
                        // Update or add the translation
                        if (existingLocalization != null)
                        {
                            existingLocalization.OfferedServiceLocalizationName = name!;
                        }
                        else
                        {
                            model.OfferedServiceLocalizations!.Add(new OfferedServiceLocalization
                            {
                                Language = culture,
                                OfferedServiceLocalizationName = name!,
                                OfferedServiceId = model.OfferedServiceId,
                                TenantId = tenantId // Set tenant ID
                            });
                        }
                    }
                }
            }
        }

        private async Task ValidateOfferedServiceAsync(
            OfferedServiceDtoForInsert? offeredServiceDtoForInsert,
            OfferedServiceDtoForUpdate? offeredServiceDtoForUpdate,
            Guid currentTenantId)
        {
            var dtoToValidate = (object?)offeredServiceDtoForInsert ?? offeredServiceDtoForUpdate;
            if (dtoToValidate == null)
                throw new ArgumentNullException(nameof(dtoToValidate), _localizer["DtoCannotBeNull"] + ".");

            var validationException = new List<ValidationException>();

            // Get properties via reflection
            var ageGroupIds = (List<int>?)dtoToValidate.GetType().GetProperty("AgeGroupIds")?.GetValue(dtoToValidate);
            var genders = (List<Gender>?)dtoToValidate.GetType().GetProperty("Genders")?.GetValue(dtoToValidate);
            var approximateDuration = (TimeSpan?)dtoToValidate.GetType().GetProperty("ApproximateDuration")?.GetValue(dtoToValidate);

            // Validation logic
            if (ageGroupIds?.Count == 0)
            {
                validationException.Add(new ValidationException(
                    _localizer["PleaseSelectAtLeastOneAgeGroup"] + ".",
                    new Exception() { Source = "AgeGroups" }
                ));
            }
            else if (ageGroupIds != null)
            {
                // Verify all age groups belong to current tenant
                var tenantAgeGroups = await _repositoryManager.AgeGroupRepository
                    .GetAllByConditionAsync(ag =>
                        ageGroupIds.Contains(ag.AgeGroupId)
                        && ag.TenantId == currentTenantId, // Tenant filter
                        false);

                if (tenantAgeGroups.Count() != ageGroupIds.Count)
                {
                    validationException.Add(new ValidationException(
                        _localizer["SomeAgeGroupsDoNotBelongToYourTenant"] + ".",
                        new Exception() { Source = "AgeGroups" }
                    ));
                }
            }

            if (genders?.Count == 0)
            {
                validationException.Add(new ValidationException(
                    _localizer["PleaseSelectAtLeastOneGender"] + ".",
                    new Exception() { Source = "Genders" }
                ));
            }

            if (approximateDuration?.Equals(TimeSpan.Zero) == true)
            {
                validationException.Add(new ValidationException(
                    _localizer["ApproximateDurationMustBeBiggerThan0Minutes"] + ".",
                    new Exception() { Source = "ApproximateDuration" }
                ));
            }

            if (validationException.Count != 0)
                throw new AggregateException(validationException);
        }
    }
}