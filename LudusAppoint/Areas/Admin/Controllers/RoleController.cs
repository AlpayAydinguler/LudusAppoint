using Entities.Dtos;
using LudusAppoint.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Services.Contracts;

namespace LudusAppoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly IStringLocalizer<RoleController> _localizer;

        public RoleController(IServiceManager serviceManager, IStringLocalizer<RoleController> localizer)
        {
            _serviceManager = serviceManager;
            _localizer = localizer;
        }
        [Authorize(Policy = nameof(Permissions.Role_Index))]
        public async Task<IActionResult> Index()
        {
            var model = await _serviceManager.AuthService.GetRolesAsync();
            return View(model);
        }

        [Authorize(Policy = nameof(Permissions.Role_Create))]
        public async Task<IActionResult> Create()
        {
            var model = new RoleDtoForInsert();
            PopulatePageData();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(Permissions.Role_Create))]
        public async Task<IActionResult> Create(RoleDtoForInsert roleDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                PopulatePageData();
                return View(roleDtoForInsert);
            }
            try
            {
                await _serviceManager.AuthService.CreateRoleAsync(roleDtoForInsert);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["RoleCreatedSuccessfully"].ToString() + ".";
                return RedirectToAction(nameof(Index));
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                PopulatePageData();
                return View(roleDtoForInsert);
            }
        }

        [Authorize(Policy = nameof(Permissions.Role_Update))]
        public async Task<IActionResult> Update([FromRoute] string id)
        {
            if (id.Equals("Admin"))
            {
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = _localizer["AdminRoleCannotBeUpdated"].ToString() + ".";
                return RedirectToAction("Index");
            }
            var model = await _serviceManager.AuthService.GetRoleByIdAsync(id);
            PopulatePageData();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(Permissions.Role_Update))]
        public async Task<IActionResult> Update(RoleDtoForUpdate roleDtoForUpdate)
        {
            if (!ModelState.IsValid)
            {
                PopulatePageData();
                return View(roleDtoForUpdate);
            }
            try
            {
                if (roleDtoForUpdate.RoleName.Equals("Admin"))
                {
                    TempData["OperationSuccessfull"] = false;
                    TempData["OperationMessage"] = _localizer["AdminRoleCannotBeUpdated"].ToString() + ".";
                    return RedirectToAction("Index");
                }
                await _serviceManager.AuthService.UpdateRoleAsync(roleDtoForUpdate);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["RoleUpdatedSuccessfully"].ToString() + ".";
                return RedirectToAction(nameof(Index));
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(roleDtoForUpdate);
            }

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(Permissions.Role_Delete))]
        public async Task<IActionResult> Delete([FromForm] string id)
        {
            try
            {
                if (id.Equals("Admin"))
                {
                    TempData["OperationSuccessfull"] = false;
                    TempData["OperationMessage"] = _localizer["AdminRoleCannotBeUpdated"].ToString() + ".";
                    return RedirectToAction("Index");
                }
                await _serviceManager.AuthService.DeleteRoleAsync(id);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["RoleDeletedSuccessfully"].ToString() + ".";
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
            ViewBag.AllPermissions = new SelectList(
                Enum.GetValues(typeof(Permissions))
                    .Cast<Permissions>()
                    .Select(p => new
                    {
                        Value = p.ToString(),
                        Text = p.ToString()
                    }),
                "Value",
                "Text");
        }
    }
}
