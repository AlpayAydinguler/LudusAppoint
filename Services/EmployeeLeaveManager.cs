using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.Extensions.Localization;
using Repositories.Contracts;
using Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class EmployeeLeaveManager : IEmployeeLeaveService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IStringLocalizer<EmployeeLeaveManager> _localizer;
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService;

        public EmployeeLeaveManager(
            IRepositoryManager repositoryManager,
            IStringLocalizer<EmployeeLeaveManager> localizer,
            IMapper mapper,
            ITenantService tenantService)
        {
            _repositoryManager = repositoryManager;
            _localizer = localizer;
            _mapper = mapper;
            _tenantService = tenantService;
        }

        public async Task CreateEmployeeLeaveAsync(EmployeeLeaveDtoForInsert employeeLeaveDtoForInsert)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            await ValidateEmployeeLeaveAsync(employeeLeaveDtoForInsert, null, currentTenant.Id);

            var employeeLeave = _mapper.Map<EmployeeLeave>(employeeLeaveDtoForInsert);
            employeeLeave.TenantId = currentTenant.Id;

            await _repositoryManager.EmployeeLeaveRepository.CreateEmployeeLeaveAsync(employeeLeave);
            await _repositoryManager.SaveAsync();
        }

        public async Task DeleteEmployeeLeaveAsync(int id)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var employeeLeave = await _repositoryManager.EmployeeLeaveRepository.FindByConditionAsync(
                h => h.EmployeeLeaveId == id && h.TenantId == currentTenant.Id, // Tenant filter
                false
            );

            if (employeeLeave == null)
            {
                throw new ValidationException(_localizer["EmployeeLeaveNotFound"]);
            }

            await _repositoryManager.EmployeeLeaveRepository.DeleteAsync(employeeLeave);
            await _repositoryManager.SaveAsync();
        }

        public async Task<ICollection<EmployeeLeaveDto>> GetAllEmployeeLeavesAsync(bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return new List<EmployeeLeaveDto>();
            }

            // Pass tenantId to repository
            var employeeLeaves = await _repositoryManager.EmployeeLeaveRepository
                .GetAllEmployeeLeavesAsync(currentTenant.Id, trackChanges);

            var employeeLeavesDto = _mapper.Map<ICollection<EmployeeLeaveDto>>(employeeLeaves);
            return employeeLeavesDto;
        }

        public async Task<EmployeeLeaveDtoForUpdate> GetEmployeeLeaveForUpdateAsync(int id)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var employeeLeave = await _repositoryManager.EmployeeLeaveRepository.FindByConditionAsync(
                h => h.EmployeeLeaveId == id && h.TenantId == currentTenant.Id, // Tenant filter
                false
            );

            if (employeeLeave == null)
            {
                throw new ValidationException(_localizer["EmployeeLeaveNotFound"]);
            }

            var employeeLeaveDto = _mapper.Map<EmployeeLeaveDtoForUpdate>(employeeLeave);
            return employeeLeaveDto;
        }

        public async Task UpdateEmployeeLeaveAsync(EmployeeLeaveDtoForUpdate employeeLeaveDtoForUpdate)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            await ValidateEmployeeLeaveAsync(null, employeeLeaveDtoForUpdate, currentTenant.Id);

            // Get existing employee leave with tenant check
            var existingEmployeeLeave = await _repositoryManager.EmployeeLeaveRepository.FindByConditionAsync(
                h => h.EmployeeLeaveId == employeeLeaveDtoForUpdate.EmployeeLeaveId
                  && h.TenantId == currentTenant.Id,
                true
            );

            if (existingEmployeeLeave == null)
            {
                throw new ValidationException(_localizer["EmployeeLeaveNotFound"]);
            }

            // Map updates to existing entity
            _mapper.Map(employeeLeaveDtoForUpdate, existingEmployeeLeave);

            await _repositoryManager.EmployeeLeaveRepository.UpdateAsync(existingEmployeeLeave);
            await _repositoryManager.SaveAsync();
        }

        private async Task ValidateEmployeeLeaveAsync(
            EmployeeLeaveDtoForInsert? employeeLeaveDtoForInsert,
            EmployeeLeaveDtoForUpdate? employeeLeaveDtoForUpdate,
            Guid currentTenantId)
        {
            var model = (object?)employeeLeaveDtoForInsert ?? employeeLeaveDtoForUpdate;
            if (model == null)
                throw new ArgumentNullException(nameof(model), _localizer["DtoCannotBeNull"] + ".");

            var validationException = new List<ValidationException>();

            // Get common properties through reflection
            var employeeId = (int)model.GetType().GetProperty("EmployeeId")!.GetValue(model)!;
            var leaveStart = (DateTime)model.GetType().GetProperty("LeaveStartDateTime")!.GetValue(model)!;
            var leaveEnd = (DateTime)model.GetType().GetProperty("LeaveEndDateTime")!.GetValue(model)!;
            var employeeLeaveId = (int?)model.GetType().GetProperty("EmployeeLeaveId")?.GetValue(model);

            // Check for existing employee (must belong to same tenant)
            var employee = await _repositoryManager.EmployeeRepository.FindByConditionAsync(
                h => h.EmployeeId == employeeId && h.TenantId == currentTenantId,
                false
            );

            if (employee == null)
            {
                validationException.Add(new ValidationException(
                    _localizer["EmployeeCouldNotBeFound"] + ".",
                    new Exception() { Source = "EmployeeId" }
                ));
            }
            else
            {
                // Check for overlapping appointments for this employee (must be in same tenant)
                var hasOverlappingAppointments = await _repositoryManager.EmployeeLeaveRepository.HasOverlappingAppointmentsAsync(currentTenantId, employeeId, leaveStart, leaveEnd);

                if (hasOverlappingAppointments)
                {
                    validationException.Add(new ValidationException(
                        _localizer["EmployeeHasOverlappingAppointments"] + ".",
                        new Exception() { Source = "EmployeeId" }
                    ));
                }

                // Check for overlapping leaves for this employee (must be in same tenant)
                var hasOverlappingLeaves = await _repositoryManager.EmployeeLeaveRepository.HasOverlappingLeavesAsync(currentTenantId, employeeId, leaveStart, leaveEnd, employeeLeaveId);

                if (hasOverlappingLeaves)
                {
                    validationException.Add(new ValidationException(
                        _localizer["EmployeeHasOverlappingLeaves"] + ".",
                        new Exception() { Source = "EmployeeId" }
                    ));
                }
            }

            // Date validations
            if (leaveStart > leaveEnd)
            {
                validationException.Add(new ValidationException(
                    _localizer["StartDateCannotBeGreaterThanEndDate"] + ".",
                    new Exception() { Source = "LeaveStartDateTime" }
                ));
            }

            // Optional: Validate that leave is not in the past
            /*
            if (leaveStart < DateTime.UtcNow)
            {
                validationException.Add(new ValidationException(
                    _localizer["StartDateCannotBeLessThanNow"] + ".",
                    new Exception() { Source = "LeaveStartDateTime" }
                ));
            }
            */

            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }
    }
}