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
    public class BranchManager : IBranchService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IStringLocalizer<BranchManager> _localizer;
        private readonly IMapper _mapper;

        public BranchManager(IRepositoryManager repositoryManager, IMapper mapper, IStringLocalizer<BranchManager> localizer)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _localizer = localizer;
        }

        public void CreateBranch(BranchDtoForInsert branchDtoForInsert)
        {
            Branch branch = _mapper.Map<Branch>(branchDtoForInsert);
            _repositoryManager.BranchRepository.CreateBranch(branch);
            _repositoryManager.Save();
        }

        public IEnumerable<BranchDto> GetAllBranches(bool trackChanges)
        {
            var branches = _repositoryManager.BranchRepository.GetAll(trackChanges);
            var branchesDto = _mapper.Map<IEnumerable<BranchDto>>(branches);
            return branchesDto;
        }

        private Branch? GetBranch(int id, bool trackChanges)
        {
            return _repositoryManager.BranchRepository.FindByCondition(x => x.BranchId.Equals(id), trackChanges);
        }

        public BranchDtoForUpdate? GetBranchForUpdate(int id, bool trackChanges)
        {
            var branch = GetBranch(id, trackChanges);
            return _mapper.Map<BranchDtoForUpdate>(branch);
        }

        public void UpdateBranch(BranchDtoForUpdate branchDtoForUpdate)
        {
            var model = _mapper.Map<Branch>(branchDtoForUpdate);
            _repositoryManager.BranchRepository.Update(model);
            _repositoryManager.Save();
        }

        public void DeleteAgeGroup(int id)
        {
            var branch = GetBranch(id, false);
            try
            {
                _repositoryManager.BranchRepository.Delete(branch);
                _repositoryManager.Save();
            }
            catch (Exception exception)
            {
                if (exception.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    throw new ValidationException(_localizer["BranchCannotBeDeletedBecauseItIsUsedInAnotherEntity"] + ".", new Exception() { Source = "Model" });
                }
                throw;
            }
        }
    }
}
