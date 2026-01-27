using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IBookingFlowConfigRepository : IRepositoryBase<BookingFlowConfig>
    {
        Task<IEnumerable<BookingFlowConfig>> GetAllBookingFlowConfigByTenantIdAsync(Guid tenantId, bool trackChanges);
        Task<BookingFlowConfig?> GetBookingFlowConfigByBranchIdForUpdateAsync(Guid tenantId, int branchId, bool trackChanges);
    }
}
