using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IBookingFlowConfigService
    {
        Task<BookingFlowConfigDto?> GetBookingFlowConfigForBranchAsync(Guid tenantId, int branchId);
        Task<IEnumerable<BookingFlowConfigDto>> GetAllBookingFlowConfigsByTenantIdAsync(Guid tenantId);
        Task UpdateBookingFlowConfigAsync(BookingFlowConfigDtoForUpdate configDto);
    }
}
