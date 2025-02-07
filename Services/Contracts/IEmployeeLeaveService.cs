
using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IEmployeeLeaveService
    {
        void CreateEmployeeLeave(EmployeeLeaveDtoForInsert employeeLeaveDtoForInsert);
        void DeleteEmployeeLeave(int id);
        ICollection<EmployeeLeaveDto> GetAllEmployeeLeaves(bool v);
        EmployeeLeaveDtoForUpdate GetEmployeeLeaveForUpdate(int id);
        void UpdateEmployeeLeave(EmployeeLeaveDtoForUpdate employeeLeaveDtoForUpdate);
    }
}
