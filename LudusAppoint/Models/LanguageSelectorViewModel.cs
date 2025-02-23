using System.Globalization;

namespace LudusAppoint.Models
{
    public class LanguageSelectorViewModel
    {
        public CultureInfo CurrentCulture { get; set; }
        public List<CultureInfo> SupportedCultures { get; set; }
        public string ReturnUrl { get; set; }
    }
}
