using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class BookingFlowConfig
    {
        public int BookingFlowConfigId { get; set; }

        [ForeignKey("Tenant")]
        public Guid TenantId { get; set; }

        [Required]
        public int BranchId { get; set; }

        // JSON: All steps in order (including disabled ones for reference)
        [Required]
        public string AllStepsInOrder { get; set; } = "[\"Services\", \"DateTime\", \"RoomSelection\", \"Employee\"]";

        // JSON: Only enabled steps in display order
        [Required]
        public string EnabledStepsInOrder { get; set; } = "[\"Services\", \"DateTime\", \"RoomSelection\", \"Employee\"]";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Helper properties
        public List<string> AllStepsList =>
            JsonSerializer.Deserialize<List<string>>(AllStepsInOrder) ??
            new List<string> { "Services", "DateTime", "RoomSelection", "Employee" };

        public List<string> EnabledStepsList =>
            JsonSerializer.Deserialize<List<string>>(EnabledStepsInOrder) ??
            new List<string> { "Services", "DateTime", "RoomSelection", "Employee" };

        // Check if a specific step is enabled
        public bool IsStepEnabled(string stepName) =>
            EnabledStepsList.Contains(stepName);

        // Navigation Properties
        public Tenant Tenant { get; set; }
        public Branch Branch { get; set; }
    }
}
