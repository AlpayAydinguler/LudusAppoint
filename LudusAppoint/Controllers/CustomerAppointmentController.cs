using Entities.Dtos;
using Entities.Models;
using LudusAppoint.Dtos;
using LudusAppoint.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Services.Contracts;

namespace LudusAppoint.Controllers
{
    public class CustomerAppointmentController : Controller
    {
        private readonly IStringLocalizer<CustomerAppointmentController> _localizer;
        private readonly IServiceManager _serviceManager;

        public CustomerAppointmentController(IStringLocalizer<CustomerAppointmentController> localizer, IServiceManager serviceManager)
        {
            _localizer = localizer;
            _serviceManager = serviceManager;
        }
        /*
        public async Task<IActionResult> Index()
        {
            var model = _serviceManager.CustomerAppointmentService.GetCustomersAppointments(false);
            return View(model);
        }
        */
        public async Task<IActionResult> Create()
        {
            var model = HttpContext.Session.GetJson<SessionCustomerAppointmentDtoForInsert>("CustomerAppointment") ?? new SessionCustomerAppointmentDtoForInsert();
            await PopulateBranchesAndAgeGroupsAsync();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] SessionCustomerAppointmentDtoForInsert sessionCustomerAppointmentDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                HttpContext.Session.SetJson("CustomerAppointment", sessionCustomerAppointmentDtoForInsert);
                return View(sessionCustomerAppointmentDtoForInsert);
            }
            try
            {
                await _serviceManager.CustomerAppointmentService.CreateCustomerAppointmentAsync(sessionCustomerAppointmentDtoForInsert);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["CustomerAppointmentCreatedSuccessfully"].ToString() + ".";
                HttpContext.Session.Remove("CustomerAppointment");
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                await PopulateBranchesAndAgeGroupsAsync();
                return View(sessionCustomerAppointmentDtoForInsert);
            }
        }

        private async Task PopulateBranchesAndAgeGroupsAsync()
        {
            var supportedGendersSetting = await _serviceManager.ApplicationSettingService.GetSettingEntityAsync("SupportedGenders", false);
            ViewBag.SupportedGenders = supportedGendersSetting.Value;
            ViewBag.AllBranches = _serviceManager.BranchService.GetAllActiveBranches(false).ToList();
            ViewBag.AllAgeGroups = _serviceManager.AgeGroupService.GetAllAgeGroups(false).ToList();
        }

    }
}
