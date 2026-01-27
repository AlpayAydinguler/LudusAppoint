using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace LudusAppoint.Controllers
{
    public class LanguageController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public LanguageController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost] // Change to HttpPost for security
        [ValidateAntiForgeryToken] // Add this for security
        public async Task<IActionResult> Change(string culture, string returnUrl)
        {
            // Validate parameters
            if (string.IsNullOrEmpty(culture))
            {
                return BadRequest("Culture parameter is required");
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = "~/"; // Default to home
            }

            var fixedFormattingCulture = await _serviceManager.ApplicationSettingService
                                      .GetSettingEntityAsync("Currency", false);

            // Check if the setting exists
            if (fixedFormattingCulture == null || string.IsNullOrEmpty(fixedFormattingCulture.Value))
            {
                // Fallback to the selected culture for both UI and formatting
                fixedFormattingCulture = new Entities.Models.ApplicationSetting { Value = culture };
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(fixedFormattingCulture.Value, culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true // Make sure cookie is always set
                }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
