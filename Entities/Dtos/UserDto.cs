using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record UserDto
    {
        public string UserId { get; init; }
        [Phone]
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [RegularExpression(@"^\(\d{3}\)\s\d{3}\s\d{2}\s\d{2}$",
                  ErrorMessageResourceType = typeof(Resources.Dtos.RegisterDto),
                  ErrorMessageResourceName = "PhoneFormat")]
        public String? PhoneNumber { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [StringLength(50, MinimumLength = 2,
                     ErrorMessageResourceType = typeof(Resources.Dtos.RegisterDto),
                     ErrorMessageResourceName = "NameValidation")]
        public string Name { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [StringLength(50, MinimumLength = 2,
                     ErrorMessageResourceType = typeof(Resources.Dtos.RegisterDto),
                     ErrorMessageResourceName = "SurnameValidation")]
        public string Surname { get; init; }
        public string UserName => $"{Name}{Surname}";
        [EmailAddress]
        public string Email { get; init; }
        public bool? IsActive { get; init; }
    }
}
