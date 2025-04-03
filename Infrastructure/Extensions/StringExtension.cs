using System.Text.RegularExpressions;

namespace Infrastructure.Extensions
{
    public static class StringExtension
    {
        public static string NormalizePhoneNumber(this string phone)
        {
            // Remove any non-digit characters
            string digits = Regex.Replace(phone, @"\D", "");
            // If the result has exactly 10 digits, assume it's missing the country code
            if (digits.Length == 10)
            {
                digits = "90" + digits;
            }
            // Prepend a plus sign
            return "+" + digits;
        }
    }
}
