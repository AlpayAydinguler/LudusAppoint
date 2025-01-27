using Entities.Models;
using Entities.Models.Enums;

namespace Repositories.Contracts
{
    public interface IOfferedServiceRepository : IRepositoryBase<OfferedService>
    {
        OfferedService GetofferedService(int id, bool trackChanges);
        List<OfferedService> GetAllOfferedServices(bool trackChanges, string language = "en");
        IEnumerable<OfferedService> GetActiveOfferedServices(bool trackChanges, string language);
        void CreateofferedService(OfferedService offeredService);
        IEnumerable<OfferedService> GetAllForCustomerAppointment(Gender gender, int ageGroupId, bool trackChanges, string language);
        int GetMinApproximateDuration();
        void AttachAsUnchanged(OfferedService offeredService);
    }
}
