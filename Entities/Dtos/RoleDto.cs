using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record RoleDto
    {
        public string RoleId { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public string RoleName { get; init; }
    }
}
