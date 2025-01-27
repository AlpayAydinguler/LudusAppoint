using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services.Contracts;

namespace LudusAppoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class EmployeeLeaveController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public EmployeeLeaveController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        public IActionResult Index()
        {
            var model = _serviceManager.EmployeeLeaveService.GetAllEmployeeLeaves(false);
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new EmployeeLeave();
            PopulateViewBag();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([FromForm] EmployeeLeave model)
        {
            if (!ModelState.IsValid)
            {
                PopulateViewBag();
                return View(model);
            }
            try
            {
                _serviceManager.EmployeeLeaveService.CreateEmployeeLeave(model);
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                PopulateViewBag();
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception.InnerException?.Source.ToString(), exception.Message);
                }
                return View(model);
            }
        }

        private void PopulateViewBag()
        {
            ViewBag.Employees = new SelectList(_serviceManager.EmployeeService.GetAllEmployees(false), "Id", "FullName");
        }
    }
}
