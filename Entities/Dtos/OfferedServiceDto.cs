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
        [StringLength(100, MinimumLength = 3, ErrorMessageResourceType = typeof(Resources.Dtos.OfferedServiceDto), ErrorMessageResourceName = "ServiceNameIsRequired")]
        public String OfferedServiceName { get; init; }

        [Column(TypeName = "nvarchar(max)")]
        public List<Gender> Genders { get; init; } = new List<Gender>();

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [DisplayFormat(DataFormatString = "{0:hh\\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan ApproximateDuration { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [DataType(DataType.Currency)]
        [Range(1.00, 10000.00, ErrorMessageResourceType = typeof(Resources.Dtos.OfferedServiceDto), ErrorMessageResourceName = "ThePriceMustBeBetween1And10000")]
        [Precision(18, 2)]
        public decimal Price { get; init; }

        public bool Status { get; init; } = false;

        //Foreign Keys
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingBindRequiredValueAccessor")]
        public List<AgeGroupDto> AgeGroups { get; init; } = new();
        public ICollection<OfferedServiceLocalization>? OfferedServiceLocalizations { get; init; }

        //Navigation Properties
        public ICollection<Employee>? Employees { get; init; }
        public ICollection<CustomerAppointment>? CustomerAppointments { get; init; }
    }
}
