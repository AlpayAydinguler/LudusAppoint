using Entities.Models;
using Entities.Models.Enums;

namespace Repositories.Contracts
{
    public interface IOfferedServiceRepository : IRepositoryBase<OfferedService>
    {
        Task<OfferedService> GetofferedServiceAsync(int id, bool trackChanges);
        Task<List<OfferedService>> GetAllOfferedServicesAsync(bool trackChanges, string language = "en");
        Task<IEnumerable<OfferedService>> GetActiveOfferedServicesAsync(bool trackChanges, string language);
        Task<IEnumerable<OfferedService>> GetAllForCustomerAppointmentAsync(Gender gender, int ageGroupId, bool trackChanges, string language);
        Task<int> GetMinApproximateDurationAsync();
        Task AttachAsUnchangedAsync(OfferedService offeredService);
        Task<OfferedService> GetOfferedServiceForUpdateAsync(int id, bool trackChanges);
        Task UpdateAsync(OfferedService offeredService, List<int> ageGroupIds);
    }
}
