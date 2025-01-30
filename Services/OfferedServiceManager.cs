using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Entities.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;
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

        public void CreateofferedService(OfferedService offeredService)
        {
            //ValidateofferedService(offeredService);
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

        public IEnumerable<OfferedServiceDto> GetAllOfferedServices(bool trackChanges, string language = "en-GB")
        {
            var offeredServices = _repositoryManager.OfferedServiceRepository.GetAllOfferedServices(trackChanges, language);
            var offeredServicesDto = _mapper.Map<IEnumerable<OfferedServiceDto>>(offeredServices);
            return offeredServicesDto;
        }

        public OfferedService? GetofferedService(int id, bool trackChanges)
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
                                                                   .FirstOrDefault();
            // 2. Update scalar properties
            _mapper.Map(offeredServiceDtoForUpdate, model);
            ValidateofferedService(offeredServiceDtoForUpdate);
            _repositoryManager.OfferedServiceRepository.Update(model, offeredServiceDtoForUpdate.AgeGroupIds);
            _repositoryManager.Save();
        }

        public void ValidateofferedService(OfferedServiceDtoForUpdate offeredServiceDtoForUpdate)
        {
            var validationException = new List<ValidationException>();
            if (offeredServiceDtoForUpdate.AgeGroupIds.Count == 0)
            {
                validationException.Add(new ValidationException(_localizer["PleaseSelectAtLeastOneAgeGroup"] + ".", new Exception() { Source = "AgeGroups" }));
            }
            if (offeredServiceDtoForUpdate.Genders.Count == 0)
            {
                validationException.Add(new ValidationException(_localizer["PleaseSelectAtLeastOneGender"] + ".", new Exception() { Source = "Genders" }));
            }
            if (offeredServiceDtoForUpdate.ApproximateDuration.Equals(new TimeSpan(0, 0, 0)))
            {
                validationException.Add(new ValidationException(_localizer["ApproximateDurationMustBeBiggerThan0Minutes"] + ".", new Exception() { Source = "ApproximateDuration" }));
            }
            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }

    }
}
