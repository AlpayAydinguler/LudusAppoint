using Entities.Dtos;
using Entities.Models;

namespace Services.Contracts
{
    public interface IEmployeeService
    {
        void CreateEmployee(EmployeeDtoForInsert employeeDtoForInsert);
        void DeleteEmployee(int id);
        IEnumerable<EmployeeDto> GetAllEmployees(bool trackChanges);
        IEnumerable<Employee> GetEmployeesForCustomerAppointment(int branchId, List<int> offeredServiceIds, bool trackChanges);
        EmployeeDtoForUpdate GetOneEmployeeForUpdate(int id, bool trackChanges, string language);
        void UpdateEmployee(EmployeeDtoForUpdate employeeDtoForUpdate);
    }
}
