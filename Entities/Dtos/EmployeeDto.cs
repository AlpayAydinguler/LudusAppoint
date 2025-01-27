using Entities.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Dtos
{
    public record EmployeeDto
    {
        public int EmployeeId { get; init; }
        [ValidateNever]
        public String? IdentityUserId { get; init; }
        [ForeignKey("Branch")]
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public int BranchId { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public String EmployeeName { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public String EmployeeSurname { get; init; }
        [Required(ErrorMessageResourceType = typeof(Resources.SharedResources), ErrorMessageResourceName = "MissingKeyOrValueAccessor")]
        public String EmployeeFullName => $"{EmployeeName} {EmployeeSurname}";
        public Boolean CanTakeClients { get; init; }

        public DayOfWeek DayOff { get; init; }
        public TimeSpan StartOfWorkingHours { get; init; }
        public TimeSpan EndOfWorkingHours { get; init; }

        //Foreign Keys
        public ICollection<OfferedService> OfferedServices { get; init; } = new List<OfferedService>();
        [ValidateNever]
        public Branch Branch { get; init; }

        //Navigation Properties
        public ICollection<CustomerAppointment>? CustomerAppointment { get; init; }
        public ICollection<EmployeeLeave>? EmployeesLeaves { get; init; }
    }
}
