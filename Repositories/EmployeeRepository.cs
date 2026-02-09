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

        public async Task CreateEmployeeAsync(Employee employee)
        {
            await CreateAsync(employee);
        }

        public async Task<IEnumerable<Employee>> GetAllEmployeesAsync(Guid tenantId, bool trackChanges)
        {
            var employees = _repositoryContext.Employees
                .Include(h => h.Branch)
                .Where(e => e.TenantId == tenantId); // Tenant filter

            return trackChanges ?
                await employees.ToListAsync() :
                await employees.AsNoTracking().ToListAsync();
        }

        public async Task<Employee> GetEmployeeAsync(Guid tenantId, int id, bool trackChanges, string language)
        {
            var employeeQuery = _repositoryContext.Employees
                .Include(h => h.Branch)
                .Include(h => h.OfferedServices)
                    .ThenInclude(hs => hs.OfferedServiceLocalizations)
                .Where(h => h.EmployeeId == id && h.TenantId == tenantId); // Tenant filter

            var employee = trackChanges ?
                await employeeQuery.SingleOrDefaultAsync() :
                await employeeQuery.AsNoTracking().SingleOrDefaultAsync();

            if (employee == null)
                return null;

            // Process localizations
            employee.OfferedServices = employee.OfferedServices.Select(hs => new OfferedService
            {
                OfferedServiceId = hs.OfferedServiceId,
                OfferedServiceName = hs.OfferedServiceLocalizations.FirstOrDefault(l => l.Language == language)?.OfferedServiceLocalizationName ?? hs.OfferedServiceName
            }).ToList();

            return employee;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesForForCustomerAppointmentAsync(Guid tenantId, int branchId, List<int> offeredServiceIds, bool trackChanges)
        {
            var employees = _repositoryContext.Employees
                .Where(h => h.BranchId == branchId && h.TenantId == tenantId) // Tenant filter
                .Where(h => offeredServiceIds.All(id => h.OfferedServices.Any(hs => hs.OfferedServiceId == id)));

            return trackChanges ?
                await employees.ToListAsync() :
                await employees.AsNoTracking().ToListAsync();
        }

        public async Task<Employee> GetEmployeeWorkingInfoAsync(Guid tenantId, int employeeId)
        {
            var employeeQuery = await _repositoryContext.Employees
                .Where(h => h.EmployeeId == employeeId && h.TenantId == tenantId) // Tenant filter
                .Select(h => new Employee
                {
                    StartOfWorkingHours = h.StartOfWorkingHours,
                    EndOfWorkingHours = h.EndOfWorkingHours,
                    DayOff = h.DayOff
                })
                .AsNoTracking()
                .FirstOrDefaultAsync();

            return employeeQuery;
        }
    }
}