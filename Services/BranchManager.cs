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
        private readonly ITenantService _tenantService; // Added

        public BranchManager(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            IStringLocalizer<BranchManager> localizer,
            ITenantService tenantService) // Added
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _localizer = localizer;
            _tenantService = tenantService;
        }

        public async Task CreateBranchAsync(BranchDtoForInsert branchDtoForInsert)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var branch = _mapper.Map<Branch>(branchDtoForInsert);
            branch.TenantId = currentTenant.Id; // Set tenant ID from context

            await _repositoryManager.BranchRepository.CreateBranchAsync(branch);
            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<BranchDto>> GetAllBranchesAsync(bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<BranchDto>();
            }

            // Get branches for current tenant
            var branches = await _repositoryManager.BranchRepository
                .GetAllByConditionAsync(x => x.TenantId == currentTenant.Id, trackChanges);

            var branchesDto = _mapper.Map<IEnumerable<BranchDto>>(branches);
            return branchesDto;
        }

        private async Task<Branch?> GetBranchAsync(int id, bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return null;
            }

            // Get branch by ID and tenant
            return await _repositoryManager.BranchRepository
                .FindByConditionAsync(x => x.BranchId.Equals(id) && x.TenantId == currentTenant.Id,
                                     trackChanges);
        }

        public async Task<BranchDtoForUpdate?> GetBranchForUpdateAsync(int id, bool trackChanges)
        {
            var branch = await GetBranchAsync(id, trackChanges);
            return _mapper.Map<BranchDtoForUpdate>(branch);
        }

        public async Task UpdateBranchAsync(BranchDtoForUpdate branchDtoForUpdate)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            // Get existing branch with tenant check
            var existingBranch = await GetBranchAsync(branchDtoForUpdate.BranchId, true);
            if (existingBranch == null)
            {
                throw new ValidationException(_localizer["BranchNotFound"]);
            }

            // Ensure the branch belongs to the current tenant
            if (existingBranch.TenantId != currentTenant.Id)
            {
                throw new ValidationException(_localizer["CannotUpdateBranchFromAnotherTenant"]);
            }

            // Map updates to existing branch
            _mapper.Map(branchDtoForUpdate, existingBranch);

            await _repositoryManager.BranchRepository.UpdateAsync(existingBranch);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteBranchAsync(int id)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var branch = await GetBranchAsync(id, false);
            if (branch == null)
            {
                throw new ValidationException(_localizer["BranchNotFound"]);
            }

            // Ensure the branch belongs to the current tenant
            if (branch.TenantId != currentTenant.Id)
            {
                throw new ValidationException(_localizer["CannotDeleteBranchFromAnotherTenant"]);
            }

            try
            {
                await _repositoryManager.BranchRepository.DeleteAsync(branch);
                await _repositoryManager.SaveAsync();
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

        public async Task<IEnumerable<BranchDto>> GetAllActiveBranchesAsync(bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<BranchDto>();
            }

            // Get active branches for current tenant
            var branches = await _repositoryManager.BranchRepository
                .GetAllByConditionAsync(x => x.Status && x.TenantId == currentTenant.Id, trackChanges);

            var branchesDto = _mapper.Map<IEnumerable<BranchDto>>(branches);
            return branchesDto;
        }
    }
}