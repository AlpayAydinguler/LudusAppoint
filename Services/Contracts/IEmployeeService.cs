using Entities.Dtos;
using Entities.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Contracts
{
    public interface IEmployeeService
    {
        Task CreateEmployeeAsync(EmployeeDtoForInsert employeeDtoForInsert);
        Task DeleteEmployeeAsync(int id);
        Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(bool trackChanges);
        Task<IEnumerable<Employee>> GetEmployeesForCustomerAppointmentAsync(int branchId, List<int> offeredServiceIds, bool trackChanges);
        Task<EmployeeDtoForUpdate> GetOneEmployeeForUpdateAsync(int id, bool trackChanges, string language);
        Task UpdateEmployeeAsync(EmployeeDtoForUpdate employeeDtoForUpdate);
    }
}