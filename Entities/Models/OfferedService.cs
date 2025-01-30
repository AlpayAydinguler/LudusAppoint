using Entities.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities.Models
{
    public class OfferedService
    {
        public int OfferedServiceId { get; set; }
        public String OfferedServiceName { get; set; }
        public List<Gender> Genders { get; set; } = new List<Gender>();
        public TimeSpan ApproximateDuration { get; set; }
        public decimal Price { get; set; }
        public bool Status { get; set; } = false;

        //Foreign Keys
        public ICollection<AgeGroup> AgeGroups { get; set; } = [];
        public ICollection<OfferedServiceLocalization>? OfferedServiceLocalizations { get; set; }

        //Navigation Properties
        public ICollection<Employee>? Employees { get; set; }
        public ICollection<CustomerAppointment>? CustomerAppointments { get; set; }

    }
}