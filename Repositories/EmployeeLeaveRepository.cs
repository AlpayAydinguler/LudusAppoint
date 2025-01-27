using Entities.Models;
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

        public object GetLeaveTimes(int employeeId)
        {
            var leaveTimes = _repositoryContext.EmployeeLeaves.Where(hl => hl.EmployeeId == employeeId)
                                                                 .Select(hl => new
                                                                 {
                                                                     hl.LeaveStartDateTime,
                                                                     hl.LeaveEndDateTime
                                                                 })
                                                                 .AsNoTracking()
                                                                 .ToList();
            return leaveTimes;
        }
    }
}
