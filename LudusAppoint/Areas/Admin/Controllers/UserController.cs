using Entities.Dtos;
using LudusAppoint.Models.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Services.Contracts;

namespace LudusAppoint.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IServiceManager _serviceManager;
        private readonly IStringLocalizer<UserController> _localizer;

        public UserController(IServiceManager serviceManager, IStringLocalizer<UserController> localizer)
        {
            _serviceManager = serviceManager;
            _localizer = localizer;
        }

        [Authorize(Policy = nameof(Permissions.User_Index))]
        public async Task<IActionResult> Index()
        {
            var model = await _serviceManager.UserService.GetAllUsersAsync();
            return View(model);
        }

        [Authorize(Policy = nameof(Permissions.User_Create))]
        public IActionResult Create()
        {
            var model = new UserDtoForInsert();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(Permissions.User_Create))]
        public async Task<IActionResult> Create(UserDtoForInsert userDtoForInsert)
        {
            if (!ModelState.IsValid)
            {
                return View(userDtoForInsert);
            }
            try
            {
                await _serviceManager.AccountService.CreateUserAsync(userDtoForInsert);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["UserCreatedSuccessfully"].ToString() + ".";
                return RedirectToAction(nameof(Index));
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = _localizer["UserCreationFailed"].ToString() + ".";
                return View(userDtoForInsert);
            }
        }

        [Authorize(Policy = nameof(Permissions.User_Update))]
        public async Task<IActionResult> Update([FromRoute] string id)
        {
            var model = await _serviceManager.UserService.GetUserForUpdateAsync(id);
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(Permissions.User_Update))]
        public async Task<IActionResult> Update(UserDtoForUpdate userDtoForUpdate)
        {
            if (userDtoForUpdate.UserId.Equals("LaAdmin"))
            {
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = _localizer["AdminUserCannotBeDeleted"].ToString() + ".";
                return RedirectToAction("Index");
            }
            if (!ModelState.IsValid)
            {
                return View(userDtoForUpdate);
            }
            try
            {
                await _serviceManager.AccountService.UpdateUserAsync(userDtoForUpdate);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["UserUpdatedSuccessfully"].ToString() + ".";
                return RedirectToAction(nameof(Index));
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = _localizer["UserUpdateFailed"].ToString() + ".";
                return View(userDtoForUpdate);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = nameof(Permissions.User_Delete))]
        public async Task<IActionResult> Delete([FromForm] string id)
        {
            if (id.Equals("LaAdmin"))
            {
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = _localizer["AdminUserCannotBeDeleted"].ToString() + ".";
                return RedirectToAction("Index");
            }
            try
            {
                await _serviceManager.AccountService.DeleteUserAsync(id);
                TempData["OperationSuccessfull"] = true;
                TempData["OperationMessage"] = _localizer["UserDeletedSuccessfully"].ToString() + ".";
                return RedirectToAction(nameof(Index));
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                TempData["OperationSuccessfull"] = false;
                TempData["OperationMessage"] = _localizer["UserDeletionFailed"].ToString() + ".";
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
