using LudusAppoint.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Services.Contracts;
using System.Globalization;

namespace LudusAppoint.Components
{
    public class LanguageSelectorViewComponent : ViewComponent
    {
        private readonly IServiceManager _serviceManager;
        private readonly IOptions<RequestLocalizationOptions> _locOptions;

        public LanguageSelectorViewComponent(
            IServiceManager serviceManager,
            IOptions<RequestLocalizationOptions> locOptions)
        {
            _serviceManager = serviceManager;
            _locOptions = locOptions;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var model = new LanguageSelectorViewModel
            {
                CurrentCulture = CultureInfo.CurrentUICulture,
                SupportedCultures = _locOptions.Value.SupportedUICultures?.ToList(),
                ReturnUrl = string.IsNullOrEmpty(HttpContext.Request.Path)
                            ? "~/"
                            : $"~{HttpContext.Request.Path}"
            };

            return View(model);
        }
    }
}