using Entities.Dtos;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IEmployeeLeaveRepository : IRepositoryBase<EmployeeLeave>
    {
        Task<ICollection<EmployeeLeave>> GetAllEmployeeLeavesAsync(Guid tenantId, bool trackChanges);
        Task CreateEmployeeLeaveAsync(EmployeeLeave model);
        Task<object> GetLeaveTimesAsync(Guid tenantId, int employeeId, int reservationInAdvanceDayLimit);
        Task<bool> HasOverlappingAppointmentsAsync(Guid tenantId, int employeeId, DateTime leaveStart, DateTime leaveEnd);
        Task<bool> HasOverlappingLeavesAsync(Guid tenantId, int employeeId, DateTime leaveStart, DateTime leaveEnd, int? employeeLeaveId);
    }
}