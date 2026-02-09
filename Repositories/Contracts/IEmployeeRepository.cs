using Entities.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repositories.Contracts
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        Task CreateEmployeeAsync(Employee employee);
        Task<Employee> GetEmployeeAsync(Guid tenantId, int id, bool trackChanges, string language = "en-GB");
        Task<IEnumerable<Employee>> GetEmployeesForForCustomerAppointmentAsync(Guid tenantId, int branchId, List<int> offeredServiceIds, bool trackChanges);
        Task<IEnumerable<Employee>> GetAllEmployeesAsync(Guid tenantId, bool trackChanges);
        Task<Employee> GetEmployeeWorkingInfoAsync(Guid tenantId, int employeeId);
    }
}