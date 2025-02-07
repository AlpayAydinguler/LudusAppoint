using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Services.Contracts;
using System;
using System.Globalization;

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

        public IActionResult Index()
        {
            var model = _serviceManager.EmployeeService.GetAllEmployees(false);
            return View(model);
        }
        public IActionResult Update([FromRoute] int id)
        {
            var model = _serviceManager.EmployeeService.GetOneEmployeeForUpdate(id, false, System.Globalization.CultureInfo.CurrentCulture.Name);
            PopulatePageData();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update([FromForm] EmployeeDtoForUpdate employeeDtoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                PopulatePageData();
                return View(employeeDtoForUpdate);
            }
            try
            {
                _serviceManager.EmployeeService.UpdateEmployee(employeeDtoForUpdate);
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
                PopulatePageData();
                return View(employeeDtoForUpdate);
            }
        }
        public IActionResult Create()
        {
            var model = new EmployeeDtoForInsert();
            PopulatePageData();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create([FromForm] EmployeeDtoForInsert employeeDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                PopulatePageData();
                return View(employeeDtoForInsert);
            }

            try
            {
                _serviceManager.EmployeeService.CreateEmployee(employeeDtoForInsert);
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
                return View(employeeDtoForInsert);
            }
        }

        [HttpGet]
        [AutoValidateAntiforgeryToken]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                _serviceManager.EmployeeService.DeleteEmployee(id);
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

        private void PopulatePageData()
        {
            var offeredServices = _serviceManager.OfferedServiceService.GetActiveOfferedServices(false, System.Globalization.CultureInfo.CurrentCulture.Name);
            ViewBag.AllOfferedServices = offeredServices;
            ViewBag.AllBranches = _serviceManager.BranchService.GetAllActiveBranches(false).ToList();
            //ViewBag.AllBranches = _serviceManager.BranchService.GetAllBranches(false).ToList();
        }
    }
}
