using System.ComponentModel.DataAnnotations;

namespace Entities.Models.Enums
{
    public enum Gender
    {
        [Display(Name = "Male")]
        m,
        [Display(Name = "Female")]
        f,
    }
}
