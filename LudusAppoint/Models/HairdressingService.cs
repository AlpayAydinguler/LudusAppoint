using LudusAppoint.Models.Enums;

namespace LudusAppoint.Models
{
    public class HairdressingService
    {
        public int Id { get; set; }
        public required String Name { get; set; }
        public required String Description { get; set; }
        public required IEnumerable<Gender> Gender { get; set; }
        public required IEnumerable<AgeGroup> AgeGroups { get; set; }
        public required TimeSpan Duration { get; set; }
        public decimal Price { get; set; }
    }
}
