using Entities.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Entities.Models
{
    public class CustomerAppointment
    {
        public int CustomerAppointmentId { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public Gender Gender { get; set; }
        public int EmployeeId { get; set; }
        public TimeSpan ApproximateDuration { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime => StartDateTime.Add(ApproximateDuration);
        public String PhoneNumber { get; set; }
        public String? EMail { get; set; }
        public IdentityUser? CreatedBy { get; set; }
        public CustomerAppointmentStatus Status { get; set; }
        public int BranchId { get; set; }

        //Foreign Keys
        public int AgeGroupId { get; set; }
        public ICollection<OfferedService> OfferedServices { get; set; } = new List<OfferedService>();
        [ValidateNever]
        public Employee Employee { get; set; }
        [ValidateNever]
        public Branch Branch { get; set; }

        //Navigation Properties
        [ValidateNever]
        public AgeGroup AgeGroup { get; set; }
    }
}
