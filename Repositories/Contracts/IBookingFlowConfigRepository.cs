using Entities.Models;

namespace Repositories.Contracts
{
    public interface IBookingFlowConfigRepository : IRepositoryBase<BookingFlowConfig>
    {
        Task<IEnumerable<BookingFlowConfig>> GetAllBookingFlowConfigByTenantIdAsync(Guid tenantId, bool trackChanges);
        Task<BookingFlowConfig?> GetBookingFlowConfigByBranchIdForUpdateAsync(Guid tenantId, int branchId, bool trackChanges);
    }
}
