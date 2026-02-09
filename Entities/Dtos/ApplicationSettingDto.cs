using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record ApplicationSettingDto
    {
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Dtos.ApplicationSettingDto), ErrorMessageResourceName = "KeyCannotBeLongerThan255Characters")]
        [Display(Name = "Setting Key")]
        public string Key { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [ForeignKey("Tenant")]
        public Guid TenantId { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [Display(Name = "Setting Value")]
        public string Value { get; init; }

        [Display(Name = "Last Modified")]
        public DateTime LastModified { get; init; } = DateTime.Now;

        public TenantDto? Tenant { get; init; }
    }
}
