using Entities.Models;

namespace Repositories.Contracts
{
    public interface IBranchRepository : IRepositoryBase<Branch>
    {
        void CreateBranch(Branch branch);
        int GetReservationInAdvanceDayLimit(int branchId);
    }
}
