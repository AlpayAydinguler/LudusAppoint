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

        public async Task<IActionResult> Index()
        {
            var model = await _serviceManager.EmployeeLeaveService.GetAllEmployeeLeavesAsync(false);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new EmployeeLeaveDtoForInsert();
            await PopulateViewBagAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] EmployeeLeaveDtoForInsert employeeLeaveDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBagAsync();
                return View(employeeLeaveDtoForInsert);
            }
            try
            {
                await _serviceManager.EmployeeLeaveService.CreateEmployeeLeaveAsync(employeeLeaveDtoForInsert);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["EmployeeLeaveCreatedSuccessfully"] + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                await PopulateViewBagAsync();
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(employeeLeaveDtoForInsert);
            }
        }

        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var model = await _serviceManager.EmployeeLeaveService.GetEmployeeLeaveForUpdateAsync(id);
            await PopulateViewBagAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromForm] EmployeeLeaveDtoForUpdate employeeLeaveDtoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                await PopulateViewBagAsync();
                return View(employeeLeaveDtoForUpdate);
            }
            try
            {
                await _serviceManager.EmployeeLeaveService.UpdateEmployeeLeaveAsync(employeeLeaveDtoForUpdate);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["EmployeeLeaveUpdatedSuccessfully"] + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                await PopulateViewBagAsync();
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(employeeLeaveDtoForUpdate);
            }
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _serviceManager.EmployeeLeaveService.DeleteEmployeeLeaveAsync(id);
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

        private async Task PopulateViewBagAsync()
        {
            var employees = await _serviceManager.EmployeeService.GetAllEmployeesAsync(false);
            ViewBag.Employees = new SelectList(employees, "EmployeeId", "EmployeeFullName");
        }
    }
}
