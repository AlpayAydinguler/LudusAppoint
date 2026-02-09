using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Repositories.Contracts;
using Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class EmployeeManager : IEmployeeService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<EmployeeManager> _localizer;
        private readonly ITenantService _tenantService; // Added

        public EmployeeManager(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            IStringLocalizer<EmployeeManager> localizer,
            ITenantService tenantService) // Added
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _localizer = localizer;
            _tenantService = tenantService;
        }

        public async Task CreateEmployeeAsync(EmployeeDtoForInsert employeeDtoForInsert)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var employee = _mapper.Map<Employee>(employeeDtoForInsert);
            employee.TenantId = currentTenant.Id; // Set tenant ID from context

            // Get offered services for current tenant only
            var existingOfferedServices = await _repositoryManager.OfferedServiceRepository
                .GetAllByConditionAsync(h =>
                    employeeDtoForInsert.OfferedServiceIds.Contains(h.OfferedServiceId)
                    && h.TenantId == currentTenant.Id,
                    true);

            employee.OfferedServices = existingOfferedServices.ToList();
            await _repositoryManager.EmployeeRepository.CreateEmployeeAsync(employee);
            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllEmployeesAsync(bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<EmployeeDto>();
            }

            // Get employees for current tenant only
            var employees = await _repositoryManager.EmployeeRepository
                .GetAllByConditionAsync(e => e.TenantId == currentTenant.Id, trackChanges);

            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return employeeDtos;
        }

        public async Task<IEnumerable<Employee>> GetEmployeesForCustomerAppointmentAsync(int branchId, List<int> offeredServiceIds, bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<Employee>();
            }

            // Get employees for current tenant only
            return await _repositoryManager.EmployeeRepository
                .GetAllByConditionAsync(e =>
                    e.TenantId == currentTenant.Id
                    && e.BranchId == branchId
                    && offeredServiceIds.All(id => e.OfferedServices.Any(os => os.OfferedServiceId == id)),
                    trackChanges);
        }

        private async Task<Employee> GetOneEmployeeAsync(int id, bool trackChanges, string language = "en-GB")
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return null;
            }

            // Get employee for current tenant only
            return await _repositoryManager.EmployeeRepository
                .FindByConditionAsync(e =>
                    e.EmployeeId == id
                    && e.TenantId == currentTenant.Id,
                    trackChanges);
        }

        private async Task<Employee> GetOneEmployeeByIdAsync(int id)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return null;
            }

            // Get employee for current tenant only
            return await _repositoryManager.EmployeeRepository
                .FindByConditionAsync(e =>
                    e.EmployeeId == id
                    && e.TenantId == currentTenant.Id,
                    false);
        }

        public async Task<EmployeeDtoForUpdate> GetOneEmployeeForUpdateAsync(int id, bool trackChanges, string language)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var employee = await GetOneEmployeeAsync(id, trackChanges, language);

            if (employee == null)
            {
                throw new ValidationException(_localizer["EmployeeNotFound"]);
            }

            var employeeDtoForUpdate = _mapper.Map<EmployeeDtoForUpdate>(employee);
            return employeeDtoForUpdate;
        }

        public async Task UpdateEmployeeAsync(EmployeeDtoForUpdate employeeDtoForUpdate)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            // Get existing employee with tenant check
            var existingEmployee = await _repositoryManager.EmployeeRepository
                .FindByConditionAsync(e =>
                    e.EmployeeId == employeeDtoForUpdate.EmployeeId
                    && e.TenantId == currentTenant.Id,
                    true,
                    include: q => q.Include(h => h.OfferedServices));

            if (existingEmployee == null)
            {
                throw new ValidationException(_localizer["EmployeeNotFound"]);
            }

            // Map updates to existing employee
            _mapper.Map(employeeDtoForUpdate, existingEmployee);
            existingEmployee.OfferedServices.Clear();

            // Get offered services for current tenant only
            foreach (var offeredServiceId in employeeDtoForUpdate.OfferedServiceIds)
            {
                var offeredService = await _repositoryManager.OfferedServiceRepository
                    .FindByConditionAsync(h =>
                        h.OfferedServiceId == offeredServiceId
                        && h.TenantId == currentTenant.Id,
                        true);

                if (offeredService != null)
                {
                    existingEmployee.OfferedServices.Add(offeredService);
                }
            }

            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var employee = await GetOneEmployeeByIdAsync(id);

            if (employee == null)
            {
                throw new ValidationException(_localizer["EmployeeNotFound"]);
            }

            // Security check - ensure employee belongs to current tenant
            if (employee.TenantId != currentTenant.Id)
            {
                throw new ValidationException(_localizer["CannotDeleteEmployeeFromAnotherTenant"]);
            }

            try
            {
                await _repositoryManager.EmployeeRepository.DeleteAsync(employee);
                await _repositoryManager.SaveAsync();
            }
            catch (Exception exception)
            {
                if (exception.InnerException is SqlException sqlEx && sqlEx.Number == 547)
                {
                    throw new ValidationException(_localizer["EmployeeCannotBeDeletedBecauseItIsUsedInAnotherEntity"] + ".", new Exception() { Source = "Model" });
                }
                throw;
            }
        }
    }
}