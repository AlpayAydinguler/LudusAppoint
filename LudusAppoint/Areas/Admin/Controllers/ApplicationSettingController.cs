using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Services.Contracts;
using System.Runtime;

namespace LudusAppoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ApplicationSettingController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly IStringLocalizer<ApplicationSettingController> _localizer;

        public ApplicationSettingController(IServiceManager serviceManager, IStringLocalizer<ApplicationSettingController> localizer)
        {
            _serviceManager = serviceManager;
            _localizer = localizer;
        }
        public async Task<IActionResult> Index()
        {
            var model = await _serviceManager.ApplicationSettingService.GetAllApplicationSettingsAsync(false);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new ApplicationSettingDtoForInsert();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] ApplicationSettingDtoForInsert applicationSettingDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                return View(applicationSettingDtoForInsert);
            }
            try
            {
                await _serviceManager.ApplicationSettingService.CreateApplicationSettingAsync(applicationSettingDtoForInsert);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["ApplicationSettingCreatedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(applicationSettingDtoForInsert);
            }
        }

        public async Task<IActionResult> Update([FromRoute(Name = "id")] string key)
        {
            var model = await _serviceManager.ApplicationSettingService.GetApplicationSettingForUpdateAsync(key, false);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] ApplicationSettingDtoForUpdate applicationSettingDtoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(applicationSettingDtoForUpdate);
            }
            try
            {
                await _serviceManager.ApplicationSettingService.UpdateApplicationSettingAsync(applicationSettingDtoForUpdate);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["ApplicationSettingUpdatedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(applicationSettingDtoForUpdate);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute(Name = "id")] string key)
        {
            try
            {
                await _serviceManager.ApplicationSettingService.DeleteApplicationSettingAsync(key);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["ApplicationSettingDeletedSuccessfully"].ToString() + ".";
            }
            catch (Exception exception)
            {
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = exception.Message.ToString();
            }
            return RedirectToAction("Index");
        }
    }
}
