using Entities.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record OfferedServiceLocalizationDto
    {
        public int OfferedServiceLocalizationId { get; init; }
        [ForeignKey("Tenant")]
        public Guid TenantId { get; init; }
        public string Language { get; init; } = "en"; // ISO Language Code
        public string OfferedServiceLocalizationName { get; init; }

        // Foreign Key
        public int OfferedServiceId { get; init; }
        // Navigation Property
        public OfferedService OfferedService { get; init; }
        public TenantDto? Tenant { get; init; }
    }
}
