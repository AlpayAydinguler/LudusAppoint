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
        private readonly IStringLocalizer<OfferedServiceManager> _localizer;
        private readonly IMapper _mapper;

        public EmployeeLeaveManager(IRepositoryManager repositoryManager, IStringLocalizer<OfferedServiceManager> localizer, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _localizer = localizer;
            _mapper = mapper;
        }

        public void CreateEmployeeLeave(EmployeeLeaveDtoForInsert employeeLeaveDtoForInsert)
        {
            ValidateEmployeeLeave(employeeLeaveDtoForInsert, null);
            var employeeLeave = _mapper.Map<EmployeeLeave>(employeeLeaveDtoForInsert);
            _repositoryManager.EmployeeLeaveRepository.CreateEmployeeLeave(employeeLeave);
            _repositoryManager.Save();
        }

        public void DeleteEmployeeLeave(int id)
        {
            var employeeLeave = _repositoryManager.EmployeeLeaveRepository.FindByCondition(
                    h => h.EmployeeLeaveId == id,
                    false
                );
            _repositoryManager.EmployeeLeaveRepository.Delete(employeeLeave);
            _repositoryManager.Save();
        }

        public ICollection<EmployeeLeaveDto> GetAllEmployeeLeaves(bool trackChanges)
        {
            var employeeLeaves = _repositoryManager.EmployeeLeaveRepository.GetAllEmployeeLeaves(trackChanges);
            var employeeLeavesDto = _mapper.Map<ICollection<EmployeeLeaveDto>>(employeeLeaves);
            return employeeLeavesDto;
        }

        public EmployeeLeaveDtoForUpdate GetEmployeeLeaveForUpdate(int id)
        {
            var employeeLeave = _repositoryManager.EmployeeLeaveRepository.FindByCondition(
                    h => h.EmployeeLeaveId == id,
                    false
                );
            var employeeLeaveDto = _mapper.Map<EmployeeLeaveDtoForUpdate>(employeeLeave);
            return employeeLeaveDto;
        }

        public void UpdateEmployeeLeave(EmployeeLeaveDtoForUpdate employeeLeaveDtoForUpdate)
        {
            ValidateEmployeeLeave(null, employeeLeaveDtoForUpdate);
            var employeeLeave = _mapper.Map<EmployeeLeave>(employeeLeaveDtoForUpdate);
            _repositoryManager.EmployeeLeaveRepository.Update(employeeLeave);
            _repositoryManager.Save();
        }

        private void ValidateEmployeeLeave(EmployeeLeaveDtoForInsert? employeeLeaveDtoForInsert,
                                  EmployeeLeaveDtoForUpdate? employeeLeaveDtoForUpdate)
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

            // Check for existing appointments
            var employee = _repositoryManager.EmployeeRepository.FindByCondition(
                    h => h.EmployeeId == employeeId,
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
                var hasOverlappingAppointments = _repositoryManager.EmployeeLeaveRepository.HasOverlappingAppointments(employeeId, leaveStart, leaveEnd);
                if (hasOverlappingAppointments == true)
                {
                    validationException.Add(new ValidationException(
                        _localizer["EmployeeHasOverlappingAppointments"] + ".",
                        new Exception() { Source = "EmployeeId" }
                    ));
                }
                var hasOverlappingLeaves = _repositoryManager.EmployeeLeaveRepository.HasOverlappingLeaves(employeeId, leaveStart, leaveEnd, employeeLeaveId);
                if (hasOverlappingLeaves == true)
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

            /*
            if (leaveStart < DateTime.Now)
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
