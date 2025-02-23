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

        [HttpGet]
        public async Task<IActionResult> Change(string culture, string returnUrl)
        {
            var fixedFormattingCulture = await _serviceManager.ApplicationSettingService
                                      .GetSettingEntityAsync("Currency", false);

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(fixedFormattingCulture.Value, culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}
