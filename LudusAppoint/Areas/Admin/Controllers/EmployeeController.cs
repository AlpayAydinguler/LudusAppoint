using Entities.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Services.Contracts;

namespace LudusAppoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly IStringLocalizer<EmployeeController> _localizer;

        public EmployeeController(IServiceManager serviceManager, IStringLocalizer<EmployeeController> localizer)
        {
            _serviceManager = serviceManager;
            _localizer = localizer;
        }

        public async Task<IActionResult> Index()
        {
            var model = await _serviceManager.EmployeeService.GetAllEmployeesAsync(false);
            return View(model);
        }

        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var model = await _serviceManager.EmployeeService.GetOneEmployeeForUpdateAsync(id, false, System.Globalization.CultureInfo.CurrentCulture.Name);
            await PopulatePageDataAsync();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update([FromForm] EmployeeDtoForUpdate employeeDtoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                await PopulatePageDataAsync();
                return View(employeeDtoForUpdate);
            }
            try
            {
                await _serviceManager.EmployeeService.UpdateEmployeeAsync(employeeDtoForUpdate);
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
                await PopulatePageDataAsync();
                return View(employeeDtoForUpdate);
            }
        }

        public async Task<IActionResult> Create()
        {
            var model = new EmployeeDtoForInsert();
            await PopulatePageDataAsync();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create([FromForm] EmployeeDtoForInsert employeeDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                await PopulatePageDataAsync();
                return View(employeeDtoForInsert);
            }

            try
            {
                await _serviceManager.EmployeeService.CreateEmployeeAsync(employeeDtoForInsert);
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
                await PopulatePageDataAsync();
                return View(employeeDtoForInsert);
            }
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            try
            {
                await _serviceManager.EmployeeService.DeleteEmployeeAsync(id);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["EmployeeDeletedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (Exception exception)
            {
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = exception.Message.ToString();
                return RedirectToAction("Index");
            }
        }

        private async Task PopulatePageDataAsync()
        {
            var offeredServices = await _serviceManager.OfferedServiceService.GetActiveOfferedServicesAsync(false, System.Globalization.CultureInfo.CurrentCulture.Name);
            ViewBag.AllOfferedServices = offeredServices;
            var branches = await _serviceManager.BranchService.GetAllActiveBranchesAsync(false);
            ViewBag.AllBranches = branches.ToList();
        }
    }
}
