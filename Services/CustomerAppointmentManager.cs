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

        public CustomerAppointmentManager(IRepositoryManager repositoryManager, IMapper mapper, IStringLocalizer<CustomerAppointmentManager> localizer)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task ChangeStatusAsync(int id, CustomerAppointmentStatus newStatus)
        {
            var entity = await _repositoryManager.CustomerAppointmentRepository.FindByConditionAsync(x => x.CustomerAppointmentId == id, false);
            if (entity == null)
            {
                throw new KeyNotFoundException(_localizer["CustomerAppointmentWithId {id} NotFound"] + ".");
            }
            entity.Status = newStatus;
            _repositoryManager.CustomerAppointmentRepository.Update(entity);
            await _repositoryManager.CustomerAppointmentRepository.SaveAsync();
        }

        public void CreateAppointment(CustomerAppointment customerAppointment, int[] offeredServiceIds)
        {
            foreach (var offeredServiceId in offeredServiceIds)
            {
                var offeredService = _repositoryManager.OfferedServiceRepository.GetofferedService(offeredServiceId, false);
                _repositoryManager.OfferedServiceRepository.AttachAsUnchanged(offeredService);
                customerAppointment.OfferedServices.Add(offeredService);
            }
            _repositoryManager.CustomerAppointmentRepository.Create(customerAppointment);
            _repositoryManager.Save();
        }

        public async Task CreateCustomerAppointmentAsync(CustomerAppointmentDtoForInsert customerAppointmentDtoForInsert)
        {
            // Validate timing first
            ValidateAppointmentTiming(customerAppointmentDtoForInsert.StartDateTime,
                                      customerAppointmentDtoForInsert.EndDateTime,
                                      customerAppointmentDtoForInsert.EmployeeId,
                                      customerAppointmentDtoForInsert.BranchId);
            var customerAppointment = _mapper.Map<CustomerAppointment>(customerAppointmentDtoForInsert);
            var offeredServices = await _repositoryManager.OfferedServiceRepository.GetAllByConditionAsync(x => customerAppointmentDtoForInsert.OfferedServiceIds.Contains(x.OfferedServiceId), true);
            customerAppointment.OfferedServices = offeredServices.ToList();
            _repositoryManager.CustomerAppointmentRepository.Create(customerAppointment);
            await _repositoryManager.CustomerAppointmentRepository.SaveAsync();
        }

        public IEnumerable<CustomerAppointmentDto> GetAllCustomerAppointments(bool trackChanges, string language = "en-GB")
        {
            var customerAppointments = _repositoryManager.CustomerAppointmentRepository.GetAllCustomerAppointments(trackChanges, language);
            var customerAppointmentsDto = _mapper.Map<IEnumerable<CustomerAppointmentDto>>(customerAppointments);
            return customerAppointmentsDto;
        }

        public async Task<CustomerAppointmentDtoForUpdate> GetCustomerAppointmentForUpdateAsync(int id, bool trackChanges, string language = "en-GB")
        {
            var entity = await _repositoryManager.CustomerAppointmentRepository.GetCustomerAppointmentForUpdateAsync(id, trackChanges, language);
            var customerAppointmentDtoForUpdate = _mapper.Map<CustomerAppointmentDtoForUpdate>(entity);
            return customerAppointmentDtoForUpdate;
        }

        public CustomerAppointment? GetOneCustomerAppointment(int id, bool trackChanges)
        {
            return _repositoryManager.CustomerAppointmentRepository.FindByCondition(x => x.CustomerAppointmentId.Equals(id), trackChanges);
        }

        public IEnumerable<CustomerAppointmentDto> GetPendingCustomerAppointments(bool trackChanges, string language = "en-GB")
        {
            var customerAppointments = _repositoryManager.CustomerAppointmentRepository.GetPendingCustomerAppointments(trackChanges, language);
            var customerAppointmentsDto = _mapper.Map<IEnumerable<CustomerAppointmentDto>>(customerAppointments);
            return customerAppointmentsDto;
        }

        public JsonResult GetReservedDaysTimes(int employeeId, int branchId)
        {
            var reservationInAdvanceDayLimit = _repositoryManager.BranchRepository.GetReservationInAdvanceDayLimit(branchId);
            var employeeLeaves = _repositoryManager.EmployeeLeaveRepository.GetLeaveTimes(employeeId, reservationInAdvanceDayLimit);
            var reservedDaysTimes = _repositoryManager.CustomerAppointmentRepository.GetReservedDaysTimes(employeeId, reservationInAdvanceDayLimit);
            var minimumServiceDuration = _repositoryManager.OfferedServiceRepository.GetMinApproximateDuration();
            var result = new
            {
                reservationInAdvanceDayLimit,
                minimumServiceDuration,
                employeeLeaves,
                reservedDaysTimes
            };

            return new JsonResult(result);
        }

        public async Task UpdateCustomerAppointmentAsync(CustomerAppointmentDtoForUpdate customerAppointmentDtoForUpdate)
        {
            // Get existing entity with its offered services
            var entity = await _repositoryManager.CustomerAppointmentRepository
                .FindByConditionAsync(
                    x => x.CustomerAppointmentId == customerAppointmentDtoForUpdate.CustomerAppointmentId,
                    include: q => q.Include(x => x.OfferedServices),
                    trackChanges: true
                );

            // Map scalar properties
            _mapper.Map(customerAppointmentDtoForUpdate, entity);

            // Clear existing services
            entity.OfferedServices.Clear();

            // Get new services
            var services = await _repositoryManager.OfferedServiceRepository.GetAllByConditionAsync(x => customerAppointmentDtoForUpdate.OfferedServiceIds.Contains(x.OfferedServiceId),
                                                                                                    trackChanges: true
                                                                                                    );
                

            // Add new services
            foreach (var service in services)
            {
                entity.OfferedServices.Add(service);
            }

            await _repositoryManager.CustomerAppointmentRepository.SaveAsync();
        }

        public void ValidateAppointmentTiming(DateTime startDateTime, DateTime endDateTime, int employeeId, int branchId)
        {
            var validationException = new List<ValidationException>();
            // Get the JsonResult from your method (assuming your GetReservedDaysTimes returns a JsonResult)
            var jsonResult = GetReservedDaysTimes(employeeId, branchId) as JsonResult;
            if (jsonResult == null)
            {
                validationException.Add(new ValidationException(
                    _localizer["GetReservedDaysTimesDidNotReturnAJsonResult"] + ".",
                    new Exception() { Source = "Model" }
                ));
            }

            var commitments = jsonResult.Value;
            if (commitments == null)
            {
                validationException.Add(new ValidationException(
                    _localizer["TheJsonResultsValueIsNull"] + ".",
                    new Exception() { Source = "Model" }
                ));
            }

            // Use reflection to get the employeeLeaves property
            var commitmentsType = commitments.GetType();
            var employeeLeavesProp = commitmentsType.GetProperty("employeeLeaves");
            if (employeeLeavesProp == null)
            {
                validationException.Add(new ValidationException(
                    _localizer["TheCommitmentsObjectDoesNotContainAn 'employeeLeaves' Property"] + ".",
                    new Exception() { Source = "Model" }
                ));
            }
            var employeeLeavesObj = employeeLeavesProp.GetValue(commitments);
            if (employeeLeavesObj is not IEnumerable<object> employeeLeaves)
            {
                validationException.Add(new ValidationException(
                    _localizer["TheEmployeeLeavesPropertyIsNotAnEnumerable"] + ".",
                    new Exception() { Source = "Model" }
                ));
            }
            else
            {
                // Validate appointment against each leave using reflection to get property values
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
            var reservedTimesProp = commitmentsType.GetProperty("reservedDaysTimes");
            if (reservedTimesProp == null)
            {
                validationException.Add(new ValidationException(
                _localizer["TheCommitmentsObjectDoesNotContainA 'reservedDaysTimes' Property"] + ".",
                new Exception() { Source = "Model" }
                ));
            }
            var reservedTimesObj = reservedTimesProp.GetValue(commitments);
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
