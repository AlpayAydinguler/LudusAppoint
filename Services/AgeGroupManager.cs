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

        public AgeGroupManager(IRepositoryManager repositoryManager, IStringLocalizer<AgeGroupManager> localizer, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _localizer = localizer;
            _mapper = mapper;
        }

        public void CreateAgeGroup(AgeGroupDtoForInsert ageGroupDtoForInsert)
        {
            ValidateAgeGroup(ageGroupDtoForInsert, null);
            AgeGroup ageGroup = _mapper.Map<AgeGroup>(ageGroupDtoForInsert);
            _repositoryManager.AgeGroupRepository.CreateAgeGroup(ageGroup);
            _repositoryManager.Save();

        }

        public IEnumerable<AgeGroupDto> GetAllAgeGroups(bool trackChanges)
        {
            var ageGroups = _repositoryManager.AgeGroupRepository.GetAll(trackChanges);
            var ageGroupsDto = _mapper.Map<IEnumerable<AgeGroupDto>>(ageGroups);
            return ageGroupsDto;
        }

        public AgeGroup? GetAgeGroup(int id, bool trackChanges)
        {
            return _repositoryManager.AgeGroupRepository.FindByCondition(x => x.AgeGroupId.Equals(id), trackChanges);
        }

        public void UpdateAgeGroup(AgeGroupDtoForUpdate ageGroupDtoForUpdate)
        {
            ValidateAgeGroup(null, ageGroupDtoForUpdate);
            var model = _mapper.Map<AgeGroup>(ageGroupDtoForUpdate);
            _repositoryManager.AgeGroupRepository.UpdateAgeGroup(model);
            _repositoryManager.Save();
        }
        private void ValidateAgeGroup(AgeGroupDtoForInsert? ageGroupDtoForInsert, AgeGroupDtoForUpdate? ageGroupDtoForUpdate)
        {
            var dtoToValidate = (object?)ageGroupDtoForInsert ?? ageGroupDtoForUpdate;
            if (dtoToValidate == null)
                throw new ArgumentNullException(nameof(dtoToValidate), _localizer["DtoCannotBeNull"] + ".");

            var validationException = new List<ValidationException>();

            var ageGroupId = (int)dtoToValidate.GetType().GetProperty("AgeGroupId")?.GetValue(dtoToValidate);
            var minAge = (int)dtoToValidate.GetType().GetProperty("MinAge")?.GetValue(dtoToValidate);
            var maxAge = (int)dtoToValidate.GetType().GetProperty("MaxAge")?.GetValue(dtoToValidate);

            var existingAgeGroup = _repositoryManager.AgeGroupRepository.FindByCondition(ag => ag.MinAge == minAge && ag.MaxAge == maxAge, false);

            if (existingAgeGroup != null && existingAgeGroup.AgeGroupId != ageGroupId)
            {
                validationException.Add(new ValidationException("AnAgeGroupWithTheSameMinAgeAndMaxAgeAlreadyExists" + ".", new Exception() { Source = "Model" }));
            }

            if (minAge < 0)
            {
                validationException.Add(new ValidationException(_localizer["AgeCannotBeNegative"] + ".", new Exception() { Source = "MinAge" }));
            }
            if (maxAge < 0)
            {
                validationException.Add(new ValidationException(_localizer["AgeCannotBeNegative"] + ".", new Exception() { Source = "MaxAge" }));
            }
            if (minAge > maxAge)
            {
                validationException.Add(new ValidationException(_localizer["MinimumAgeCannotBeGreaterThanMaximumAge"] + ".", new Exception() { Source = "MaxAge" }));
            }
            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }

        public AgeGroupDtoForUpdate GetAgeGroupForUpdate(int id, bool trackChanges)
        {
            var ageGroup = GetAgeGroup(id, trackChanges);
            return _mapper.Map<AgeGroupDtoForUpdate>(ageGroup);
        }

        public void DeleteAgeGroup(int id)
        {
            var ageGroup = GetAgeGroup(id, false);
            try
            {
                _repositoryManager.AgeGroupRepository.Delete(ageGroup);
                _repositoryManager.Save();
            }
            catch (Exception exception)
            {
                if (exception.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    throw new ValidationException(_localizer["AgeGroupCannotBeDeletedBecauseItIsUsedInAnotherEntity"] + ".", new Exception() { Source = "Model" });
                }
                throw;
            }
        }
    }
}
