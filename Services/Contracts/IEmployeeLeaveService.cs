using Entities.Dtos;

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