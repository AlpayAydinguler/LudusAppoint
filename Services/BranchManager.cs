using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class BranchManager : IBranchService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public BranchManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        public void CreateBranch(Branch branch)
        {
            _repositoryManager.BranchRepository.CreateBranch(branch);
            _repositoryManager.Save();
        }

        public IEnumerable<BranchDto> GetAllBranches(bool trackChanges)
        {
            var branches = _repositoryManager.BranchRepository.GetAll(trackChanges);
            var branchesDto = _mapper.Map<IEnumerable<BranchDto>>(branches);
            return branchesDto;
        }

        public Branch? GetBranch(int id, bool trackChanges)
        {
            return _repositoryManager.BranchRepository.FindByCondition(x => x.BranchId.Equals(id), trackChanges);
        }

        public void UpdateBranch(Branch branch)
        {
            var model = _repositoryManager.BranchRepository.FindByCondition(x => x.BranchId.Equals(branch.BranchId), true);
            model.BranchName = branch.BranchName;
            model.Country = branch.Country;
            model.City = branch.City;
            model.District = branch.District;
            model.Street = branch.Street;
            model.Address = branch.Address;
            model.BranchPhoneNumber = branch.BranchPhoneNumber;
            model.BranchEmail = branch.BranchEmail;
            _repositoryManager.Save();
        }
    }
}
