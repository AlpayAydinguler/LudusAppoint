using Entities.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class CustomerAppointment
    {
        public int CustomerAppointmentId { get; set; }
        public String Name { get; set; }
        public String Surname { get; set; }
        public Gender Gender { get; set; }
        public TimeSpan ApproximateDuration { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime => StartDateTime.Add(ApproximateDuration);
        public String PhoneNumber { get; set; }
        public String? EMail { get; set; }
        public IdentityUser? CreatedBy { get; set; }
        public CustomerAppointmentStatus Status { get; set; }

        //Foreign Keys
        [ForeignKey("AgeGroup")]
        public int AgeGroupId { get; set; }
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        //Navigation Properties
        [ValidateNever]
        public ICollection<OfferedService> OfferedServices { get; set; } = new List<OfferedService>();
        [ValidateNever]
        public AgeGroup AgeGroup { get; set; }
        [ValidateNever]
        public Branch Branch { get; set; }
        [ValidateNever]
        public Employee Employee { get; set; }
    }
}
