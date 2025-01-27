using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class CustomerAppointmentManager : ICustomerAppointmentService
    {
        private readonly IRepositoryManager _repositoryManager;

        public CustomerAppointmentManager(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public void CreateAppointment(CustomerAppointment customerAppointment, int[] offeredServiceIds)
        {
            foreach (var offeredServiceId in offeredServiceIds)
            {
                var offeredService = _repositoryManager.OfferedServiceRepository.GetofferedService(offeredServiceId, false);
                _repositoryManager.OfferedServiceRepository.AttachAsUnchanged(offeredService);
                customerAppointment.OfferedServices.Add(offeredService);
            }
            _repositoryManager.CustomerAppointmentRepository.Create(customerAppointment);
            _repositoryManager.Save();
        }

        public IEnumerable<CustomerAppointment> GetAllCustomerAppointments(bool trackChanges, string language = "en-GB")
        {
            return _repositoryManager.CustomerAppointmentRepository.GetAllCustomerAppointments(trackChanges, language);
        }

        public CustomerAppointment? GetOneCustomerAppointment(int id, bool trackChanges)
        {
            return _repositoryManager.CustomerAppointmentRepository.FindByCondition(x => x.CustomerAppointmentId.Equals(id), trackChanges);
        }

        public IEnumerable<CustomerAppointment> GetPendingCustomerAppointments(bool trackChanges, string language = "en-GB")
        {
            return _repositoryManager.CustomerAppointmentRepository.GetPendingCustomerAppointments(trackChanges, language);
        }

        public object GetReservedDaysTimes(int employeeId, int branchId)
        {
            var reservationInAdvanceDayLimit = _repositoryManager.BranchRepository.GetReservationInAdvanceDayLimit(branchId);
            var employeeLeaves = _repositoryManager.EmployeeLeaveRepository.GetLeaveTimes(employeeId);
            var reservedDaysTimes = _repositoryManager.CustomerAppointmentRepository.GetReservedDaysTimes(employeeId);
            var minimumServiceDuration = _repositoryManager.OfferedServiceRepository.GetMinApproximateDuration();
            return new
            {
                reservationInAdvanceDayLimit,
                minimumServiceDuration,
                employeeLeaves,
                reservedDaysTimes
            };
        }
    }
}
