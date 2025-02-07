using Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record EmployeeLeaveDto
    {
        public int EmployeeLeaveId { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public DateTime LeaveStartDateTime { get; init; } = DateTime.Today.AddDays(1) + new TimeSpan(8, 0, 0);

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public DateTime LeaveEndDateTime { get; init; } = DateTime.Today.AddDays(1) + new TimeSpan(18, 0, 0);

        [StringLength(500, ErrorMessageResourceType = typeof(Resources.Dtos.EmployeeLeaveDto),
                    ErrorMessageResourceName = "ReasonMaxLength")]
        public string? Reason { get; init; }

        [ForeignKey("Employee")]
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public int EmployeeId { get; init; }

        [ValidateNever]
        public Employee Employee { get; init; }
    }
}
