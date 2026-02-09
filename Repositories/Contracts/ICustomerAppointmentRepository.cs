using Entities.Models;

namespace Repositories.Contracts
{
    public interface ICustomerAppointmentRepository : IRepositoryBase<CustomerAppointment>
    {
        Task<List<CustomerAppointment>> GetAllCustomerAppointmentsAsync(bool trackChanges, string language = "en-GB");
        Task<bool> EmployeeHaveAppointmentAsync(EmployeeLeave employeeLeave);
        Task<IEnumerable<CustomerAppointment>> GetPendingCustomerAppointmentsAsync(bool trackChanges, string language = "en-GB");
        object GetReservedDaysTimes(int employeeId, int reservationInAdvanceDayLimit);
        Task<CustomerAppointment> GetCustomerAppointmentForUpdateAsync(int id, bool trackChanges, string language);
    }
}
