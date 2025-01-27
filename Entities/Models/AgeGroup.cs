using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Entities.Models
{
    public class AgeGroup
    {
        public int AgeGroupId { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public bool Status { get; set; } = true;
        //Navigation Properties
        [ValidateNever]
        public ICollection<OfferedService>? OfferedServices { get; set; }
        public ICollection<CustomerAppointment>? CustomerAppointments { get; set; }
    }
}
