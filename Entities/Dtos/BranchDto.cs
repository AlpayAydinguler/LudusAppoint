using Entities.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Dtos
{
    public record BranchDto
    {
        public int BranchId { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Dtos.BranchDto), ErrorMessageResourceName = "BranchNameCannotExceed100Characters")]
        public String BranchName { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Dtos.BranchDto), ErrorMessageResourceName = "CountryCannotExceed50Characters")]
        public String Country { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Dtos.BranchDto), ErrorMessageResourceName = "CityCannotExceed50Characters")]
        public String City { get; init; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Dtos.BranchDto), ErrorMessageResourceName = "DistrictCannotExceed50Characters")]
        public String? District { get; init; }

        [StringLength(50, ErrorMessageResourceType = typeof(Resources.Dtos.BranchDto), ErrorMessageResourceName = "NeighbourhoodCannotExceed50Characters")]
        public String? Neighbourhood { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Dtos.BranchDto), ErrorMessageResourceName = "StreetCannotExceed100Characters")]
        public String Street { get; init; }

        [StringLength(200, ErrorMessageResourceType = typeof(Resources.Dtos.BranchDto), ErrorMessageResourceName = "AddressCannotExceed200Characters")]
        public String? Address { get; init; }

        [Phone]
        [StringLength(15, ErrorMessageResourceType = typeof(Resources.Dtos.BranchDto), ErrorMessageResourceName = "PhoneNumberCannotExceed15Characters")]
        public String? BranchPhoneNumber { get; init; }

        [EmailAddress]
        [StringLength(100, ErrorMessageResourceType = typeof(Resources.Dtos.BranchDto), ErrorMessageResourceName = "EmailCannotExceed100Characters")]
        public String? BranchEmail { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [Range(1, 365, ErrorMessageResourceType = typeof(Resources.Dtos.BranchDto), ErrorMessageResourceName = "ReservationInAdvanceDayLimitMustBeBetween1And365")]
        public int ReservationInAdvanceDayLimit { get; init; } = 60;
        public bool Status { get; init; } = true;
    }
}
