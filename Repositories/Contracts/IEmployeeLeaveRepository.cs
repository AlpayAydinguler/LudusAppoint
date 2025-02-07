using Entities.Dtos;
using Entities.Models;

namespace Repositories.Contracts
{
    public interface IEmployeeLeaveRepository : IRepositoryBase<EmployeeLeave>
    {
        public ICollection<EmployeeLeave> GetAllEmployeeLeaves(bool trackChanges);
        void CreateEmployeeLeave(EmployeeLeave model);
        object GetLeaveTimes(int employeeId, int reservationInAdvanceDayLimit);
        bool HasOverlappingAppointments(int employeeId, DateTime leaveStart, DateTime leaveEnd);
        bool HasOverlappingLeaves(int employeeId, DateTime leaveStart, DateTime leaveEnd, int? employeeLeaveId);
    }
}
