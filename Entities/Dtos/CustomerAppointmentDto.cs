using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Entities.Models;
using Entities.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Entities.Dtos
{
    public record CustomerAppointmentDto
    {
        public int CustomerAppointmentId { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [StringLength(50, MinimumLength = 2,
                     ErrorMessageResourceType = typeof(Resources.Dtos.CustomerAppointmentDto),
                     ErrorMessageResourceName = "NameValidation")]
        public string Name { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [StringLength(50, MinimumLength = 2,
                     ErrorMessageResourceType = typeof(Resources.Dtos.CustomerAppointmentDto),
                     ErrorMessageResourceName = "SurnameValidation")]
        public string Surname { get; init; }

        [Range(0, int.MaxValue,
              ErrorMessageResourceType = typeof(Resources.Dtos.CustomerAppointmentDto),
              ErrorMessageResourceName = "InvalidGender")]
        public Gender? Gender { get; init; }

        [Range(typeof(TimeSpan), "00:01:00", "23:59:59",
              ErrorMessageResourceType = typeof(Resources.Dtos.CustomerAppointmentDto),
              ErrorMessageResourceName = "DurationValidation")]
        public TimeSpan ApproximateDuration { get; init; }

        [DataType(DataType.Currency)]
        [Range(1.00, int.MaxValue,
              ErrorMessageResourceType = typeof(Resources.Dtos.CustomerAppointmentDto),
              ErrorMessageResourceName = "PriceValidation")]
        public decimal Price { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public DateTime StartDateTime { get; init; }

        public DateTime EndDateTime => StartDateTime.Add(ApproximateDuration);

        [Phone]
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [RegularExpression(@"^\d{10,15}$",
                  ErrorMessageResourceType = typeof(Resources.Dtos.CustomerAppointmentDto),
                  ErrorMessageResourceName = "PhoneFormat")]
        public string PhoneNumber { get; init; }

        [EmailAddress(ErrorMessageResourceType = typeof(Resources.Dtos.CustomerAppointmentDto),
                    ErrorMessageResourceName = "EmailFormat")]
        public string? EMail { get; init; }

        public IdentityUser? CreatedBy { get; init; }

        [Range(0, int.MaxValue,
              ErrorMessageResourceType = typeof(Resources.Dtos.CustomerAppointmentDto),
              ErrorMessageResourceName = "InvalidStatus")]
        public CustomerAppointmentStatus Status { get; init; } = CustomerAppointmentStatus.CustomerConfirmed;

        // Foreign Keys
        [ForeignKey("AgeGroup")]
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [Range(1, int.MaxValue,
              ErrorMessageResourceType = typeof(Resources.Dtos.CustomerAppointmentDto),
              ErrorMessageResourceName = "InvalidAgeGroup")]
        public int AgeGroupId { get; init; }

        [ForeignKey("Employee")]
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [Range(1, int.MaxValue,
              ErrorMessageResourceType = typeof(Resources.Dtos.CustomerAppointmentDto),
              ErrorMessageResourceName = "InvalidEmployee")]
        public int EmployeeId { get; init; }
        [ForeignKey("Branch")]
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [Range(1, int.MaxValue,
              ErrorMessageResourceType = typeof(Resources.Dtos.CustomerAppointmentDto),
              ErrorMessageResourceName = "InvalidBranch")]
        public int BranchId { get; init; }
        [ValidateNever]
        public ICollection<OfferedService> OfferedServices { get; init; } = new List<OfferedService>();

        [ValidateNever]
        public Employee Employee { get; init; }

        [ValidateNever]
        public AgeGroup AgeGroup { get; init; }

        [ValidateNever]
        public Branch Branch { get; init; }
    }
}