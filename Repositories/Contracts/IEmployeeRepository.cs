using Entities.Models;

namespace Repositories.Contracts
{
    public interface IEmployeeRepository : IRepositoryBase<Employee>
    {
        void CreateEmployee(Employee employee);
        Employee GetEmployee(int id, bool trackChanges, string language = "en-GB");
        IEnumerable<Employee> GetEmployeesForForCustomerAppointment(int branchId, List<int> offeredServiceIds, bool trackChanges);
        IEnumerable<Employee> GetAllEmployees(bool trackChanges);
        Employee GetEmployeeWorkingInfo(int employeeId);
    }
}
