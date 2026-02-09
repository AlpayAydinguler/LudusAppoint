using Entities.Dtos;
using Entities.Models;
using Entities.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories
{
    public class EmployeeLeaveRepository : RepositoryBase<EmployeeLeave>, IEmployeeLeaveRepository
    {
        public EmployeeLeaveRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<ICollection<EmployeeLeave>> GetAllEmployeeLeavesAsync(Guid tenantId, bool trackChanges)
        {
            var employeeLeaves = _repositoryContext.EmployeeLeaves
                .Include(h => h.Employee)
                .Where(h => h.TenantId == tenantId); // Tenant filter

            return trackChanges ?
                await employeeLeaves.ToListAsync() :
                await employeeLeaves.AsNoTracking().ToListAsync();
        }

        public async Task CreateEmployeeLeaveAsync(EmployeeLeave model)
        {
            await CreateAsync(model);
        }

        public async Task<object> GetLeaveTimesAsync(Guid tenantId, int employeeId, int reservationInAdvanceDayLimit)
        {
            var maxDate = DateTime.Today.AddDays(reservationInAdvanceDayLimit + 1);

            var leaveTimes = await _repositoryContext.EmployeeLeaves
                .Where(hl => hl.EmployeeId == employeeId
                          && hl.TenantId == tenantId // Tenant filter
                          && hl.LeaveStartDateTime >= DateTime.Today
                          && hl.LeaveStartDateTime < maxDate)
                .Select(hl => new
                {
                    hl.LeaveStartDateTime,
                    hl.LeaveEndDateTime
                })
                .AsNoTracking()
                .ToListAsync();

            return leaveTimes;
        }

        public async Task<bool> HasOverlappingAppointmentsAsync(Guid tenantId, int employeeId, DateTime leaveStart, DateTime leaveEnd)
        {
            var hasActiveAppointments = await _repositoryContext.CustomerAppointments
                .AsNoTracking()
                .AnyAsync(h =>
                    h.EmployeeId == employeeId
                    && h.TenantId == tenantId // Tenant filter
                    && h.StartDateTime < leaveEnd
                    && h.StartDateTime + h.ApproximateDuration > leaveStart
                    && h.Status != CustomerAppointmentStatus.Completed
                    && h.Status != CustomerAppointmentStatus.Cancelled
                );
            return hasActiveAppointments;
        }

        public async Task<bool> HasOverlappingLeavesAsync(Guid tenantId, int employeeId, DateTime leaveStart, DateTime leaveEnd, int? employeeLeaveId)
        {
            bool hasOverlappingLeaves;
            var query = _repositoryContext.EmployeeLeaves
                .Where(h =>
                    h.EmployeeId == employeeId
                    && h.TenantId == tenantId // Tenant filter
                    && h.LeaveStartDateTime < leaveEnd
                    && h.LeaveEndDateTime > leaveStart
                );

            if (employeeLeaveId != null)
            {
                query = query.Where(h => h.EmployeeLeaveId != employeeLeaveId);
            }

            hasOverlappingLeaves = await query.AnyAsync();
            return hasOverlappingLeaves;
        }
    }
}