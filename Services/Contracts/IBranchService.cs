using Entities.Dtos;

namespace Services.Contracts
{
    public interface IBranchService
    {
        Task CreateBranchAsync(BranchDtoForInsert branchDtoForInsert);
        Task DeleteBranchAsync(int id);
        Task<IEnumerable<BranchDto>> GetAllActiveBranchesAsync(bool trackChanges);
        Task<IEnumerable<BranchDto>> GetAllBranchesAsync(bool trackChanges);
        Task<BranchDtoForUpdate?> GetBranchForUpdateAsync(int id, bool trackChanges);
        Task UpdateBranchAsync(BranchDtoForUpdate branchDtoForUpdate);
    }
}
