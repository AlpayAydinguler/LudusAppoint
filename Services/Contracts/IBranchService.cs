using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IBranchService
    {
        void CreateBranch(BranchDtoForInsert branchDtoForInsert);
        void DeleteAgeGroup(int id);
        public IEnumerable<BranchDto> GetAllBranches(bool trackChanges);
        BranchDtoForUpdate? GetBranchForUpdate(int id, bool trackChanges);
        public void UpdateBranch(BranchDtoForUpdate branchDtoForUpdate);
    }
}
