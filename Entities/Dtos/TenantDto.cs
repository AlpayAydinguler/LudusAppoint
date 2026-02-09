using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record TenantDto
    {
        public Guid Id { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Dtos.TenantDto), ErrorMessageResourceName = "NameCannotExceed100Characters")]
        public string Name { get; init; } = null!;

        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Dtos.TenantDto), ErrorMessageResourceName = "HostnameCannotExceed100Characters")]
        public string? Hostname { get; init; }

        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}
