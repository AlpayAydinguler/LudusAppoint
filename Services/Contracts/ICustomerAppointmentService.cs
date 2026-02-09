using Entities.Dtos;
using Entities.Models;
using Entities.Models.Enums;

namespace Services.Contracts
{
    public interface ICustomerAppointmentService
    {
        Task ChangeStatusAsync(int id, CustomerAppointmentStatus newStatus);
        Task CreateCustomerAppointmentAsync(CustomerAppointmentDtoForInsert customerAppointmentDtoForInsert);
        Task<IEnumerable<CustomerAppointmentDto>> GetAllCustomerAppointmentsAsync(bool trackChanges, string language = "en-GB");
        Task<CustomerAppointmentDtoForUpdate> GetCustomerAppointmentForUpdateAsync(int id, bool trackChanges, string language = "en-GB");
        Task<CustomerAppointment?> GetOneCustomerAppointmentAsync(int id, bool trackChanges);
        Task<IEnumerable<CustomerAppointmentDto>> GetPendingCustomerAppointmentsAsync(bool trackChanges, string language = "en-GB");
        Task<object> GetReservedDaysTimesAsync(int employeeId, int branchId); // Changed to async and returns Task<object>
        Task UpdateCustomerAppointmentAsync(CustomerAppointmentDtoForUpdate customerAppointmentDtoForUpdate);
    }
}