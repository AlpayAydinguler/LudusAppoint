using Entities.Dtos;
using Entities.Models;
using Entities.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public class EmployeeLeaveRepository : RepositoryBase<EmployeeLeave>, IEmployeeLeaveRepository
    {
        public EmployeeLeaveRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public ICollection<EmployeeLeave> GetAllEmployeeLeaves(bool trackChanges)
        {
            var employeeLeaves = _repositoryContext.EmployeeLeaves.Include(h => h.Employee);
            return trackChanges ? employeeLeaves.ToList() : employeeLeaves.AsNoTracking().ToList();
        }
        public void CreateEmployeeLeave(EmployeeLeave model)
        {
            Create(model);
        }

        public object GetLeaveTimes(int employeeId, int reservationInAdvanceDayLimit)
        {
            var maxDate = DateTime.Today.AddDays(reservationInAdvanceDayLimit + 1); // Includes the entire day of the last allowed day

            var leaveTimes = _repositoryContext.EmployeeLeaves.Where(hl => hl.EmployeeId == employeeId &&
                                                                     hl.LeaveStartDateTime >= DateTime.Today &&
                                                                     hl.LeaveStartDateTime < maxDate)
                                                              .Select(hl => new
                                                              {
                                                                  hl.LeaveStartDateTime,
                                                                  hl.LeaveEndDateTime
                                                              })
                                                              .AsNoTracking()
                                                              .ToList();

            return leaveTimes;
        }

        public bool HasOverlappingAppointments(int employeeId, DateTime leaveStart, DateTime leaveEnd)
        {
            var hasActiveAppointments = _repositoryContext.CustomerAppointments.AsEnumerable() // Switch to client-side evaluation
                                                                               .Any(h =>
                                                                                   h.EmployeeId == employeeId &&
                                                                                   h.StartDateTime < leaveEnd &&
                                                                                   h.StartDateTime + h.ApproximateDuration > leaveStart &&
                                                                                   h.Status != CustomerAppointmentStatus.Completed &&
                                                                                   h.Status != CustomerAppointmentStatus.Cancelled
                                                                               );
            return hasActiveAppointments;
        }

        public bool HasOverlappingLeaves(int employeeId, DateTime leaveStart, DateTime leaveEnd, int? employeeLeaveId)
        {
            bool hasOverlappingLeaves;
            if (employeeLeaveId != null)
            {
                hasOverlappingLeaves = _repositoryContext.EmployeeLeaves.Any(h =>
                    h.EmployeeId == employeeId &&
                    h.LeaveStartDateTime < leaveEnd &&
                    h.LeaveEndDateTime > leaveStart &&
                    h.EmployeeLeaveId != employeeLeaveId
                );
            }
            else
            {
                hasOverlappingLeaves = _repositoryContext.EmployeeLeaves.Any(h =>
                h.EmployeeId == employeeId &&
                h.LeaveStartDateTime < leaveEnd &&
                h.LeaveEndDateTime > leaveStart
            );
            }
            return hasOverlappingLeaves;
        }
    }
}
