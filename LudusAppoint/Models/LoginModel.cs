using System.ComponentModel.DataAnnotations;

namespace LudusAppoint.Models
{
    public class LoginModel
    {
        private string? returnUrl;
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public string OneTimePassword { get; set; }
        public string ReturnUrl { get => returnUrl ?? "/"; set => returnUrl = value; }
    }
}
