using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class OfferedServiceLocalization
    {
        public int OfferedServiceLocalizationId { get; set; }
        [ForeignKey("Tenant")]
        public Guid TenantId { get; set; }
        public string Language { get; set; } = "en"; // ISO Language Code
        public string OfferedServiceLocalizationName { get; set; }

        // Foreign Key
        public int OfferedServiceId { get; set; }
        // Navigation Property
        public OfferedService OfferedService { get; set; }
        public Tenant Tenant { get; set; }
    }
}
