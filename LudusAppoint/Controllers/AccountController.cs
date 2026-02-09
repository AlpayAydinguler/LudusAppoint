using Entities.Dtos;
using LudusAppoint.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Services.Contracts;

namespace LudusAppoint.Controllers
{
    public class AccountController : Controller
    {
        private readonly IStringLocalizer<AccountController> _localizer;
        private readonly IServiceManager _serviceManager;
        private readonly IAuthService _authService;

        public AccountController(IStringLocalizer<AccountController> localizer, IServiceManager serviceManager, IAuthService authService)
        {
            _localizer = localizer;
            _serviceManager = serviceManager;
            _authService = authService;
        }

        public IActionResult Login([FromQuery(Name = "ReturnUrl")] string returnUrl = "/")
        {
            return View(new LoginModel()
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var userStatusResult = await _serviceManager.UserService.IsUserActiveAsync(model.PhoneNumber);
                if (userStatusResult == "Inactive")
                {
                    // Redirect to email OTP page
                }
                else if (userStatusResult == "NotFound")
                {
                    ModelState.AddModelError(string.Empty, _localizer["InvalidPhoneNumberOrPassword"]);
                    return View(model);
                }
                else if (userStatusResult == "Active")
                {
                    await _serviceManager.AuthService.LogoutAsync();
                    if (await _serviceManager.AuthService.LoginAsync(model.PhoneNumber, model.OneTimePassword))
                    {
                        return Redirect(model?.ReturnUrl ?? "/");
                    }
                    ModelState.AddModelError(string.Empty, _localizer["FailedToLogin"]);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Logout([FromQuery(Name = "ReturnUrl")] string returnUrl = "/")
        {
            await _serviceManager.AuthService.LogoutAsync();
            return Redirect(returnUrl);
        }

        public IActionResult Register()
        {
            return View(new UserDtoForInsert());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromForm] UserDtoForInsert model, [FromForm(Name = "ReturnUrl")] string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            try
            {
                await _serviceManager.AccountService.CreateUserAsync(model);
                if (!User.Identity.IsAuthenticated)
                {
                    await _authService.LoginAsync(model.PhoneNumber, "0000");
                }
                return Redirect(returnUrl);
            }
            catch (AggregateException exceptions)
            {
                foreach (var exception in exceptions.InnerExceptions)
                {
                    ModelState.AddModelError(exception?.InnerException?.Source?.ToString() ?? string.Empty, exception?.Message ?? string.Empty);
                }
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GenerateDummyOTP(string phoneNumber)
        {
            // In production: Replace with real SMS service integration
            var dummyOTP = new Random().Next(1000, 9999).ToString();
            TempData["OTP"] = dummyOTP;
            TempData["OTPPhone"] = phoneNumber;
            return Content(dummyOTP);
        }
    }
}
