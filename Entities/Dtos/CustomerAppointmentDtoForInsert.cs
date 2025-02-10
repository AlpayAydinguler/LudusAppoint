using System.ComponentModel.DataAnnotations;

namespace Entities.Dtos
{
    public record CustomerAppointmentDtoForInsert : CustomerAppointmentDto
    {
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [MinLength(1,
                  ErrorMessageResourceType = typeof(Resources.Dtos.CustomerAppointmentDto),
                  ErrorMessageResourceName = "AtLeastOneServiceMustBeSelected")]
        public List<int> OfferedServiceIds { get; init; } = new List<int>();
    }
}
