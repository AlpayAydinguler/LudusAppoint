using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Entities.Dtos
{
    public record BookingFlowConfigDto
    {
        public int BookingFlowConfigId { get; init; }

        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        [Range(1, int.MaxValue,
              ErrorMessageResourceType = typeof(Resources.Dtos.BookingFlowConfigDto),
              ErrorMessageResourceName = "InvalidBranch")]
        public int BranchId { get; init; }

        // All possible steps
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public string AllStepsInOrder { get; init; } = "[\"Services\", \"DateTime\", \"RoomSelection\", \"Employee\"]";

        // Only enabled steps
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources),
                  ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public string EnabledStepsInOrder { get; init; } = "[\"Services\", \"DateTime\", \"RoomSelection\", \"Employee\"]";

        // Helper properties
        public List<string> AllStepsList =>
            JsonSerializer.Deserialize<List<string>>(AllStepsInOrder) ??
            new List<string> { "Services", "DateTime", "RoomSelection", "Employee" };

        public List<string> EnabledStepsList =>
            JsonSerializer.Deserialize<List<string>>(EnabledStepsInOrder) ??
            new List<string> { "Services", "DateTime", "RoomSelection", "Employee" };

        public bool IsServicesEnabled => IsStepEnabled("Services");
        public bool IsDateTimeEnabled => IsStepEnabled("DateTime");
        public bool IsRoomSelectionEnabled => IsStepEnabled("RoomSelection");
        public bool IsEmployeeEnabled => IsStepEnabled("Employee");

        private bool IsStepEnabled(string step) => EnabledStepsList.Contains(step);

        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }

        public BranchDto? Branch { get; init; }
    }
}