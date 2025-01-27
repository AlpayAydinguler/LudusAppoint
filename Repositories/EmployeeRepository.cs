using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;

namespace Repositories
{
    public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public void CreateEmployee(Employee employee)
        {
            Create(employee);
        }

        public IEnumerable<Employee> GetAllEmployees(bool trackChanges)
        {
            var employees = _repositoryContext.Employees.Include(h => h.Branch);
            return trackChanges ? employees : employees.AsNoTracking();
        }

        public Employee GetEmployee(int id, bool trackChanges, string language)
        {
            // Eagerly load the required data
            var employeeQuery = _repositoryContext.Employees.Include(h => h.Branch)
                                                                  .Include(h => h.OfferedServices)
                                                                      .ThenInclude(hs => hs.OfferedServiceLocalizations)
                                                                  .Where(h => h.EmployeeId == id);


            var employee = trackChanges ? employeeQuery.SingleOrDefault() : employeeQuery.AsNoTracking().SingleOrDefault();

            if (employee == null)
                return new Employee();

            // Perform the projection in memory
            employee.OfferedServices = employee.OfferedServices.Select(hs => new OfferedService
            {
                OfferedServiceId = hs.OfferedServiceId,
                OfferedServiceName = hs.OfferedServiceLocalizations.FirstOrDefault(l => l.Language == language)?.OfferedServiceLocalizationName ?? hs.OfferedServiceName
            }).ToList();

            return employee ?? new Employee();
        }

        public IEnumerable<Employee> GetEmployeesForForCustomerAppointment(int branchId, List<int> offeredServiceIds, bool trackChanges)
        {
            var employees = _repositoryContext.Employees.Where(h => h.BranchId == branchId)
                                                              .Where(h => offeredServiceIds.All(id => h.OfferedServices.Any(hs => hs.OfferedServiceId == id)));


            return trackChanges ? employees : employees.AsNoTracking();
        }

        public Employee GetEmployeeWorkingInfo(int employeeId)
        {
            var employeeQuery = _repositoryContext.Employees.Where(h => h.EmployeeId.Equals(employeeId))
                                                                  .Select(h => new Employee
                                                                  {
                                                                      StartOfWorkingHours = h.StartOfWorkingHours,
                                                                      EndOfWorkingHours = h.EndOfWorkingHours,
                                                                      DayOff = h.DayOff
                                                                  })
                                                                  .AsNoTracking()
                                                                  .FirstOrDefault();
            return employeeQuery;
        }
    }
}
