using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public class BranchRepository : RepositoryBase<Branch>, IBranchRepository
    {
        public BranchRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task CreateBranchAsync(Branch branch)
        {
            await CreateAsync(branch);
        }

        public async Task<int> GetReservationInAdvanceDayLimitAsync(int branchId)
        {
            return await _repositoryContext.Branches.Where(b => b.BranchId == branchId)
                                              .AsNoTracking()
                                              .Select(b => b.ReservationInAdvanceDayLimit)
                                              .FirstOrDefaultAsync();
        }
    }
}
