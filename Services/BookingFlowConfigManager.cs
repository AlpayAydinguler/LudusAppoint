using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.Extensions.Localization;
using Repositories.Contracts;
using Services.Contracts;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace Services
{
    public class BookingFlowConfigManager : IBookingFlowConfigService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryManager _repositoryManager;
        private readonly IStringLocalizer<BookingFlowConfigManager> _localizer;
        private readonly ITenantService _tenantService;

        public BookingFlowConfigManager(
            IMapper mapper,
            IRepositoryManager repositoryManager,
            IStringLocalizer<BookingFlowConfigManager> localizer,
            ITenantService tenantService)
        {
            _mapper = mapper;
            _repositoryManager = repositoryManager;
            _localizer = localizer;
            _tenantService = tenantService;
        }

        public async Task<BookingFlowConfigDto> GetBookingFlowConfigForBranchAsync(int branchId)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return null;
            }

            var config = await _repositoryManager.BookingFlowConfigRepository
                .GetBookingFlowConfigByBranchIdForUpdateAsync(currentTenant.Id, branchId, false);

            if (config == null)
            {
                // Return default configuration for current tenant
                return new BookingFlowConfigDto
                {
                    BranchId = branchId,
                    AllStepsInOrder = "[\"Services\", \"DateTime\", \"RoomSelection\", \"Employee\"]",
                    EnabledStepsInOrder = "[\"Services\", \"DateTime\", \"RoomSelection\", \"Employee\"]",
                    CreatedAt = DateTime.UtcNow
                };
            }

            return _mapper.Map<BookingFlowConfigDto>(config);
        }

        public async Task<IEnumerable<BookingFlowConfigDto>> GetAllBookingFlowConfigsAsync()
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<BookingFlowConfigDto>();
            }

            var configs = await _repositoryManager.BookingFlowConfigRepository
                .GetAllBookingFlowConfigByTenantIdAsync(currentTenant.Id, false);

            return _mapper.Map<IEnumerable<BookingFlowConfigDto>>(configs);
        }

        public async Task UpdateBookingFlowConfigAsync(BookingFlowConfigDtoForUpdate configDto)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var validationException = new List<ValidationException>();

            // Validate AllStepsInOrder JSON format
            if (!IsValidStepOrderJson(configDto.AllStepsInOrder, "AllStepsInOrder"))
            {
                validationException.Add(new ValidationException(
                    _localizer["InvalidAllStepsOrderFormat"],
                    new Exception() { Source = "Model" }));
            }

            // Validate EnabledStepsInOrder JSON format
            if (!IsValidStepOrderJson(configDto.EnabledStepsInOrder, "EnabledStepsInOrder"))
            {
                validationException.Add(new ValidationException(
                    _localizer["InvalidEnabledStepsOrderFormat"],
                    new Exception() { Source = "Model" }));
            }

            // Validate that EnabledStepsInOrder is a subset of AllStepsInOrder
            if (!IsEnabledStepsSubsetOfAllSteps(configDto.AllStepsInOrder, configDto.EnabledStepsInOrder))
            {
                validationException.Add(new ValidationException(
                    _localizer["EnabledStepsMustBeSubsetOfAllSteps"],
                    new Exception() { Source = "Model" }));
            }

            var config = await _repositoryManager.BookingFlowConfigRepository
                .GetBookingFlowConfigByBranchIdForUpdateAsync(currentTenant.Id, configDto.BranchId, true);

            if (config == null)
            {
                // Create if doesn't exist
                config = _mapper.Map<BookingFlowConfig>(configDto);
                config.TenantId = currentTenant.Id;
                await _repositoryManager.BookingFlowConfigRepository.CreateAsync(config);
            }
            else
            {
                // Security check - ensure config belongs to current tenant
                if (config.TenantId != currentTenant.Id)
                {
                    throw new ValidationException(_localizer["CannotUpdateConfigFromAnotherTenant"]);
                }

                _mapper.Map(configDto, config);
                config.UpdatedAt = DateTime.UtcNow;
                await _repositoryManager.BookingFlowConfigRepository.UpdateAsync(config);
            }

            await _repositoryManager.BookingFlowConfigRepository.SaveAsync();

            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }

        private bool IsValidStepOrderJson(string stepOrderJson, string fieldName)
        {
            try
            {
                var steps = JsonSerializer.Deserialize<List<string>>(stepOrderJson);
                if (steps == null || steps.Count == 0)
                    return false;

                // Validate that all steps are valid options
                var validSteps = new HashSet<string> { "Services", "DateTime", "RoomSelection", "Employee" };

                // For AllStepsInOrder, check all are valid and no duplicates
                if (fieldName == "AllStepsInOrder")
                {
                    var seen = new HashSet<string>();
                    foreach (var step in steps)
                    {
                        if (!validSteps.Contains(step) || seen.Contains(step))
                            return false;
                        seen.Add(step);
                    }
                    return true;
                }

                // For EnabledStepsInOrder, just check all are valid (duplicates not allowed either)
                var seenEnabled = new HashSet<string>();
                foreach (var step in steps)
                {
                    if (!validSteps.Contains(step) || seenEnabled.Contains(step))
                        return false;
                    seenEnabled.Add(step);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private bool IsEnabledStepsSubsetOfAllSteps(string allStepsJson, string enabledStepsJson)
        {
            try
            {
                var allSteps = JsonSerializer.Deserialize<List<string>>(allStepsJson) ?? new List<string>();
                var enabledSteps = JsonSerializer.Deserialize<List<string>>(enabledStepsJson) ?? new List<string>();

                // Convert to sets for subset check
                var allStepsSet = new HashSet<string>(allSteps);
                var enabledStepsSet = new HashSet<string>(enabledSteps);

                return enabledStepsSet.IsSubsetOf(allStepsSet);
            }
            catch
            {
                return false;
            }
        }
    }
}