using Entities.Models;
using Entities.Models.Enums;
using Microsoft.Extensions.Localization;
using Repositories.Contracts;
using Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class OfferedServiceManager : IOfferedServiceService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IStringLocalizer<OfferedServiceManager> _localizer;

        public OfferedServiceManager(IRepositoryManager repositoryManager, IStringLocalizer<OfferedServiceManager> localizer)
        {
            _repositoryManager = repositoryManager;
            _localizer = localizer;
        }

        public void CreateofferedService(OfferedService offeredService)
        {
            ValidateofferedService(offeredService);
            _repositoryManager.OfferedServiceRepository.CreateofferedService(offeredService);
            _repositoryManager.Save();
        }

        public IEnumerable<OfferedService> GetActiveOfferedServices(bool trackChanges, string language = "en-GB")
        {
            return _repositoryManager.OfferedServiceRepository.GetActiveOfferedServices(trackChanges, language);
        }

        public IEnumerable<OfferedService> GetAllForCustomerAppointment(Gender gender, int ageGroupId, bool trackChanges, string language = "en-GB")
        {
            return _repositoryManager.OfferedServiceRepository.GetAllForCustomerAppointment(gender, ageGroupId, trackChanges, language);
        }

        public IEnumerable<OfferedService> GetAllOfferedServices(bool trackChanges, string language = "en-GB")
        {
            return _repositoryManager.OfferedServiceRepository.GetAllOfferedServices(trackChanges, language);
        }

        public OfferedService? GetofferedService(int id, bool trackChanges)
        {
            var offeredService = _repositoryManager.OfferedServiceRepository.FindByCondition(x => x.OfferedServiceId.Equals(id), trackChanges);
            return offeredService;
        }

        public void ValidateofferedService(OfferedService offeredService)
        {
            var validationException = new List<ValidationException>();
            if (offeredService.AgeGroups.Count == 0)
            {
                validationException.Add(new ValidationException(_localizer["PleaseSelectAtLeastOneAgeGroup."], new Exception() { Source = "AgeGroups" }));
            }
            if (offeredService.Genders.Count == 0)
            {
                validationException.Add(new ValidationException(_localizer["PleaseSelectAtLeastOneGender."], new Exception() { Source = "Genders" }));
            }
            if (offeredService.ApproximateDuration.Equals(new TimeSpan(0, 0, 0)))
            {
                validationException.Add(new ValidationException(_localizer["ApproximateDurationMustBeBiggerThan0Minutes."], new Exception() { Source = "ApproximateDuration" }));
            }
            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }

    }
}
