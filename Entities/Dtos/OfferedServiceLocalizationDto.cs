using Entities.Models;

namespace Entities.Dtos
{
    public record OfferedServiceLocalizationDto
    {
        public int OfferedServiceLocalizationId { get; init; }
        public string Language { get; init; } = "en"; // ISO Language Code
        public string OfferedServiceLocalizationName { get; init; }

        // Foreign Key
        public int OfferedServiceId { get; init; }
        // Navigation Property
        public OfferedService OfferedService { get; init; }
    }
}
