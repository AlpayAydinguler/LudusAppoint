using Entities.Models;
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

        public IActionResult Index()
        {
            var model = _serviceManager.EmployeeService.GetAllEmployees(false);
            return View(model);
        }
        public IActionResult Update([FromRoute] int id)
        {
            var model = _serviceManager.EmployeeService.GetOneEmployee(id, false, System.Globalization.CultureInfo.CurrentCulture.Name);
            PopulatePageData();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update([FromForm] Employee employee, int[] offeredServiceIds)
        {
            if (!ModelState.IsValid)
            {
                PopulatePageData();
                // Manually populate the OfferedServices collection

                employee.OfferedServices = new List<OfferedService>();
                foreach (var serviceId in offeredServiceIds)
                {
                    var service = _serviceManager.OfferedServiceService.GetofferedService(serviceId, false);
                    if (service != null)
                    {
                        employee.OfferedServices.Add(service);
                    }
                }

                return View(employee);
            }
            _serviceManager.EmployeeService.UpdateEmployee(employee, offeredServiceIds);
            return RedirectToAction("Index");
        }
        public IActionResult Create()
        {
            var model = new Employee();
            PopulatePageData();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create([FromForm] Employee employee, int[] offeredServiceIds)
        {
            Console.WriteLine("Create Employee");
            employee.OfferedServices = new List<OfferedService>();

            foreach (var serviceId in offeredServiceIds)
            {
                var service = _serviceManager.OfferedServiceService.GetofferedService(serviceId, true);
                if (service != null)
                {
                    employee.OfferedServices.Add(service);
                }
            }

            if (!ModelState.IsValid)
            {
                PopulatePageData();
                return View(employee);
            }

            _serviceManager.EmployeeService.CreateEmployee(employee);
            return RedirectToAction("Index");
        }

        private void PopulatePageData()
        {
            var offeredServices = _serviceManager.OfferedServiceService.GetActiveOfferedServices(false, System.Globalization.CultureInfo.CurrentCulture.Name);
            ViewBag.AllOfferedServices = offeredServices;
            ViewBag.AllBranches = _serviceManager.BranchService.GetAllBranches(false).ToList();
        }
    }
}
