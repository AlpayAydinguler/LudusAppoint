using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
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

        public CustomerAppointmentManager(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
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
            var offeredServices = await _repositoryManager.OfferedServiceRepository.GetAllByConditionAsync(x => customerAppointmentDtoForInsert.OfferedServiceIds.Contains(x.OfferedServiceId),true);
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

        public async Task<CustomerAppointmentDtoForUpdate> GetCustomerAppointmentUpdateAsync(int id, bool trackChanges)
        {
            var entity = await _repositoryManager.CustomerAppointmentRepository.FindByConditionAsync(x => x.CustomerAppointmentId.Equals(id), trackChanges);
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
        public void ValidateAppointmentTiming(DateTime startDateTime, DateTime endDateTime, int employeeId, int branchId)
        {
            // Get the JsonResult from your method (assuming your GetReservedDaysTimes returns a JsonResult)
            var jsonResult = GetReservedDaysTimes(employeeId, branchId) as JsonResult;
            if (jsonResult == null)
            {
                throw new Exception("GetReservedDaysTimes did not return a JsonResult.");
            }

            var commitments = jsonResult.Value;
            if (commitments == null)
            {
                throw new Exception("The JsonResult's Value is null.");
            }

            // Use reflection to get the employeeLeaves property
            var commitmentsType = commitments.GetType();
            var employeeLeavesProp = commitmentsType.GetProperty("employeeLeaves");
            if (employeeLeavesProp == null)
            {
                throw new Exception("The commitments object does not contain an 'employeeLeaves' property.");
            }
            var employeeLeavesObj = employeeLeavesProp.GetValue(commitments);
            if (employeeLeavesObj is not IEnumerable<object> employeeLeaves)
            {
                throw new Exception("The employeeLeaves property is not an enumerable.");
            }

            // Validate appointment against each leave using reflection to get property values
            foreach (var leave in employeeLeaves)
            {
                var leaveType = leave.GetType();
                var startProp = leaveType.GetProperty("LeaveStartDateTime");
                var endProp = leaveType.GetProperty("LeaveEndDateTime");

                if (startProp == null || endProp == null)
                {
                    throw new Exception("Leave object does not have the expected properties.");
                }

                // Get the values and cast them to DateTime
                DateTime leaveStart = (DateTime)startProp.GetValue(leave);
                DateTime leaveEnd = (DateTime)endProp.GetValue(leave);

                if (IsOverlapping(startDateTime, endDateTime, leaveStart, leaveEnd))
                {
                    throw new ValidationException($"Appointment conflicts with employee leave from {leaveStart} to {leaveEnd}");
                }
            }

            // Similarly, validate against reservedDaysTimes:
            var reservedTimesProp = commitmentsType.GetProperty("reservedDaysTimes");
            if (reservedTimesProp == null)
            {
                throw new Exception("The commitments object does not contain a 'reservedDaysTimes' property.");
            }
            var reservedTimesObj = reservedTimesProp.GetValue(commitments);
            if (reservedTimesObj is not IEnumerable<object> reservedDaysTimes)
            {
                throw new Exception("The reservedDaysTimes property is not an enumerable.");
            }

            foreach (var reservation in reservedDaysTimes)
            {
                var resType = reservation.GetType();
                var startResProp = resType.GetProperty("StartDateTime");
                var endResProp = resType.GetProperty("EndDateTime");

                if (startResProp == null || endResProp == null)
                {
                    throw new Exception("Reservation object does not have the expected properties.");
                }

                DateTime resStart = (DateTime)startResProp.GetValue(reservation);
                DateTime resEnd = (DateTime)endResProp.GetValue(reservation);

                if (IsOverlapping(startDateTime, endDateTime, resStart, resEnd))
                {
                    throw new ValidationException($"Appointment conflicts with existing reservation from {resStart} to {resEnd}");
                }
            }
        }

        private bool IsOverlapping(DateTime newStart, DateTime newEnd, DateTime existingStart, DateTime existingEnd)
        {
            return newStart < existingEnd && newEnd > existingStart;
        }
    }
}
