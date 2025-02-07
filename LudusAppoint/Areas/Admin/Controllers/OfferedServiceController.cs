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
        public IActionResult Index()
        {
            var model = _serviceManager.OfferedServiceService.GetAllOfferedServices(false, System.Globalization.CultureInfo.CurrentCulture.Name);
            return View(model);
        }
        public IActionResult Create()
        {
            PopulateAgeGroups();
            PopulateSupportedCultures();
            var model = new OfferedServiceDtoForInsert(); // Ensure a valid model is created
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create([FromForm] OfferedServiceDtoForInsert offeredServiceDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns/ViewBag data for the form
                PopulateAgeGroups();
                PopulateSupportedCultures();
                return View(offeredServiceDtoForInsert);
            }

            try
            {
                _serviceManager.OfferedServiceService.CreateOfferedService(offeredServiceDtoForInsert);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["OfferedServiceCreatedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                // Handle validation/domain errors
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception.InnerException?.Source ?? "General", exception.Message);
                }
                PopulateAgeGroups();
                PopulateSupportedCultures();
                return View(offeredServiceDtoForInsert);
            }
        }

        public IActionResult Update([FromRoute] int id)
        {
            var model = _serviceManager.OfferedServiceService.GetOfferedServiceForUpdate(id, false);
            PopulateAgeGroups();
            PopulateSupportedCultures();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update([FromForm] OfferedServiceDtoForUpdate offeredServiceDtoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                PopulateAgeGroups();
                PopulateSupportedCultures();
                return View(offeredServiceDtoForUpdate);
            }
            try
            {
                _serviceManager.OfferedServiceService.UpdateOfferedService(offeredServiceDtoForUpdate);
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
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _serviceManager.OfferedServiceService.DeleteOfferedService(id);
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

        private void PopulateAgeGroups()
        {
            var ageGroups = _serviceManager.AgeGroupService.GetAllAgeGroups(false).ToList();
            ViewBag.AllAgeGroups = ageGroups;
        }
    }
}
