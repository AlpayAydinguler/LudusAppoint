using Entities.Models;

namespace Repositories.Contracts
{
    public interface ICustomerAppointmentRepository : IRepositoryBase<CustomerAppointment>
    {
        public List<CustomerAppointment> GetAllCustomerAppointments(bool trackChanges, string language = "en-GB");
        bool EmployeeHaveAppointment(EmployeeLeave employeeLeave);
        IEnumerable<CustomerAppointment> GetPendingCustomerAppointments(bool trackChanges, string language = "en-GB");
        object GetReservedDaysTimes(int employeeId, int reservationInAdvanceDayLimit);
    }
}
