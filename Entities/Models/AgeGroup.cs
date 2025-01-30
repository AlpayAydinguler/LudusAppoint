using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class AgeGroup
    {
        public int AgeGroupId { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public bool Status { get; set; } = true;
        public string? Name => $"{MinAge} - {MaxAge}";

        //Navigation Properties
        [ValidateNever]
        public ICollection<OfferedService>? OfferedServices { get; set; }
        public ICollection<CustomerAppointment>? CustomerAppointments { get; set; }
    }
}
