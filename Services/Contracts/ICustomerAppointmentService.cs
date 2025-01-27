using Entities.Models;

namespace Services.Contracts
{
    public interface ICustomerAppointmentService
    {
        void CreateAppointment(CustomerAppointment customerAppointment, int[] offeredServiceIds);
        IEnumerable<CustomerAppointment> GetAllCustomerAppointments(bool trackChanges, string language = "en-GB");
        CustomerAppointment? GetOneCustomerAppointment(int id, bool trackChanges);
        IEnumerable<CustomerAppointment> GetPendingCustomerAppointments(bool trackChanges, string language = "en-GB");
        object GetReservedDaysTimes(int employeeId, int branchId);
    }
}
