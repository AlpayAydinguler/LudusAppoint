using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Services.Contracts;

namespace LudusAppoint.Controllers
{
    public class CustomerAppointmentController : Controller
    {
        private readonly IStringLocalizer<CustomerAppointment> _localizer;
        private readonly IServiceManager _serviceManager;

        public CustomerAppointmentController(IStringLocalizer<CustomerAppointment> localizer, IServiceManager serviceManager)
        {
            _localizer = localizer;
            _serviceManager = serviceManager;
        }
        public IActionResult Index()
        {
            ViewBag.AgeGroups = new SelectList(_serviceManager.AgeGroupService.GetAllAgeGroups(false),
                                               "Id",
                                               "Name", 2);


            return View();
        }
        public JsonResult GetReservedTimes(string date)
        {
            // Mock reserved times for the selected date
            var reservedTimes = new[]
            {
                new { StartTime = "14:00", EndTime = "14:40" },
                new { StartTime = "16:20", EndTime = "16:40" }
            };

            return Json(reservedTimes);
        }

        // Get calendar days with availability (mocked data)
        public JsonResult GetCalendarDays()
        {
            var today = DateTime.Today;
            var days = Enumerable.Range(0, 31).Select(offset => new
            {
                Date = today.AddDays(offset), // Send DateTime object instead of formatted string
                IsAvailable = offset % 2 == 0 // Mock availability: every second day is available
            });

            return Json(days);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Book([FromForm] CustomerAppointment customerAppointment)
        {
            return View();
        }
    }
}
