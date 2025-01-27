using Entities.Models;
using Entities.Models.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
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

        public IActionResult Create()
        {
            var model = new CustomerAppointment();
            model.Status = CustomerAppointmentStatus.CustomerConfirmed;
            PopulateBranchesAndAgeGroups();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] CustomerAppointment customerAppointment, int[] offeredServiceIds, DateTime StartDateTime)
        {
            customerAppointment.StartDateTime = StartDateTime; // Assign StartDateTime to the model
            //customerAppointment.Employee = _serviceManager.EmployeeService.GetOneEmployee(customerAppointment.EmployeeId);
            if (!ModelState.IsValid)
            {
                PopulateBranchesAndAgeGroups();
                return View(customerAppointment);
            }
            _serviceManager.CustomerAppointmentService.CreateAppointment(customerAppointment, offeredServiceIds);
            return RedirectToAction("Index");
        }

        public IActionResult Update([FromRoute] int id)
        {
            var model = _serviceManager.CustomerAppointmentService.GetOneCustomerAppointment(id, false);
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
                return Json(new { Result = false, Message = _localizer["NoOfferedServicesAvailable."] + " " + _localizer["PleaseTryWithDiffrentAgeGroupOrGender."] });
            }
        }

        public IActionResult GetEmployees(int branchId, string offeredServiceIds)
        {
            var serviceIds = JsonSerializer.Deserialize<List<int>>(offeredServiceIds);
            var employees = _serviceManager.EmployeeService.GetEmployeesForForCustomerAppointment(branchId, serviceIds ?? new List<int>(), false);
            if (!employees.IsNullOrEmpty())
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
                    WriteIndented = true
                };
                return Json(employees, options);
            }
            else
            {
                return Json(new { Result = false, Message = _localizer["NoEmployeesAvailable."] + " " + _localizer["PleaseTryWithDiffrentBranchOrOfferedServices."] });
            }
        }

        public IActionResult GetReservedDaysTimes(int employeeId, int branchId)
        {
            return Json(_serviceManager.CustomerAppointmentService.GetReservedDaysTimes(employeeId, branchId));
        }

        private void PopulateBranchesAndAgeGroups()
        {
            ViewBag.AllBranches = _serviceManager.BranchService.GetAllBranches(false).ToList();
            ViewBag.AllAgeGroups = _serviceManager.AgeGroupService.GetAllAgeGroups(false).ToList();
        }
    }
}
