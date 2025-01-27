using Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record EmployeeLeaveDto
    {
        public int EmployeeLeaveId { get; init; }
        public DateTime LeaveStartDateTime { get; init; } = System.DateTime.Today.AddDays(1) + new TimeSpan(8, 0, 0);
        public DateTime LeaveEndDateTime { get; init; } = System.DateTime.Today.AddDays(1) + new TimeSpan(18, 0, 0);
        public string? Reason { get; init; }

        //Foreign Keys
        [ForeignKey("Employee")]
        public int EmployeeId { get; init; }

        // Navigarion Property
        [ValidateNever]
        public Employee Employee { get; init; }
    }
}
