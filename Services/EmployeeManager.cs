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

        public EmployeeManager(IRepositoryManager repositoryManager, IMapper mapper, IStringLocalizer<EmployeeManager> localizer)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _localizer = localizer;
        }

        public void CreateEmployee(EmployeeDtoForInsert employeeDtoForInsert)
        {
            var employee = _mapper.Map<Employee>(employeeDtoForInsert);
            var existingOfferedServices = _repositoryManager.OfferedServiceRepository.GetAllByCondition(h => employeeDtoForInsert.OfferedServiceIds.Contains(h.OfferedServiceId), true)
                                                                                     .ToList();
            employee.OfferedServices = existingOfferedServices;
            _repositoryManager.EmployeeRepository.CreateEmployee(employee);
            _repositoryManager.Save();
        }

        public IEnumerable<EmployeeDto> GetAllEmployees(bool trackChanges)
        {
            var employees = _repositoryManager.EmployeeRepository.GetAllEmployees(trackChanges);
            var employeeDtos = _mapper.Map<IEnumerable<EmployeeDto>>(employees);
            return employeeDtos;
        }

        public IEnumerable<Employee> GetEmployeesForCustomerAppointment(int branchId, List<int> offeredServiceIds, bool trackChanges)
        {
            return _repositoryManager.EmployeeRepository.GetEmployeesForForCustomerAppointment(branchId, offeredServiceIds, trackChanges);
        }

        private Employee GetOneEmployee(int id, bool trackChanges, string language = "en-GB")
        {
            return _repositoryManager.EmployeeRepository.GetEmployee(id, trackChanges, language);
        }
        private Employee GetOneEmployee(int id)
        {
            return _repositoryManager.EmployeeRepository.FindByCondition(h => h.EmployeeId.Equals(id), false);
        }

        public EmployeeDtoForUpdate GetOneEmployeeForUpdate(int id, bool trackChanges, string language)
        {
            var employee = GetOneEmployee(id, trackChanges, language);
            var employeeDtoForUpdate = _mapper.Map<EmployeeDtoForUpdate>(employee);
            return employeeDtoForUpdate;
        }

        public void UpdateEmployee(EmployeeDtoForUpdate employeeDtoForUpdate)
        {
            var model = _repositoryManager.EmployeeRepository.GetAllByCondition(x => x.EmployeeId.Equals(employeeDtoForUpdate.EmployeeId), true)
                                                             .Include(h => h.OfferedServices)
                                                             .FirstOrDefault();
            _mapper.Map(employeeDtoForUpdate, model);
            model.OfferedServices.Clear();
            foreach (var offeredServiceId in employeeDtoForUpdate.OfferedServiceIds)
            {
                var offeredService = _repositoryManager.OfferedServiceRepository.FindByCondition(h => h.OfferedServiceId.Equals(offeredServiceId), true);
                model.OfferedServices.Add(offeredService);
            }
            _repositoryManager.Save();
        }

        public void DeleteEmployee(int id)
        {
            var employee = GetOneEmployee(id);
            try
            {
                _repositoryManager.EmployeeRepository.Delete(employee);
                _repositoryManager.Save();
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
