using Entities.Models;
using Entities.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record CustomerAppointmentDto
    {
        public int CustomerAppointmentId { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public String Name { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public String Surname { get; init; }
        public Gender Gender { get; init; }
        [ForeignKey("AgeGroup")]
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public int AgeGroupId { get; init; }
        [ForeignKey("Employee")]
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public int EmployeeId { get; init; }
        public TimeSpan ApproximateDuration { get; init; }
        [DataType(DataType.Currency)]
        public decimal Price { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public DateTime StartDateTime { get; init; }
        public DateTime EndDateTime => StartDateTime.Add(ApproximateDuration);
        [Phone]
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public String PhoneNumber { get; init; }
        [EmailAddress]
        public String? EMail { get; init; }
        public IdentityUser? CreatedBy { get; init; }
        public CustomerAppointmentStatus Status { get; init; }
        [ForeignKey("Branch")]
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public int BranchId { get; init; }

        //Foreign Keys
        public ICollection<OfferedService> OfferedServices { get; init; } = new List<OfferedService>();
        [ValidateNever]
        public Employee Employee { get; init; }
        [ValidateNever]
        public AgeGroup AgeGroup { get; init; }
        [ValidateNever]
        public Branch Branch { get; init; }
    }
}
