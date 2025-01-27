using Entities.Dtos;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace LudusAppoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BranchController : Controller
    {
        private readonly IServiceManager _serviceManager;

        public BranchController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }
        public IActionResult Index()
        {
            var model = _serviceManager.BranchService.GetAllBranches(false);
            return View(model);
        }

        public IActionResult Create()
        {
            var model = new BranchDtoForInsert();
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create([FromForm] Branch branch)
        {
            if (!ModelState.IsValid)
            {
                return View(branch);
            }
            try
            {
                _serviceManager.BranchService.CreateBranch(branch);
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception.InnerException?.Source.ToString(), exception.Message);
                }
                return View(branch);
            }
        }

        public IActionResult Update([FromRoute] int id)
        {
            var model = _serviceManager.BranchService.GetBranch(id, false);
            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update([FromForm] Branch branch)
        {
            if (!ModelState.IsValid)
            {
                return View(branch);
            }
            try
            {
                _serviceManager.BranchService.UpdateBranch(branch);
                return RedirectToAction("Index");
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception.InnerException?.Source.ToString(), exception.Message);
                }
                return View(branch);
            }
        }
    }
}
