using Entities.Models;
using Entities.Models.Enums;
using Microsoft.EntityFrameworkCore;
using Repositories.Contracts;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Repositories
{
    public class CustomerAppointmentRepository : RepositoryBase<CustomerAppointment>, ICustomerAppointmentRepository
    {
        public CustomerAppointmentRepository(RepositoryContext repositoryContext) : base(repositoryContext)
        {
        }

        public async Task<List<CustomerAppointment>> GetAllCustomerAppointmentsAsync(bool trackChanges, string language)
        {
            var customerAppointments = await _repositoryContext.CustomerAppointments.Include(ca => ca.Employee)
                                                                              .Include(ca => ca.AgeGroup)
                                                                              .Include(ca => ca.OfferedServices)
                                                                              .ThenInclude(hs => hs.OfferedServiceLocalizations)
                                                                              .Select(ca => new CustomerAppointment
                                                                              {
                                                                                  CustomerAppointmentId = ca.CustomerAppointmentId,
                                                                                  Name = ca.Name,
                                                                                  Surname = ca.Surname,
                                                                                  Gender = ca.Gender,
                                                                                  AgeGroupId = ca.AgeGroupId,
                                                                                  AgeGroup = ca.AgeGroup,
                                                                                  EmployeeId = ca.EmployeeId,
                                                                                  Employee = ca.Employee,
                                                                                  ApproximateDuration = ca.ApproximateDuration,
                                                                                  Price = ca.Price,
                                                                                  StartDateTime = ca.StartDateTime,
                                                                                  PhoneNumber = ca.PhoneNumber,
                                                                                  EMail = ca.EMail,
                                                                                  Status = ca.Status,
                                                                                  OfferedServices = ca.OfferedServices.Select(hs => new OfferedService
                                                                                  {
                                                                                      OfferedServiceId = hs.OfferedServiceId,
                                                                                      OfferedServiceName = hs.OfferedServiceLocalizations.FirstOrDefault(l => l.Language == language).OfferedServiceLocalizationName ?? hs.OfferedServiceName
                                                                                  }).ToList()
                                                                              })
                                                                              .ToListAsync();
            return customerAppointments;
        }

        public async Task<bool> EmployeeHaveAppointmentAsync(EmployeeLeave employeeLeave)
        {
            var employeesAppointments = await _repositoryContext.CustomerAppointments.Where(ca => ca.EmployeeId == employeeLeave.EmployeeId &&
                                                                                         (ca.Status == CustomerAppointmentStatus.CustomerConfirmed ||
                                                                                          ca.Status == CustomerAppointmentStatus.Confirmed))
                                                                                  .AsNoTracking()
                                                                                  .ToListAsync();
            
            return employeesAppointments.AsEnumerable().Any(ca =>
            {
                var appointmentEndTime = ca.StartDateTime.Add(ca.ApproximateDuration);
                return (ca.StartDateTime < employeeLeave.LeaveEndDateTime && appointmentEndTime > employeeLeave.LeaveStartDateTime);
            });
        }

        public async Task<IEnumerable<CustomerAppointment>> GetPendingCustomerAppointmentsAsync(bool trackChanges, string language)
        {
            var customerAppointments = await _repositoryContext.CustomerAppointments.Include(ca => ca.Employee)
                                                                              .Include(ca => ca.AgeGroup)
                                                                              .Include(ca => ca.OfferedServices)
                                                                              .ThenInclude(hs => hs.OfferedServiceLocalizations)
                                                                              .Where(ca => ca.Status == CustomerAppointmentStatus.AwaitingApproval)
                                                                              .Select(ca => new CustomerAppointment
                                                                              {
                                                                                  CustomerAppointmentId = ca.CustomerAppointmentId,
                                                                                  Name = ca.Name,
                                                                                  Surname = ca.Surname,
                                                                                  Gender = ca.Gender,
                                                                                  AgeGroupId = ca.AgeGroupId,
                                                                                  AgeGroup = ca.AgeGroup,
                                                                                  EmployeeId = ca.EmployeeId,
                                                                                  Employee = ca.Employee,
                                                                                  ApproximateDuration = ca.ApproximateDuration,
                                                                                  Price = ca.Price,
                                                                                  StartDateTime = ca.StartDateTime,
                                                                                  PhoneNumber = ca.PhoneNumber,
                                                                                  EMail = ca.EMail,
                                                                                  Status = ca.Status,
                                                                                  OfferedServices = ca.OfferedServices.Select(hs => new OfferedService
                                                                                  {
                                                                                      OfferedServiceId = hs.OfferedServiceId,
                                                                                      OfferedServiceName = hs.OfferedServiceLocalizations.FirstOrDefault(l => l.Language == language).OfferedServiceLocalizationName ?? hs.OfferedServiceName
                                                                                  }).ToList()
                                                                              })
                                                                              .ToListAsync();
            return customerAppointments;
        }

        public object GetReservedDaysTimes(int employeeId, int reservationInAdvanceDayLimit)
        {
            var maxDate = DateTime.Today.AddDays(reservationInAdvanceDayLimit + 1);

            var reservedDaysTimes = _repositoryContext.CustomerAppointments
                .Where(ca => ca.EmployeeId == employeeId &&
                             ca.StartDateTime.Date >= DateTime.Today &&
                             ca.StartDateTime.Date < maxDate)
                .Select(ca => new
                {
                    ca.StartDateTime,
                    ca.EndDateTime
                })
                .ToList();
            return reservedDaysTimes;
        }

        public async Task<CustomerAppointment> GetCustomerAppointmentForUpdateAsync(int id, bool trackChanges, string language)
        {
            var customerAppointment = await _repositoryContext.CustomerAppointments.Include(ca => ca.Employee)
                                                                             .Include(ca => ca.AgeGroup)
                                                                             .Include(ca => ca.OfferedServices)
                                                                             .ThenInclude(hs => hs.OfferedServiceLocalizations.Where(loc => loc.Language == language))
                                                                             .Where(ca => ca.CustomerAppointmentId == id)
                                                                             .FirstOrDefaultAsync();
            if (customerAppointment != null)
            {
                foreach (var offeredService in customerAppointment.OfferedServices ?? Enumerable.Empty<OfferedService>())
                {
                    var localization = offeredService.OfferedServiceLocalizations.FirstOrDefault();
                    offeredService.OfferedServiceName = localization?.OfferedServiceLocalizationName ?? offeredService.OfferedServiceName;
                }
            }
            return customerAppointment;
        }
    }
}
