using Entities.Dtos;
using Entities.Models;
using Entities.Models.Enums;

namespace Services.Contracts
{
    public interface IOfferedServiceService
    {
        IEnumerable<OfferedServiceDto> GetAllOfferedServices(bool trackChanges, string language = "en-GB");
        OfferedService? GetOfferedServiceById(int id, bool trackChanges);
        IEnumerable<OfferedServiceDto> GetActiveOfferedServices(bool trackChanges, string language = "en-GB");
        IEnumerable<OfferedService> GetAllForCustomerAppointment(Gender gender, int ageGroupId, bool trackChanges, string language = "en-GB");
        void CreateOfferedService(OfferedServiceDtoForInsert offeredServiceDtoForInsert);
        OfferedServiceDtoForUpdate? GetOfferedServiceForUpdate(int id, bool v);
        void UpdateOfferedService(OfferedServiceDtoForUpdate offeredServiceDtoForUpdate);
        void DeleteOfferedService(int id);
    }
}
