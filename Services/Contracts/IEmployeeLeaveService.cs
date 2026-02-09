using Entities.Dtos;
using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IEmployeeLeaveService
    {
        Task CreateEmployeeLeaveAsync(EmployeeLeaveDtoForInsert employeeLeaveDtoForInsert);
        Task DeleteEmployeeLeaveAsync(int id);
        Task<ICollection<EmployeeLeaveDto>> GetAllEmployeeLeavesAsync(bool trackChanges);
        Task<EmployeeLeaveDtoForUpdate> GetEmployeeLeaveForUpdateAsync(int id);
        Task UpdateEmployeeLeaveAsync(EmployeeLeaveDtoForUpdate employeeLeaveDtoForUpdate);
    }
}