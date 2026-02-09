using Entities.Dtos;
using Entities.Models;
using Entities.Models.Enums;

namespace Services.Contracts
{
    public interface IOfferedServiceService
    {
        Task<IEnumerable<OfferedServiceDto>> GetAllOfferedServicesAsync(bool trackChanges, string language = "en-GB");
        Task<OfferedService?> GetOfferedServiceByIdAsync(int id, bool trackChanges);
        Task<IEnumerable<OfferedServiceDto>> GetActiveOfferedServicesAsync(bool trackChanges, string language = "en-GB");
        Task<IEnumerable<OfferedService>> GetAllForCustomerAppointmentAsync(Gender gender, int ageGroupId, bool trackChanges, string language = "en-GB");
        Task CreateOfferedServiceAsync(OfferedServiceDtoForInsert offeredServiceDtoForInsert);
        Task<OfferedServiceDtoForUpdate?> GetOfferedServiceForUpdateAsync(int id, bool trackChanges);
        Task UpdateOfferedServiceAsync(OfferedServiceDtoForUpdate offeredServiceDtoForUpdate);
        Task DeleteOfferedServiceAsync(int id);
    }
}