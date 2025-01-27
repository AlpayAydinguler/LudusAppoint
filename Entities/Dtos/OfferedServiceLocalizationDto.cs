using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
