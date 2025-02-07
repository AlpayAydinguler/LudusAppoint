using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Services.Contracts;

namespace LudusAppoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeLeaveController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly IStringLocalizer<EmployeeLeaveController> _localizer;

        public EmployeeLeaveController(IServiceManager serviceManager, IStringLocalizer<EmployeeLeaveController> localizer)
        {
            _serviceManager = serviceManager;
            _localizer = localizer;
        }

        public IActionResult Index()
        {
            var model = _serviceManager.EmployeeLeaveService.GetAllEmployeeLeaves(false);
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new EmployeeLeaveDtoForInsert();
            PopulateViewBag();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] EmployeeLeaveDtoForInsert employeeLeaveDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBag();
                return View(employeeLeaveDtoForInsert);
            }
            try
            {
                _serviceManager.EmployeeLeaveService.CreateEmployeeLeave(employeeLeaveDtoForInsert);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["EmployeeLeaveCreatedSuccessfully"] + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                PopulateViewBag();
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(employeeLeaveDtoForInsert);
            }
        }

        public IActionResult Update([FromRoute] int id)
        {
            var model = _serviceManager.EmployeeLeaveService.GetEmployeeLeaveForUpdate(id);
            PopulateViewBag();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update([FromForm] EmployeeLeaveDtoForUpdate employeeLeaveDtoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBag();
                return View(employeeLeaveDtoForUpdate);
            }
            try
            {
                _serviceManager.EmployeeLeaveService.UpdateEmployeeLeave(employeeLeaveDtoForUpdate);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["EmployeeLeaveUpdatedSuccessfully"] + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                PopulateViewBag();
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(employeeLeaveDtoForUpdate);
            }
        }
        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _serviceManager.EmployeeLeaveService.DeleteEmployeeLeave(id);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["EmployeeLeaveDeletedSuccessfully"] + ".";
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = exception.Message;
                return RedirectToAction("Index");
            }
        }
        private void PopulateViewBag()
        {
            ViewBag.Employees = new SelectList(_serviceManager.EmployeeService.GetAllEmployees(false), "EmployeeId", "EmployeeFullName");
        }
    }
}
