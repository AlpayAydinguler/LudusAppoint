using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Entities.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }
        public String? IdentityUserId { get; set; }
        public int BranchId { get; set; }
        public String EmployeeName { get; set; }
        public String EmployeeSurname { get; set; }
        public String EmployeeFullName => $"{EmployeeName} {EmployeeSurname}";
        public Boolean CanTakeClients { get; set; }
        public DayOfWeek DayOff { get; set; }
        public TimeSpan StartOfWorkingHours { get; set; }
        public TimeSpan EndOfWorkingHours { get; set; }

        //Foreign Keys
        public ICollection<OfferedService> OfferedServices { get; set; } = new List<OfferedService>();
        [ValidateNever]
        public Branch Branch { get; set; }

        //Navigation Properties
        public ICollection<CustomerAppointment>? CustomerAppointment { get; set; }
        public ICollection<EmployeeLeave>? EmployeesLeaves { get; set; }
    }
}
