
using Entities.Models;

namespace Services.Contracts
{
    public interface IEmployeeLeaveService
    {
        void CreateEmployeeLeave(EmployeeLeave model);
        ICollection<EmployeeLeave> GetAllEmployeeLeaves(bool v);
    }
}
