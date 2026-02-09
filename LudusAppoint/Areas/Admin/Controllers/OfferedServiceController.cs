using Entities.Dtos;
using Entities.Models;
using Entities.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Services.Contracts;

namespace LudusAppoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OfferedServiceController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly RequestLocalizationOptions _localizationOptions;
        private readonly IStringLocalizer<OfferedServiceController> _localizer;

        public OfferedServiceController(IServiceManager serviceManager, IOptions<RequestLocalizationOptions> localizationOptions, IStringLocalizer<OfferedServiceController> localizer)
        {
            _serviceManager = serviceManager;
            _localizationOptions = localizationOptions.Value;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _serviceManager.OfferedServiceService.GetAllOfferedServicesAsync(false, System.Globalization.CultureInfo.CurrentCulture.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            PopulateAgeGroups();
            PopulateSupportedCultures();
            var model = new OfferedServiceDtoForInsert();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([FromForm] OfferedServiceDtoForInsert offeredServiceDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                PopulateAgeGroups();
                PopulateSupportedCultures();
                return View(offeredServiceDtoForInsert);
            }

            try
            {
                await _serviceManager.OfferedServiceService.CreateOfferedServiceAsync(offeredServiceDtoForInsert);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["OfferedServiceCreatedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception.InnerException?.Source ?? "General", exception.Message);
                }
                PopulateAgeGroups();
                PopulateSupportedCultures();
                return View(offeredServiceDtoForInsert);
            }
        }

        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var model = await _serviceManager.OfferedServiceService.GetOfferedServiceForUpdateAsync(id, false);
            PopulateAgeGroups();
            PopulateSupportedCultures();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update([FromForm] OfferedServiceDtoForUpdate offeredServiceDtoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                PopulateAgeGroups();
                PopulateSupportedCultures();
                return View(offeredServiceDtoForUpdate);
            }
            try
            {
                await _serviceManager.OfferedServiceService.UpdateOfferedServiceAsync(offeredServiceDtoForUpdate);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["OfferedServiceUpdatedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                PopulateAgeGroups();
                PopulateSupportedCultures();
                return View(offeredServiceDtoForUpdate);
            }
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _serviceManager.OfferedServiceService.DeleteOfferedServiceAsync(id);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["OfferedServiceDeletedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = exception.Message.ToString();
                return RedirectToAction("Index");
            }
        }

        private void PopulateSupportedCultures()
        {
            ViewBag.SupportedCultures = _localizationOptions.SupportedCultures.Select(c => c.Name).ToList();
        }

        private async void PopulateAgeGroups()
        {
            var ageGroups = await _serviceManager.AgeGroupService.GetAllAgeGroupsAsync(false);
            ViewBag.AllAgeGroups = ageGroups.ToList();
        }
    }
}
