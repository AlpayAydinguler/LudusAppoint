using Entities.Models;
using Repositories.Contracts;
using Services.Contracts;

namespace Services
{
    public class EmployeeManager : IEmployeeService
    {
        private readonly IRepositoryManager _repositoryManager;

        public EmployeeManager(IRepositoryManager repositoryManager)
        {
            _repositoryManager = repositoryManager;
        }

        public void CreateEmployee(Employee employee)
        {
            _repositoryManager.EmployeeRepository.CreateEmployee(employee);
            _repositoryManager.Save();
        }

        public IEnumerable<Employee> GetAllEmployees(bool trackChanges)
        {
            return _repositoryManager.EmployeeRepository.GetAllEmployees(trackChanges);
        }

        public IEnumerable<Employee> GetEmployeesForForCustomerAppointment(int branchId, List<int> offeredServiceIds, bool trackChanges)
        {
            return _repositoryManager.EmployeeRepository.GetEmployeesForForCustomerAppointment(branchId, offeredServiceIds, trackChanges);
        }

        public Employee? GetOneEmployee(int id, bool trackChanges, string language = "en-GB")
        {
            return _repositoryManager.EmployeeRepository.GetEmployee(id, trackChanges, language);
        }
        public Employee? GetOneEmployee(int id)
        {
            return _repositoryManager.EmployeeRepository.FindByCondition(h => h.EmployeeId.Equals(id), false);
        }

        public void UpdateEmployee(Employee employee, int[] selectedServiceIds)
        {
            var model = _repositoryManager.EmployeeRepository.GetEmployee(employee.EmployeeId, true);
            if (model != null)
            {
                model.EmployeeName = employee.EmployeeName;
                model.EmployeeSurname = employee.EmployeeSurname;
                model.CanTakeClients = employee.CanTakeClients;
                model.OfferedServices.Clear();
                foreach (var serviceId in selectedServiceIds)
                {
                    var service = _repositoryManager.OfferedServiceRepository.GetofferedService(serviceId, true);
                    if (service != null)
                    {
                        model.OfferedServices.Add(service);
                    }
                }
                model.BranchId = employee.BranchId;
                model.StartOfWorkingHours = employee.StartOfWorkingHours;
                model.EndOfWorkingHours = employee.EndOfWorkingHours;
                model.DayOff = employee.DayOff;
            }
            _repositoryManager.Save();
        }
    }
}
