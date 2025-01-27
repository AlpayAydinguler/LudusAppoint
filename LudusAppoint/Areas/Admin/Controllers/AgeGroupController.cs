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

        public IActionResult Index()
        {
            var model = _serviceManager.AgeGroupService.GetAllAgeGroups(false);
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new AgeGroupDtoForInsert();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create([FromForm] AgeGroupDtoForInsert ageGroupDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                return View(ageGroupDtoForInsert);
            }
            try
            {
                _serviceManager.AgeGroupService.CreateAgeGroup(ageGroupDtoForInsert);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["AgeGroupCreatedSuccessfully."].ToString();
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString(), exception.Message);
                }
                return View(ageGroupDtoForInsert);
            }
        }

        public IActionResult Update([FromRoute] int id)
        {
            var model = _serviceManager.AgeGroupService.GetAgeGroupForUpdate(id, false);
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update([FromForm] AgeGroupDtoForUpdate ageGroupDtoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(ageGroupDtoForUpdate);
            }
            try
            {
                _serviceManager.AgeGroupService.UpdateAgeGroup(ageGroupDtoForUpdate);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["AgeGroupUpdatedSuccessfully."].ToString();
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString(), exception.Message);
                }
                return View(ageGroupDtoForUpdate);
            }
        }
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _serviceManager.AgeGroupService.DeleteAgeGroup(id);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["AgeGroupDeletedSuccessfully."].ToString();
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
