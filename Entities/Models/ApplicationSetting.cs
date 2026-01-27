using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class ApplicationSetting
    {
        [Key]
        public string Key { get; set; }
        [ForeignKey("Tenant")]
        public Guid TenantId { get; set; }
        public string Value { get; set; }
        public DateTime LastModified { get; set; }

        public Tenant Tenant { get; set; }
    }
}
