using Entities.Models;
using Entities.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record OfferedServiceDto
    {
        public int OfferedServiceId { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [StringLength(100, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources.OfferedServiceDto), ErrorMessageResourceName = "ServiceNameIsRequired.")]
        public String OfferedServiceName { get; init; }
        [Column(TypeName = "nvarchar(max)")]
        public List<Gender> Genders { get; init; } = new List<Gender>();
        [Range(1, 1440)]  // 1 minute to 24 hours (1440 minutes)
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public int DurationMinutes { get; init; }
        [NotMapped]
        public TimeSpan ApproximateDuration { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [DataType(DataType.Currency)]
        [Range(0.01, 10000.00, ErrorMessageResourceType = typeof(Resources.OfferedServiceDto), ErrorMessageResourceName = "AgeMustBeBetween0And125")]
        [Precision(18, 2)]  // For EF Core 6+ (mapped to decimal(18,2) in SQL)
        public decimal Price { get; init; }
        public bool Status { get; init; } = false;

        //Foreign Keys
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingBindRequiredValueAccessor")]
        public ICollection<AgeGroup> AgeGroups { get; init; } = [];
        public ICollection<OfferedServiceLocalization>? OfferedServiceLocalizations { get; init; }

        //Navigation Properties
        public ICollection<Employee>? Employees { get; init; }
        public ICollection<CustomerAppointment>? CustomerAppointments { get; init; }
    }
}
