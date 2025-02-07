using Entities.Dtos;
using Entities.Models;
using Entities.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System.Runtime;
using System.Text.Json;

namespace LudusAppoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CustomerAppointmentController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly IStringLocalizer<CustomerAppointmentController> _localizer;

        public CustomerAppointmentController(IServiceManager serviceManager, IStringLocalizer<CustomerAppointmentController> localizer)
        {
            _serviceManager = serviceManager;
            _localizer = localizer;
        }
        public IActionResult Index()
        {
            var model = _serviceManager.CustomerAppointmentService.GetAllCustomerAppointments(false, System.Globalization.CultureInfo.CurrentCulture.Name);
            return View(model);
        }
        public IActionResult PendingConfirmationList()
        {
            var model = _serviceManager.CustomerAppointmentService.GetPendingCustomerAppointments(false, System.Globalization.CultureInfo.CurrentCulture.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new CustomerAppointmentDtoForInsert();
            var supportedGendersSetting = await _serviceManager.ApplicationSettingService.GetSettingEntityAsync("SupportedGenders",false);
            ViewBag.SupportedGenders = supportedGendersSetting.Value;
            PopulateBranchesAndAgeGroups();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CustomerAppointmentDtoForInsert customerAppointmentDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SupportedGenders = await _serviceManager.ApplicationSettingService.GetSettingEntityAsync("SupportedGenders", false);
                PopulateBranchesAndAgeGroups();
                return View(customerAppointmentDtoForInsert);
            }
            try
            {
                await _serviceManager.CustomerAppointmentService.CreateCustomerAppointmentAsync(customerAppointmentDtoForInsert);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["CustomerAppointmentCreatedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(customerAppointmentDtoForInsert);
            }
        }

        public async Task<IActionResult> Update([FromRoute] int id)
        {

            var model = await _serviceManager.CustomerAppointmentService.GetCustomerAppointmentUpdateAsync(id, false);
            PopulateBranchesAndAgeGroups();

            ViewBag.AllServices = _serviceManager.OfferedServiceService.GetAllOfferedServices(false, System.Globalization.CultureInfo.CurrentCulture.Name).ToList();
            ViewBag.AllEmployees = _serviceManager.EmployeeService.GetAllEmployees(false).ToList();
            return View(model);
        }

        public IActionResult GetOfferedServices(string genderValue, int ageGroupId)
        {
            List<OfferedService> offeredServices = new List<OfferedService>();
            if (Enum.TryParse<Gender>(genderValue, out var gender))
            {
                offeredServices = _serviceManager.OfferedServiceService
                    .GetAllForCustomerAppointment(gender, ageGroupId, false)
                    .ToList();
            }
            if (!offeredServices.IsNullOrEmpty())
            {
                return Json(offeredServices);
            }
            else
            {
                return Json(new { Result = false, Message = _localizer["NoOfferedServicesAvailable"] + ". " + _localizer["PleaseTryWithDiffrentAgeGroupOrGender"] + "." });
            }
        }

        public IActionResult GetEmployees(int branchId, string offeredServiceIds)
        {
            // Split comma-separated string into list of integers
            var serviceIds = offeredServiceIds?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList() ?? new List<int>();
            var employees = _serviceManager.EmployeeService.GetEmployeesForCustomerAppointment(branchId, serviceIds ?? new List<int>(), false);
            return employees.IsNullOrEmpty()? Json(new {
                                                           Result = false,
                                                           Message = _localizer["NoEmployeesAvailable"] + ". " +
                                                                    _localizer["PleaseTryWithDifferentBranchOrOfferedServices"] + "."
                                                       })
                                                       : Json(employees);
        }

        public IActionResult GetReservedDaysTimes(int employeeId, int branchId)
        {
            return _serviceManager.CustomerAppointmentService.GetReservedDaysTimes(employeeId, branchId);
        }

        private void PopulateBranchesAndAgeGroups()
        {
            ViewBag.AllBranches = _serviceManager.BranchService.GetAllActiveBranches(false).ToList();
            ViewBag.AllAgeGroups = _serviceManager.AgeGroupService.GetAllAgeGroups(false).ToList();
        }
    }
}
