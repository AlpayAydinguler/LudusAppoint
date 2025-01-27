namespace Entities.Models
{
    public class OfferedServiceLocalization
    {
        public int OfferedServiceLocalizationId { get; set; }
        public string Language { get; set; } = "en"; // ISO Language Code
        public string OfferedServiceLocalizationName { get; set; }

        // Foreign Key
        public int OfferedServiceId { get; set; }
        // Navigation Property
        public OfferedService OfferedService { get; set; }
    }
}
