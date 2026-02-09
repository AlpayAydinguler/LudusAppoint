using Entities.Dtos;

namespace Services.Contracts
{
    public interface IBookingFlowConfigService
    {
        Task<BookingFlowConfigDto?> GetBookingFlowConfigForBranchAsync(int branchId);
        Task<IEnumerable<BookingFlowConfigDto>> GetAllBookingFlowConfigsAsync();
        Task UpdateBookingFlowConfigAsync(BookingFlowConfigDtoForUpdate configDto);
    }
}