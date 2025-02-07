using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Services.Contracts;

namespace LudusAppoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly IStringLocalizer<BranchController> _localizer;

        public BranchController(IServiceManager serviceManager, IStringLocalizer<BranchController> localizer)
        {
            _serviceManager = serviceManager;
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            var model = _serviceManager.BranchService.GetAllBranches(false);
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new BranchDtoForInsert();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create([FromForm] BranchDtoForInsert branchDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                return View(branchDtoForInsert);
            }
            try
            {
                _serviceManager.BranchService.CreateBranch(branchDtoForInsert);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["BranchCreatedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(branchDtoForInsert);
            }
        }

        public IActionResult Update([FromRoute] int id)
        {
            var model = _serviceManager.BranchService.GetBranchForUpdate(id, false);
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update([FromForm] BranchDtoForUpdate branchDtoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View(branchDtoForUpdate);
            }
            try
            {
                _serviceManager.BranchService.UpdateBranch(branchDtoForUpdate);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["BranchUpdatedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(branchDtoForUpdate);
            }
        }
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _serviceManager.BranchService.DeleteBranch(id);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["BranchDeletedSuccessfully"].ToString() + ".";
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
