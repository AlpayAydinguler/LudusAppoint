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

        public EmployeeLeaveManager(IRepositoryManager repositoryManager, IStringLocalizer<OfferedServiceManager> localizer)
        {
            _repositoryManager = repositoryManager;
            _localizer = localizer;
        }

        public void CreateEmployeeLeave(EmployeeLeave model)
        {
            ValidateEmployeeLeave(model);
            _repositoryManager.EmployeeLeaveRepository.CreateEmployeeLeave(model);
            _repositoryManager.Save();
        }

        public ICollection<EmployeeLeave> GetAllEmployeeLeaves(bool trackChanges)
        {
            var employeeLeaves = _repositoryManager.EmployeeLeaveRepository.GetAllEmployeeLeaves(trackChanges);

            return employeeLeaves;
        }
        private void ValidateEmployeeLeave(EmployeeLeave model)
        {
            var validationException = new List<ValidationException>();
            if (_repositoryManager.CustomerAppointmentRepository.EmployeeHaveAppointment(model))
            {
                var employee = _repositoryManager.EmployeeRepository.FindByCondition(h => h.EmployeeId.Equals(model.EmployeeId), false);
                if (employee == null)
                {
                    validationException.Add(new ValidationException(_localizer["EmployeeCouldNotBeFound."], new Exception() { Source = "EmployeeId" }));
                }
                else
                {
                    validationException.Add(new ValidationException(_localizer["{0} HaveAppointmentsAtTheseDates.", employee.EmployeeFullName], new Exception() { Source = "EmployeeId" }));
                }
            }
            if (model.LeaveStartDateTime > model.LeaveEndDateTime)
            {
                validationException.Add(new ValidationException(_localizer["StartDateCannotBeGreaterThanEndDate."], new Exception() { Source = "StartDateTime" }));
            }
            if (model.LeaveStartDateTime < DateTime.Now)
            {
                validationException.Add(new ValidationException(_localizer["StartDateCannotBeLessThanNow."], new Exception() { Source = "StartDateTime" }));
            }
            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }
    }
}
