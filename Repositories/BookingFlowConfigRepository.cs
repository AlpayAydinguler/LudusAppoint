using Entities.Models;
using Repositories.Contracts;

namespace Repositories
{
    public class BookingFlowConfigRepository : RepositoryBase<BookingFlowConfig>, IBookingFlowConfigRepository
    {
        public BookingFlowConfigRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<IEnumerable<BookingFlowConfig>> GetAllBookingFlowConfigByTenantIdAsync(Guid tenantId, bool trackChanges)
        {
            return await GetAllByConditionAsync(b => b.TenantId == tenantId, trackChanges);
        }

        public async Task<BookingFlowConfig> GetBookingFlowConfigByBranchIdForUpdateAsync(Guid tenantId, int branchId, bool trackChanges)
        {
            return await FindByConditionAsync(b => b.TenantId == tenantId && b.BranchId == branchId, trackChanges);
        }
    }
}
