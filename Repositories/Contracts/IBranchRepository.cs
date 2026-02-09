using Entities.Models;

namespace Repositories.Contracts
{
    public interface IBranchRepository : IRepositoryBase<Branch>
    {
        Task CreateBranchAsync(Branch branch);
        Task<int> GetReservationInAdvanceDayLimitAsync(int branchId);
    }
}
