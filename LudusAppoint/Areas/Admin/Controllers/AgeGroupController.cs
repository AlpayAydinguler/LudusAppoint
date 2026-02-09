using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Services.Contracts;

namespace LudusAppoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AgeGroupController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly IStringLocalizer<AgeGroupController> _localizer;

        public AgeGroupController(IServiceManager serviceManager, IStringLocalizer<AgeGroupController> localizer)
        {
            _serviceManager = serviceManager;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _serviceManager.AgeGroupService.GetAllAgeGroupsAsync(false);
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new AgeGroupDtoForInsert();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([FromForm] AgeGroupDtoForInsert ageGroupDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                return View(ageGroupDtoForInsert);
            }
            try
            {
                await _serviceManager.AgeGroupService.CreateAgeGroupAsync(ageGroupDtoForInsert);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["AgeGroupCreatedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(ageGroupDtoForInsert);
            }
        }

        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var model = await _serviceManager.AgeGroupService.GetAgeGroupForUpdateAsync(id, false);
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update([FromForm] AgeGroupDtoForUpdate ageGroupDtoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(ageGroupDtoForUpdate);
            }
            try
            {
                await _serviceManager.AgeGroupService.UpdateAgeGroupAsync(ageGroupDtoForUpdate);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["AgeGroupUpdatedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(ageGroupDtoForUpdate);
            }
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _serviceManager.AgeGroupService.DeleteAgeGroupAsync(id);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["AgeGroupDeletedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = exception.Message.ToString();
                return RedirectToAction("Index");
            }
        }
    }
}
