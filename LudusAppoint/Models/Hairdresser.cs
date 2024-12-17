

using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;

namespace LudusAppoint.Models
{
    public class Hairdresser
    {
        public int Id { get; set; }
        public required String IdentityUserId { get; set; }
        public required String Name { get; set; }
        public required String Surname { get; set; }
        public required IEnumerable<HairdressingService> HairdressingServices { get; set; }
        public required Boolean Status { get; set; }
    }
}
