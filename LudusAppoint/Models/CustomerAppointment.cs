using LudusAppoint.Models.Enums;
using Microsoft.AspNetCore.Identity;

namespace LudusAppoint.Models
{
    public class CustomerAppointment
    {
        public int Id { get; set; }
        public required HairdressingService HairdressingService { get; set; }
        public required Hairdresser Hairdresser { get; set; }
        public required TimeSpan Duration { get; set; } 
        public required decimal Price { get; set; }
        public required String Name { get; set; }
        public required String Surname { get; set; }
        public required DateTime DateTime { get; set; }
        public required String PhoneNumber { get; set; }
        public String? EMail { get; set; }
        public required IdentityUser CreatedBy { get; set; }
    }
}
