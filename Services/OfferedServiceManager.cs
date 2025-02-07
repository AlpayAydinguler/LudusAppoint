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

        public OfferedServiceManager(IRepositoryManager repositoryManager, IStringLocalizer<OfferedServiceManager> localizer, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _localizer = localizer;
            _mapper = mapper;
        }

        public void CreateOfferedService(OfferedServiceDtoForInsert offeredServiceDtoForInsert)
        {
            // Validate the DTO
            ValidateOfferedService(offeredServiceDtoForInsert, null);

            // Map the DTO to the entity (without AgeGroups for now)
            var offeredService = _mapper.Map<OfferedService>(offeredServiceDtoForInsert);

            // Fetch existing AgeGroups from the database
            var existingAgeGroups = _repositoryManager.AgeGroupRepository
                .GetAllByCondition(ag => offeredServiceDtoForInsert.AgeGroupIds.Contains(ag.AgeGroupId), trackChanges: true)
                .ToList();

            // Link existing AgeGroups to the OfferedService
            offeredService.AgeGroups = existingAgeGroups;

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
                            OfferedService = offeredService
                        });
                    }
                }
            }

            // Save the OfferedService (EF Core will handle relationships)
            _repositoryManager.OfferedServiceRepository.Create(offeredService);
            _repositoryManager.Save();
        }

        public void DeleteOfferedService(int id)
        {
            var offeredService = GetOfferedServiceById(id, false);
            try
            {
                _repositoryManager.OfferedServiceRepository.Delete(offeredService);
                _repositoryManager.Save();
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

        public IEnumerable<OfferedServiceDto> GetActiveOfferedServices(bool trackChanges, string language = "en-GB")
        {
            var offeredServices = _repositoryManager.OfferedServiceRepository.GetActiveOfferedServices(trackChanges, language);
            var offeredServicesDto = _mapper.Map<IEnumerable<OfferedServiceDto>>(offeredServices);
            return offeredServicesDto;
        }

        public IEnumerable<OfferedService> GetAllForCustomerAppointment(Gender gender, int ageGroupId, bool trackChanges, string language = "en-GB")
        {
            return _repositoryManager.OfferedServiceRepository.GetAllForCustomerAppointment(gender, ageGroupId, trackChanges, language);
        }

        public IEnumerable<OfferedServiceDto> GetAllOfferedServices(bool trackChanges, string language = "en-GB")
        {
            var offeredServices = _repositoryManager.OfferedServiceRepository.GetAllOfferedServices(trackChanges, language);
            var offeredServicesDto = _mapper.Map<IEnumerable<OfferedServiceDto>>(offeredServices);
            return offeredServicesDto;
        }

        public OfferedService? GetOfferedServiceById(int id, bool trackChanges)
        {
            var offeredService = _repositoryManager.OfferedServiceRepository.FindByCondition(x => x.OfferedServiceId.Equals(id), trackChanges);
            return offeredService;
        }

        public OfferedServiceDtoForUpdate? GetOfferedServiceForUpdate(int id, bool trackChanges)
        {
            var offeredService = _repositoryManager.OfferedServiceRepository.GetOfferedServiceForUpdate(id, false);
            var offeredServiceDtoForUpdate = _mapper.Map<OfferedServiceDtoForUpdate>(offeredService);
            return offeredServiceDtoForUpdate;
        }

        public void UpdateOfferedService(OfferedServiceDtoForUpdate offeredServiceDtoForUpdate)
        {
            // 1. Get existing entity WITH TRACKING and INCLUDED AgeGroups
            var model = _repositoryManager.OfferedServiceRepository.GetAllByCondition(x => x.OfferedServiceId.Equals(offeredServiceDtoForUpdate.OfferedServiceId), true)
                                                                   .Include(os => os.AgeGroups)
                                                                   .Include(os => os.OfferedServiceLocalizations)
                                                                   .FirstOrDefault();
            // 2. Update scalar properties
            _mapper.Map(offeredServiceDtoForUpdate, model);
            ValidateOfferedService(null, offeredServiceDtoForUpdate);
            ProccessTranslations(model, offeredServiceDtoForUpdate.Translations);
            _repositoryManager.OfferedServiceRepository.Update(model, offeredServiceDtoForUpdate.AgeGroupIds);

            _repositoryManager.Save();
        }

        private void ProccessTranslations(OfferedService model, Dictionary<string, string> tranlations)
        {
            // Process Translations
            if (tranlations != null)
            {
                foreach (var translation in tranlations)
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
                                OfferedServiceId = model.OfferedServiceId
                            });
                        }
                    }
                }
            }
        }

        private void ValidateOfferedService(OfferedServiceDtoForInsert? offeredServiceDtoForInsert, OfferedServiceDtoForUpdate? offeredServiceDtoForUpdate)
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
