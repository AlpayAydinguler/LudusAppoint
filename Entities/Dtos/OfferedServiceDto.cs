using Entities.Models.Enums;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record OfferedServiceDto
    {
        public int OfferedServiceId { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public String OfferedServiceName { get; init; }
        public ICollection<Gender> Genders { get; init; } = new List<Gender>();
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public TimeSpan ApproximateDuration { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [DataType(DataType.Currency)]
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
