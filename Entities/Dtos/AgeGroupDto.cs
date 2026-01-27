using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record AgeGroupDto
    {
        public int AgeGroupId { get; init; }
        [ForeignKey("Tenant")]
        public Guid TenantId { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [Range(0, 125, ErrorMessageResourceType = typeof(Resources.Dtos.AgeGroupDto), ErrorMessageResourceName = "AgeMustBeBetween0And125")]
        public int? MinAge { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [Range(0, 125, ErrorMessageResourceType = typeof(Resources.Dtos.AgeGroupDto), ErrorMessageResourceName = "AgeMustBeBetween0And125")]
        public int? MaxAge { get; init; }
        public bool Status { get; init; } = true;
        public string? Name => $"{MinAge} - {MaxAge}";

        public TenantDto? Tenant { get; init; }
    }
}
