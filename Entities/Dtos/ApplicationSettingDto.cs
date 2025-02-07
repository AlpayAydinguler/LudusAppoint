using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record ApplicationSettingDto
    {
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [MaxLength(255, ErrorMessageResourceType = typeof(Resources.Dtos.ApplicationSettingDto), ErrorMessageResourceName = "KeyCannotBeLongerThan255Characters")]
        [Display(Name = "Setting Key")]
        public string Key { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [Display(Name = "Setting Value")]
        public string Value { get; init; }

        [Display(Name = "Last Modified")]
        public DateTime LastModified { get; init; }
    }
}
