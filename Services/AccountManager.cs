using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Services.Contracts;
using System.ComponentModel.DataAnnotations;

namespace Services
{
    public class AccountManager : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<AccountManager> _localizer;
        private readonly IAuthService _authService;
        private readonly ITenantService _tenantService;

        public AccountManager(
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<AccountManager> localizer,
            IAuthService authService,
            ITenantService tenantService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _localizer = localizer;
            _authService = authService;
            _tenantService = tenantService;
        }

        public async Task CreateUserAsync(UserDtoForInsert userDtoForInsert)
        {
            var validationException = new List<ValidationException>();

            // Get current tenant from HttpContext
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                validationException.Add(new ValidationException(_localizer["NoTenantContextAvailable"],
                    new Exception() { Source = "Tenant" }));
                throw new AggregateException(validationException);
            }

            if (userDtoForInsert.PhoneNumber != null)
            {
                // Pass tenantId to extension methods
                var phoneNumberExists = await _userManager.PhoneNumberExistsAsync(
                    userDtoForInsert.PhoneNumber.NormalizePhoneNumber(),
                    currentTenant.Id);

                var emailExists = await _userManager.EmailExistsAsync(
                    userDtoForInsert.Email,
                    currentTenant.Id);

                if (!phoneNumberExists && !emailExists)
                {
                    var applicationUser = _mapper.Map<ApplicationUser>(userDtoForInsert);
                    applicationUser.UserName = $"{Guid.NewGuid()}-{DateTime.UtcNow.Ticks}";
                    applicationUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(applicationUser, "0000");
                    applicationUser.PhoneNumber = applicationUser.PhoneNumber.NormalizePhoneNumber();
                    applicationUser.PhoneNumberConfirmed = true;
                    applicationUser.IsActive = true;

                    // Set tenant ID for the user
                    applicationUser.TenantId = currentTenant.Id;

                    var result = await _userManager.CreateAsync(applicationUser);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            validationException.Add(new ValidationException(
                                _localizer["UserCreationFailed"] + "." + error.Description,
                                new Exception() { Source = "Model" }));
                        }
                    }
                    else
                    {
                        var roleResult = await _userManager.AddToRoleAsync(applicationUser, "User");
                        if (!roleResult.Succeeded)
                        {
                            foreach (var error in roleResult.Errors)
                            {
                                validationException.Add(new ValidationException(
                                    _localizer["UserRoleCreationFailed"] + "." + error.Description,
                                    new Exception() { Source = "Model" }));
                            }
                        }
                    }
                }
                else
                {
                    validationException.Add(new ValidationException(
                        _localizer["PhoneNumberOrEmailAlreadyExists"] + ".",
                        new Exception() { Source = "Model" }));
                }
            }

            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            var validationException = new List<ValidationException>();

            // Get current tenant from HttpContext
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                validationException.Add(new ValidationException(_localizer["NoTenantContextAvailable"],
                    new Exception() { Source = "Tenant" }));
                throw new AggregateException(validationException);
            }

            if (id.Equals("LaAdmin"))
            {
                validationException.Add(new ValidationException(
                    _localizer["AdminUserCannotBeDeleted"] + ".",
                    new Exception() { Source = "Model" }));
            }
            else
            {
                // Filter by tenant ID for regular users
                var user = await _userManager.Users
                    .FirstOrDefaultAsync(x => x.Id == id && x.TenantId == currentTenant.Id);

                if (user != null)
                {
                    var result = await _userManager.DeleteAsync(user);
                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            validationException.Add(new ValidationException(
                                _localizer["UserDeletionFailed"] + "." + error.Description,
                                new Exception() { Source = "Model" }));
                        }
                    }
                }
                else
                {
                    validationException.Add(new ValidationException(
                        _localizer["UserNotFound"] + ".",
                        new Exception() { Source = "Model" }));
                }
            }

            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }

        public async Task UpdateUserAsync(UserDtoForUpdate userDtoForUpdate)
        {
            var validationException = new List<ValidationException>();

            // Get current tenant from HttpContext
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                validationException.Add(new ValidationException(_localizer["NoTenantContextAvailable"],
                    new Exception() { Source = "Tenant" }));
                throw new AggregateException(validationException);
            }

            if (string.IsNullOrEmpty(userDtoForUpdate.PhoneNumber) || string.IsNullOrEmpty(userDtoForUpdate.Email))
            {
                validationException.Add(new ValidationException(
                    _localizer["PhoneNumberAndEmailIsRequired"] + ".",
                    new Exception() { Source = "Model" }));
            }
            else
            {
                // Pass tenantId to extension methods
                var phoneNumberExists = await _userManager.PhoneNumberExistsAsync(
                    userDtoForUpdate.PhoneNumber.NormalizePhoneNumber(),
                    currentTenant.Id,
                    userDtoForUpdate.UserId);

                var emailExists = await _userManager.EmailExistsAsync(
                    userDtoForUpdate.Email,
                    currentTenant.Id,
                    userDtoForUpdate.UserId);

                if (!phoneNumberExists && !emailExists)
                {
                    // Filter by tenant ID
                    var applicationUser = await _userManager.Users
                        .FirstOrDefaultAsync(u => u.Id == userDtoForUpdate.UserId && u.TenantId == currentTenant.Id);

                    if (applicationUser == null)
                    {
                        validationException.Add(new ValidationException(
                            _localizer["UserNotFound"] + ".",
                            new Exception() { Source = "Model" }));
                    }
                    else
                    {
                        _mapper.Map(userDtoForUpdate, applicationUser);
                        var result = await _userManager.UpdateAsync(applicationUser);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                validationException.Add(new ValidationException(
                                    _localizer["UserUpdateFailed"] + "." + error.Description,
                                    new Exception() { Source = "Model" }));
                            }
                        }
                    }
                }
                else
                {
                    validationException.Add(new ValidationException(
                        _localizer["PhoneNumberOrEmailAlreadyExists"] + ".",
                        new Exception() { Source = "Model" }));
                }
            }

            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
        }
    }
}