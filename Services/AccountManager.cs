using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AccountManager : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<AccountManager> _localizer;
        private readonly IAuthService _authService;

        public AccountManager(IMapper mapper, UserManager<ApplicationUser> userManager, IStringLocalizer<AccountManager> localizer, IAuthService authService)
        {
            _mapper = mapper;
            _userManager = userManager;
            _localizer = localizer;
            _authService = authService;
        }

        public async Task CreateUserAsync(UserDtoForInsert userDtoForInsert)
        {
            var validationException = new List<ValidationException>();
            if (userDtoForInsert.PhoneNumber != null)
            {
                var phoneNumberExists = await _userManager.PhoneNumberExistsAsync(userDtoForInsert.PhoneNumber.NormalizePhoneNumber());
                var emailExists = await _userManager.EmailExistsAsync(userDtoForInsert.Email);
                if (!phoneNumberExists && !emailExists)
                {
                    var applicationUser = _mapper.Map<ApplicationUser>(userDtoForInsert);
                    applicationUser.UserName = $"{Guid.NewGuid()}-{DateTime.UtcNow.Ticks}";
                    applicationUser.PasswordHash = new PasswordHasher<ApplicationUser>().HashPassword(applicationUser, "0000");
                    applicationUser.PhoneNumber = applicationUser.PhoneNumber.NormalizePhoneNumber();
                    applicationUser.PhoneNumberConfirmed = true;
                    applicationUser.IsActive = true;
                    var result = await _userManager.CreateAsync(applicationUser);

                    if (!result.Succeeded)
                    {
                        foreach (var error in result.Errors)
                        {
                            validationException.Add(new ValidationException(_localizer["UserCreationFailed"] + "." + error.Description,
                                                                            new Exception() { Source = "Model" }));
                        }
                    }
                    else
                    {
                        var roleResult = await _userManager.AddToRoleAsync(applicationUser, "User");
                        if (!roleResult.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                validationException.Add(new ValidationException(_localizer["UserRoleCreationFailed"] + "." + error.Description,
                                                                                new Exception() { Source = "Model" }));
                            }
                        }
                    }
                }
                else
                {
                    validationException.Add(new ValidationException(_localizer["PhoneNumberOrEmailAlreadyExists"] + ".",
                                                                    new Exception() { Source = "Model" }));
                }
            }

            if (validationException.Count != 0)
            { 
                throw new AggregateException(validationException);
            }
        }

        public Task DeleteUserAsync(string id)
        {
            var validationException = new List<ValidationException>();
            if (id.Equals("LaAdmin"))
            {
                validationException.Add(new ValidationException(_localizer["AdminUserCannotBeDeleted"] + ".",
                                                                new Exception() { Source = "Model" }));
            }
            else
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == id);
                if (user != null)
                {
                    var result = _userManager.DeleteAsync(user);
                    if (!result.Result.Succeeded)
                    {
                        foreach (var error in result.Result.Errors)
                        {
                            validationException.Add(new ValidationException(_localizer["UserDeletionFailed"] + "." + error.Description,
                                                                            new Exception() { Source = "Model" }));
                        }
                    }
                }
            }
            if (validationException.Count != 0)
            {
                throw new AggregateException(validationException);
            }
            return Task.CompletedTask;
        }

        public async Task UpdateUserAsync(UserDtoForUpdate userDtoForUpdate)
        {
            var validationException = new List<ValidationException>();
            if (userDtoForUpdate.PhoneNumber.IsNullOrEmpty() || userDtoForUpdate.Email.IsNullOrEmpty() )
            {
                validationException.Add(new ValidationException(_localizer["PhoneNumberAndEmailIsRequired"] + ".",
                                                                new Exception() { Source = "Model" }));
            }
            else
            {
                var phoneNumberExists = await _userManager.PhoneNumberExistsAsync(userDtoForUpdate.PhoneNumber.NormalizePhoneNumber(), userDtoForUpdate.UserId);
                var emailExists = await _userManager.EmailExistsAsync(userDtoForUpdate.Email, userDtoForUpdate.UserId);
                if (!phoneNumberExists && !emailExists)
                {
                    var applicationUser = await _userManager.Users.FirstOrDefaultAsync(_userManager => _userManager.Id == userDtoForUpdate.UserId);
                    _mapper.Map(userDtoForUpdate, applicationUser);
                    var result = _userManager.UpdateAsync(applicationUser);
                    if (!result.Result.Succeeded)
                    {
                        foreach (var error in result.Result.Errors)
                        {
                            validationException.Add(new ValidationException(_localizer["UserUpdateFailed"] + "." + error.Description,
                                                                            new Exception() { Source = "Model" }));
                        }
                    }
                }
                else
                {
                    validationException.Add(new ValidationException(_localizer["PhoneNumberOrEmailAlreadyExists"] + ".",
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
