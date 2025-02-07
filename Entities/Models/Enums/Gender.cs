using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Enums
{
    public enum Gender
    {
        [Display(Name = "Male")]
        Male,
        [Display(Name = "Female")]
        Female,
    }
}
