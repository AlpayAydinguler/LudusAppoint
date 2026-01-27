using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class AgeGroup
    {
        public int AgeGroupId { get; set; }
        [ForeignKey("Tenant")]
        public Guid TenantId { get; set; }
        public int MinAge { get; set; }
        public int MaxAge { get; set; }
        public bool Status { get; set; } = true;
        public string? Name => $"{MinAge} - {MaxAge}";

        //Navigation Properties
        [ValidateNever]
        public ICollection<OfferedService>? OfferedServices { get; set; }
        public ICollection<CustomerAppointment>? CustomerAppointments { get; set; }
        public Tenant Tenant { get; set; }
    }
}
