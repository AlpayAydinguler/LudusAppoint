using System.ComponentModel.DataAnnotations;

namespace LudusAppoint.Models
{
    public class AgeGroup
    {
        public int Id { get; set; }

        public required String Name { get; set; }

        public required int MinAge { get; set; }

        public required int MaxAge { get; set; }
    }
}
