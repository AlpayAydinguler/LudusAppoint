using Entities.Models;

namespace Services.Contracts
{
    public interface IEmployeeService
    {
        void CreateEmployee(Employee employee);
        IEnumerable<Employee> GetAllEmployees(bool trackChanges);
        IEnumerable<Employee> GetEmployeesForForCustomerAppointment(int branchId, List<int> offeredServiceIds, bool trackChanges);
        Employee GetOneEmployee(int id, bool trackChanges, string language);
        public Employee? GetOneEmployee(int id);
        void UpdateEmployee(Employee employee, int[] selectedServiceIds);
    }
}
