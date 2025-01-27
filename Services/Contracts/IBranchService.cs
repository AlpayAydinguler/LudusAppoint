using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IBranchService
    {
        void CreateBranch(Branch branch);
        public IEnumerable<BranchDto> GetAllBranches(bool trackChanges);
        public Branch? GetBranch(int id, bool trackChangesv);
        void UpdateBranch(Branch branch);
    }
}
