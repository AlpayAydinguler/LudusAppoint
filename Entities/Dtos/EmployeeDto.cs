using Entities.Models;
using Entities.Models.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;

namespace Entities.Dtos
{
    public record EmployeeDto
    {
        public int EmployeeId { get; init; }

        [ValidateNever]
        public String? IdentityUserId { get; init; }

        [ForeignKey("Branch")]
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public int BranchId { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [StringLength(50, MinimumLength = 2,
                     ErrorMessageResourceType = typeof(Resources.Dtos.EmployeeDto),
                     ErrorMessageResourceName = "NameLengthValidation")]
        public String EmployeeName { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [StringLength(50, MinimumLength = 2,
                     ErrorMessageResourceType = typeof(Resources.Dtos.EmployeeDto),
                     ErrorMessageResourceName = "SurnameLengthValidation")]
        public String EmployeeSurname { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public String EmployeeFullName => $"{EmployeeName} {EmployeeSurname}";

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public Boolean CanTakeClients { get; init; }

        [EnumDataType(typeof(DayOfWeek),
                     ErrorMessageResourceType = typeof(Resources.Dtos.EmployeeDto),
                     ErrorMessageResourceName = "InvalidDayOfWeek")]
        public DayOfWeek DayOff { get; init; }
        public String DayOffName => CultureInfo.CurrentCulture.DateTimeFormat.GetDayName(DayOff);

        [Range(typeof(TimeSpan), "00:00:00", "23:59:59",
              ErrorMessageResourceType = typeof(Resources.Dtos.EmployeeDto),
              ErrorMessageResourceName = "InvalidStartTime")]
        public TimeSpan StartOfWorkingHours { get; init; }

        [Range(typeof(TimeSpan), "00:00:00", "23:59:59",
              ErrorMessageResourceType = typeof(Resources.Dtos.EmployeeDto),
              ErrorMessageResourceName = "InvalidEndTime")]
        public TimeSpan EndOfWorkingHours { get; init; }
        public bool Status { get; set; } = false;

        //Foreign Keys
        public ICollection<OfferedService> OfferedServices { get; init; } = new List<OfferedService>();
        [ValidateNever]
        public Branch Branch { get; init; }

        //Navigation Properties
        [ValidateNever]
        public ICollection<CustomerAppointment>? CustomerAppointment { get; init; }

        [ValidateNever]
        public ICollection<EmployeeLeave>? EmployeesLeaves { get; init; }
    }
}