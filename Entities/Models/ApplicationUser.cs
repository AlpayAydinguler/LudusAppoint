using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class ApplicationUser : IdentityUser
    {
        [ForeignKey("Tenant")]
        public Guid TenantId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime LastLogin { get; set; }
        public bool IsActive { get; set; } = true;

        public Tenant Tenant { get; set; }
    }
}
