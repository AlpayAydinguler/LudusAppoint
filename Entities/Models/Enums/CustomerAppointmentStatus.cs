using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Enums
{
    public enum CustomerAppointmentStatus
    {
        [Display(Name = "Awaiting Approval")]
        AwaitingApproval,
        [Display(Name = "Confirmed")]
        Confirmed,
        [Display(Name = "Cancelled")]
        Cancelled,
        [Display(Name = "Completed")]
        Completed,
        [Display(Name = "Customer Confirmed")]
        CustomerConfirmed
    }
}
