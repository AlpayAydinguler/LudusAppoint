using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;

namespace Services.Contracts
{
    public interface ICustomerAppointmentService
    {
        void CreateAppointment(CustomerAppointment customerAppointment, int[] offeredServiceIds);
        Task CreateCustomerAppointmentAsync(CustomerAppointmentDtoForInsert customerAppointmentDtoForInsert);
        IEnumerable<CustomerAppointmentDto> GetAllCustomerAppointments(bool trackChanges, string language = "en-GB");
        Task<CustomerAppointmentDtoForUpdate> GetCustomerAppointmentForUpdateAsync(int id, bool trackChangesstring,string language = "en-GB");
        CustomerAppointment? GetOneCustomerAppointment(int id, bool trackChanges);
        IEnumerable<CustomerAppointmentDto> GetPendingCustomerAppointments(bool trackChanges, string language = "en-GB");
        JsonResult GetReservedDaysTimes(int employeeId, int branchId);
        Task UpdateCustomerAppointmentAsync(CustomerAppointmentDtoForUpdate customerAppointmentDtoForUpdate);
    }
}
