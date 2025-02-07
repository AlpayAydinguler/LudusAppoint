using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class EmployeeLeave
    {
        public int EmployeeLeaveId { get; set; }
        public DateTime LeaveStartDateTime { get; set; } = System.DateTime.Today.AddDays(1) + new TimeSpan(8, 0, 0);
        public DateTime LeaveEndDateTime { get; set; } = System.DateTime.Today.AddDays(1) + new TimeSpan(18, 0, 0);
        public string? Reason { get; set; }

        //Foreign Keys
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        // Navigarion Property
        public Employee Employee { get; set; }
    }
}
