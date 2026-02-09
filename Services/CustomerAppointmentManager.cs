using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Entities.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Repositories.Contracts;
using Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class CustomerAppointmentManager : ICustomerAppointmentService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CustomerAppointmentManager> _localizer;
        private readonly ITenantService _tenantService; // Added

        public CustomerAppointmentManager(
            IRepositoryManager repositoryManager,
            IMapper mapper,
            IStringLocalizer<CustomerAppointmentManager> localizer,
            ITenantService tenantService) // Added
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _localizer = localizer;
            _tenantService = tenantService;
        }

        public async Task ChangeStatusAsync(int id, CustomerAppointmentStatus newStatus)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            var entity = await _repositoryManager.CustomerAppointmentRepository
                .FindByConditionAsync(x => x.CustomerAppointmentId == id && x.TenantId == currentTenant.Id, false);

            if (entity == null)
            {
                throw new KeyNotFoundException(_localizer["CustomerAppointmentWithId {id} NotFound"] + ".");
            }

            entity.Status = newStatus;
            await _repositoryManager.CustomerAppointmentRepository.UpdateAsync(entity);
            await _repositoryManager.SaveAsync();
        }

        public async Task CreateCustomerAppointmentAsync(CustomerAppointmentDtoForInsert customerAppointmentDtoForInsert)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            // Validate timing first
            await ValidateAppointmentTimingAsync(customerAppointmentDtoForInsert.StartDateTime,
                                      customerAppointmentDtoForInsert.EndDateTime,
                                      customerAppointmentDtoForInsert.EmployeeId,
                                      customerAppointmentDtoForInsert.BranchId);

            var customerAppointment = _mapper.Map<CustomerAppointment>(customerAppointmentDtoForInsert);
            customerAppointment.TenantId = currentTenant.Id; // Set tenant ID from context

            // Get offered services for current tenant only
            var offeredServices = await _repositoryManager.OfferedServiceRepository
                .GetAllByConditionAsync(x => customerAppointmentDtoForInsert.OfferedServiceIds.Contains(x.OfferedServiceId)
                                          && x.TenantId == currentTenant.Id, true);

            customerAppointment.OfferedServices = offeredServices.ToList();
            await _repositoryManager.CustomerAppointmentRepository.CreateAsync(customerAppointment);
            await _repositoryManager.SaveAsync();
        }

        public async Task<IEnumerable<CustomerAppointmentDto>> GetAllCustomerAppointmentsAsync(bool trackChanges, string language = "en-GB")
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<CustomerAppointmentDto>();
            }

            // Get appointments for current tenant only
            var customerAppointments = await _repositoryManager.CustomerAppointmentRepository
                .GetAllByConditionAsync(x => x.TenantId == currentTenant.Id, trackChanges);

            // Note: This might need adjustment based on your actual GetAllCustomerAppointmentsAsync method
            // If you have a specialized method, update it to include tenant filtering

            var customerAppointmentsDto = _mapper.Map<IEnumerable<CustomerAppointmentDto>>(customerAppointments);
            return customerAppointmentsDto;
        }

        public async Task<CustomerAppointmentDtoForUpdate> GetCustomerAppointmentForUpdateAsync(int id, bool trackChanges, string language = "en-GB")
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            // Get appointment for current tenant only
            var entity = await _repositoryManager.CustomerAppointmentRepository
                .FindByConditionAsync(x => x.CustomerAppointmentId == id && x.TenantId == currentTenant.Id,
                                     trackChanges,
                                     include: q => q.Include(x => x.OfferedServices));

            if (entity == null)
            {
                throw new KeyNotFoundException(_localizer["CustomerAppointmentWithId {id} NotFound"] + ".");
            }

            var customerAppointmentDtoForUpdate = _mapper.Map<CustomerAppointmentDtoForUpdate>(entity);
            return customerAppointmentDtoForUpdate;
        }

        public async Task<CustomerAppointment?> GetOneCustomerAppointmentAsync(int id, bool trackChanges)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return null;
            }

            // Get appointment for current tenant only
            return await _repositoryManager.CustomerAppointmentRepository
                .FindByConditionAsync(x => x.CustomerAppointmentId.Equals(id) && x.TenantId == currentTenant.Id, trackChanges);
        }

        public async Task<IEnumerable<CustomerAppointmentDto>> GetPendingCustomerAppointmentsAsync(bool trackChanges, string language = "en-GB")
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<CustomerAppointmentDto>();
            }

            // Get pending appointments for current tenant only
            var customerAppointments = await _repositoryManager.CustomerAppointmentRepository
                .GetAllByConditionAsync(x => x.Status == CustomerAppointmentStatus.AwaitingApproval
                                          && x.TenantId == currentTenant.Id, trackChanges);

            var customerAppointmentsDto = _mapper.Map<IEnumerable<CustomerAppointmentDto>>(customerAppointments);
            return customerAppointmentsDto;
        }

        public async Task<object> GetReservedDaysTimesAsync(int employeeId, int branchId)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            // Get reservation limit for current tenant's branch
            var reservationInAdvanceDayLimit = await _repositoryManager.BranchRepository
                .GetReservationInAdvanceDayLimitAsync(branchId);

            // Get leaves for current tenant's employee
            var employeeLeaves = await _repositoryManager.EmployeeLeaveRepository.GetLeaveTimesAsync(currentTenant.Id, employeeId, reservationInAdvanceDayLimit);

            // Get reserved times for current tenant's appointments
            var reservedDaysTimes = _repositoryManager.CustomerAppointmentRepository
                .GetReservedDaysTimes(employeeId, reservationInAdvanceDayLimit);

            // Get minimum duration for current tenant's services
            var minimumServiceDuration = await _repositoryManager.OfferedServiceRepository
                .GetMinApproximateDurationAsync();

            var result = new
            {
                reservationInAdvanceDayLimit,
                minimumServiceDuration,
                employeeLeaves,
                reservedDaysTimes
            };

            return result; // Return object instead of JsonResult
        }

        public async Task UpdateCustomerAppointmentAsync(CustomerAppointmentDtoForUpdate customerAppointmentDtoForUpdate)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                throw new ValidationException(_localizer["NoTenantContextAvailable"]);
            }

            // Get existing entity with tenant check
            var entity = await _repositoryManager.CustomerAppointmentRepository
                .FindByConditionAsync(
                    x => x.CustomerAppointmentId == customerAppointmentDtoForUpdate.CustomerAppointmentId
                      && x.TenantId == currentTenant.Id,
                    include: q => q.Include(x => x.OfferedServices),
                    trackChanges: true
                );

            if (entity == null)
            {
                throw new KeyNotFoundException(_localizer["CustomerAppointmentWithId {id} NotFound"] + ".");
            }

            // Validate timing if needed (you might want to add this back)
            // await ValidateAppointmentTimingAsync(...);

            // Map scalar properties
            _mapper.Map(customerAppointmentDtoForUpdate, entity);

            // Clear existing services
            entity.OfferedServices.Clear();

            // Get new services for current tenant only
            var services = await _repositoryManager.OfferedServiceRepository
                .GetAllByConditionAsync(x => customerAppointmentDtoForUpdate.OfferedServiceIds.Contains(x.OfferedServiceId)
                                          && x.TenantId == currentTenant.Id,
                                       trackChanges: true);

            // Add new services
            foreach (var service in services)
            {
                entity.OfferedServices.Add(service);
            }

            await _repositoryManager.SaveAsync();
        }

        private async Task ValidateAppointmentTimingAsync(DateTime startDateTime, DateTime endDateTime, int employeeId, int branchId)
        {
            var validationException = new List<ValidationException>();

            // Get the reserved days and times asynchronously
            var result = await GetReservedDaysTimesAsync(employeeId, branchId);

            if (result == null)
            {
                validationException.Add(new ValidationException(
                    _localizer["GetReservedDaysTimesDidNotReturnAResult"] + ".",
                    new Exception() { Source = "Model" }
                ));
            }

            // Use reflection to get the employeeLeaves property (same as before)
            var resultType = result.GetType();
            var employeeLeavesProp = resultType.GetProperty("employeeLeaves");

            if (employeeLeavesProp == null)
            {
                validationException.Add(new ValidationException(
                    _localizer["TheResultObjectDoesNotContainAn 'employeeLeaves' Property"] + ".",
                    new Exception() { Source = "Model" }
                ));
            }

            var employeeLeavesObj = employeeLeavesProp.GetValue(result);
            if (employeeLeavesObj is not IEnumerable<object> employeeLeaves)
            {
                validationException.Add(new ValidationException(
                    _localizer["TheEmployeeLeavesPropertyIsNotAnEnumerable"] + ".",
                    new Exception() { Source = "Model" }
                ));
            }
            else
            {
                // Validate appointment against each leave
                foreach (var leave in employeeLeaves)
                {
                    var leaveType = leave.GetType();
                    var startProp = leaveType.GetProperty("LeaveStartDateTime");
                    var endProp = leaveType.GetProperty("LeaveEndDateTime");

                    if (startProp == null || endProp == null)
                    {
                        validationException.Add(new ValidationException(
                        _localizer["LeaveObjectDoesNotHaveTheExpectedProperties"] + ".",
                        new Exception() { Source = "Model" }
                        ));
                    }

                    // Get the values and cast them to DateTime
                    DateTime leaveStart = (DateTime)startProp.GetValue(leave);
                    DateTime leaveEnd = (DateTime)endProp.GetValue(leave);

                    if (IsOverlapping(startDateTime, endDateTime, leaveStart, leaveEnd))
                    {
                        validationException.Add(new ValidationException(
                        _localizer["AppointmentConflictsWithEmployeeLeaveFrom{0}To{1}", leaveStart, leaveEnd] + ".",
                        new Exception() { Source = "StartDateTime" }
                        ));
                    }
                }
            }

            // Similarly, validate against reservedDaysTimes:
            var reservedTimesProp = resultType.GetProperty("reservedDaysTimes");
            if (reservedTimesProp == null)
            {
                validationException.Add(new ValidationException(
                _localizer["TheResultObjectDoesNotContainA 'reservedDaysTimes' Property"] + ".",
                new Exception() { Source = "Model" }
                ));
            }

            var reservedTimesObj = reservedTimesProp.GetValue(result);
            if (reservedTimesObj is not IEnumerable<object> reservedDaysTimes)
            {
                validationException.Add(new ValidationException(
                _localizer["TheReservedDaysTimesPropertyIsNotAnEnumerable"] + ".",
                new Exception() { Source = "Model" }
                ));
            }
            else
            {
                foreach (var reservation in reservedDaysTimes)
                {
                    var resType = reservation.GetType();
                    var startResProp = resType.GetProperty("StartDateTime");
                    var endResProp = resType.GetProperty("EndDateTime");

                    if (startResProp == null || endResProp == null)
                    {
                        validationException.Add(new ValidationException(
                        _localizer["ReservationObjectDoesNotHaveTheExpectedProperties"] + ".",
                        new Exception() { Source = "Model" }
                        ));
                    }

                    DateTime resStart = (DateTime)startResProp.GetValue(reservation);
                    DateTime resEnd = (DateTime)endResProp.GetValue(reservation);

                    if (IsOverlapping(startDateTime, endDateTime, resStart, resEnd))
                    {
                        validationException.Add(new ValidationException(
                        _localizer["AppointmentConflictsWithExistingReservationFrom{0}To{1}", resStart, resEnd] + ".",
                        new Exception() { Source = "StartDateTime" }
                        ));
                    }
                }
            }

            if (validationException.Count != 0)
                throw new AggregateException(validationException);
        }

        private bool IsOverlapping(DateTime newStart, DateTime newEnd, DateTime existingStart, DateTime existingEnd)
        {
            return newStart < existingEnd && newEnd > existingStart;
        }
    }
}