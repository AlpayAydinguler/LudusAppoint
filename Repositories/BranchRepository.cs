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

        public void CreateBranch(Branch branch)
        {
            Create(branch);
        }

        public IEnumerable<Branch> GetAllBranches(bool trackChanges)
        {
            return GetAll(trackChanges);
        }

        public int GetReservationInAdvanceDayLimit(int branchId)
        {
            return _repositoryContext.Branches.Where(b => b.BranchId == branchId)
                                              .AsNoTracking()
                                              .Select(b => b.ReservationInAdvanceDayLimit)
                                              .FirstOrDefault();

        }
    }
}
