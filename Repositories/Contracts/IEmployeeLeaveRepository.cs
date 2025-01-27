using Entities.Models;

namespace Repositories.Contracts
{
    public interface IEmployeeLeaveRepository : IRepositoryBase<EmployeeLeave>
    {
        public ICollection<EmployeeLeave> GetAllEmployeeLeaves(bool trackChanges);
        void CreateEmployeeLeave(EmployeeLeave model);
        object GetLeaveTimes(int employeeId);
    }
}
