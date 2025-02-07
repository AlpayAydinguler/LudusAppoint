using System.ComponentModel.DataAnnotations;

namespace Entities.Models
{
    public class ApplicationSetting
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTime LastModified { get; set; } = DateTime.Now;
    }
}
