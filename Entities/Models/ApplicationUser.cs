using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
