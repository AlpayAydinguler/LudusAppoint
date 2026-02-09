using Entities.Dtos;
using Entities.Models;
using Entities.Models.Enums;
using LudusAppoint.Models.Enums;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Policy = nameof(Permissions.CustomerAppointment_Index))]
        public async Task<IActionResult> Index()
        {
            var model = await _serviceManager.CustomerAppointmentService.GetAllCustomerAppointmentsAsync(false, System.Globalization.CultureInfo.CurrentCulture.Name);
            return View(model);
        }

        public async Task<IActionResult> PendingConfirmationList()
        {
            var model = await _serviceManager.CustomerAppointmentService.GetPendingCustomerAppointmentsAsync(false, System.Globalization.CultureInfo.CurrentCulture.Name);
            return View(model);
        }

        public async Task<IActionResult> Create()
        {
            var model = new CustomerAppointmentDtoForInsert();
            await PopulateBranchesAndAgeGroupsAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] CustomerAppointmentDtoForInsert customerAppointmentDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.SupportedGenders = await _serviceManager.ApplicationSettingService.GetSettingEntityAsync("SupportedGenders", false);
                await PopulateBranchesAndAgeGroupsAsync();
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
                await PopulateBranchesAndAgeGroupsAsync();
                return View(customerAppointmentDtoForInsert);
            }
        }

        [Authorize(Policy = nameof(Permissions.CustomerAppointment_Update))]
        public async Task<IActionResult> Update([FromRoute] int id)
        {
            var model = await _serviceManager.CustomerAppointmentService.GetCustomerAppointmentForUpdateAsync(id, false, System.Globalization.CultureInfo.CurrentCulture.Name);
            await PopulateBranchesAndAgeGroupsAsync();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(Permissions.CustomerAppointment_Update))]
        public async Task<IActionResult> Update([FromForm] CustomerAppointmentDtoForUpdate customerAppointmentDtoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                await PopulateBranchesAndAgeGroupsAsync();
                return View(customerAppointmentDtoForUpdate);
            }
            try
            {
                await _serviceManager.CustomerAppointmentService.UpdateCustomerAppointmentAsync(customerAppointmentDtoForUpdate);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["CustomerAppointmentUpdatedSuccessfully"].ToString() + ".";
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                await PopulateBranchesAndAgeGroupsAsync();
                var offeredServices = await _serviceManager.OfferedServiceService.GetAllOfferedServicesAsync(false, System.Globalization.CultureInfo.CurrentCulture.Name);
                ViewBag.AllServices = offeredServices.ToList();
                var employees = await _serviceManager.EmployeeService.GetAllEmployeesAsync(false);
                ViewBag.AllEmployees = employees.ToList();
                return View(customerAppointmentDtoForUpdate);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApproveAppointment([FromRoute] int id)
        {
            try
            {
                await _serviceManager.CustomerAppointmentService.ChangeStatusAsync(id, CustomerAppointmentStatus.Confirmed);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["CustomerAppointmentApprovedSuccessfully"].ToString() + ".";
                return RedirectToAction("PendingConfirmationList");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = _localizer["CancellationFailed"].ToString();
                return RedirectToAction("PendingConfirmationList");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelAppointment([FromRoute] int id)
        {
            try
            {
                await _serviceManager.CustomerAppointmentService.ChangeStatusAsync(id, CustomerAppointmentStatus.Cancelled);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["CustomerAppointmentCancelledSuccessfully"].ToString() + ".";
                return RedirectToAction("PendingConfirmationList");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = _localizer["CancellationFailed"].ToString();
                return RedirectToAction("PendingConfirmationList");
            }
        }

        public async Task<IActionResult> GetOfferedServices(string genderValue, int ageGroupId)
        {
            List<OfferedService> offeredServices = new List<OfferedService>();
            if (Enum.TryParse<Gender>(genderValue, out var gender))
            {
                offeredServices = (await _serviceManager.OfferedServiceService
                    .GetAllForCustomerAppointmentAsync(gender, ageGroupId, false, System.Globalization.CultureInfo.CurrentCulture.Name))
                    .ToList();
            }
            if (offeredServices != null && offeredServices.Count > 0)
            {
                return Json(offeredServices);
            }
            else
            {
                return Json(new { Result = false, Message = _localizer["NoOfferedServicesAvailable"] + ". " + _localizer["PleaseTryWithDiffrentAgeGroupOrGender"] + "." });
            }
        }

        public async Task<IActionResult> GetEmployees(int branchId, string offeredServiceIds)
        {
            var serviceIds = offeredServiceIds?
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(int.Parse)
                .ToList() ?? new List<int>();
            var employees = await _serviceManager.EmployeeService.GetEmployeesForCustomerAppointmentAsync(branchId, serviceIds ?? new List<int>(), false);
            return (employees == null || !employees.Any()) ? Json(new
            {
                Result = false,
                Message = _localizer["NoEmployeesAvailable"] + ". " +
                                                                            _localizer["PleaseTryWithDifferentBranchOrOfferedServices"] + "."
            })
                                                           : Json(employees);
        }

        public async Task<IActionResult> GetReservedDaysTimes(int employeeId, int branchId)
        {
            var reservedDaysTimes = await _serviceManager.CustomerAppointmentService.GetReservedDaysTimesAsync(employeeId, branchId);
            return Json(reservedDaysTimes);
        }

        private async Task PopulateBranchesAndAgeGroupsAsync()
        {
            var supportedGendersSetting = await _serviceManager.ApplicationSettingService.GetSettingEntityAsync("SupportedGenders", false);
            ViewBag.SupportedGenders = supportedGendersSetting.Value;
            var branches = await _serviceManager.BranchService.GetAllActiveBranchesAsync(false);
            ViewBag.AllBranches = branches.ToList();
            var ageGroups = await _serviceManager.AgeGroupService.GetAllAgeGroupsAsync(false);
            ViewBag.AllAgeGroups = ageGroups.ToList();
        }
    }
}
