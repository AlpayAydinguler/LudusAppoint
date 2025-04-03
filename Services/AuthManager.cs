using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Repositories.Contracts;
using Services.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthManager : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<AuthManager> _localizer;

        public AuthManager(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper, IStringLocalizer<AuthManager> localizer)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _localizer = localizer;
        }

        public async Task CreateRoleAsync(RoleDtoForInsert roleDtoForInsert)
        {
            var validationException = new List<ValidationException>();
            var role = _mapper.Map<IdentityRole>(roleDtoForInsert);

            role.Id = await GetNextRoleIdAsync();
            var createResult = await _roleManager.CreateAsync(role);
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    validationException.Add(new ValidationException(
                        _localizer["FailedToCreateRole"] + error.Description,
                        new Exception() { Source = "Model" }));
                }
                throw new AggregateException(validationException);
            }

            var (permissionsSuccess, addedPermissions) = await AddPermissionsToRoleAsync(role, roleDtoForInsert.Permissions);
            if (!permissionsSuccess)
            {
                foreach (var permission in addedPermissions)
                {
                    await _roleManager.RemoveClaimAsync(role, new Claim("permission", permission));
                }
                await _roleManager.DeleteAsync(role);
                throw new AggregateException(validationException);
            }
        }

        public async Task DeleteRoleAsync(string id)
        {
            var validationException = new List<ValidationException>();
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                validationException.Add(new ValidationException(
                    _localizer["RoleNotFound"],
                    new Exception() { Source = "Model" }));
                throw new AggregateException(validationException);
            }
            if (role.Name == "Admin")
            {
                validationException.Add(new ValidationException(
                    _localizer["AdminRoleCannotBeDeleted"],
                    new Exception() { Source = "Model" }));
                throw new AggregateException(validationException);
            }
            var deleteResult = await _roleManager.DeleteAsync(role);
            if (!deleteResult.Succeeded)
            {
                foreach (var error in deleteResult.Errors)
                {
                    validationException.Add(new ValidationException(
                        _localizer["FailedToDeleteRole"] + error.Description,
                        new Exception() { Source = "Model" }));
                }
            }
            if (validationException.Count > 0)
            {
                throw new AggregateException(validationException);
            }
        }

        public async Task<RoleDtoForUpdate> GetRoleByIdAsync(string id)
        {
            var role = await _roleManager.Roles.AsNoTracking().FirstOrDefaultAsync(r => r.Id == id);
            if (role == null) return null;

            var roleClaims = await _roleManager.GetClaimsAsync(role);
            var roleDto = _mapper.Map<RoleDtoForUpdate>(role);
            roleDto = roleDto with { Permissions = roleClaims.Where(c => c.Type == "permission").Select(c => c.Value).ToList() };

            return roleDto;
        }

        public async Task<IEnumerable<RoleDto>> GetRolesAsync()
        {
            var roles = await _roleManager.Roles
                .AsNoTracking()
                .ToListAsync();  // Execute query asynchronously

            return _mapper.Map<IEnumerable<RoleDto>>(roles);
        }

        public async Task<bool> LoginAsync(string phoneNumber, string password)
        {
            var user = await _userManager.FindByPhoneAsync(phoneNumber);
            if (user == null) return false;

            // Sign in user
            var result = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!result.Succeeded) return false;

            // Get user roles
            var roles = await _userManager.GetRolesAsync(user);

            // Collect permissions from all roles
            var claims = new List<Claim>();
            foreach (var roleName in roles)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await _roleManager.GetClaimsAsync(role);
                    claims.AddRange(roleClaims.Where(c => c.Type == "permission"));
                }
            }

            // Remove existing permissions to avoid duplicates
            var existingClaims = await _userManager.GetClaimsAsync(user);
            await _userManager.RemoveClaimsAsync(user, existingClaims.Where(c => c.Type == "permission"));

            // Add new permissions
            await _userManager.AddClaimsAsync(user, claims);

            // Refresh authentication cookie
            await _signInManager.RefreshSignInAsync(user);

            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            return true;
        }

        public async Task<bool> LogoutAsync()
        {
            await _signInManager.SignOutAsync();
            return true;
        }

        public async Task UpdateRoleAsync(RoleDtoForUpdate roleDtoForUpdate)
        {
            var validationException = new List<ValidationException>();
            var role = await _roleManager.FindByIdAsync(roleDtoForUpdate.RoleId);

            if (role == null)
            {
                validationException.Add(new ValidationException(
                    _localizer["RoleNotFound"],
                    new Exception() { Source = "Model" }));
                throw new AggregateException(validationException);
            }
            if(role.Name == "Admin")
            {
                validationException.Add(new ValidationException(
                    _localizer["AdminRoleCannotBeUpdated"],
                    new Exception() { Source = "Model" }));
                throw new AggregateException(validationException);
            }

            // Update role properties
            role.Name = roleDtoForUpdate.RoleName;
            var updateResult = await _roleManager.UpdateAsync(role);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    validationException.Add(new ValidationException(
                        _localizer["FailedToUpdateRole"] + error.Description,
                        new Exception() { Source = "Model" }));
                }
                throw new AggregateException(validationException);
            }

            // Remove existing permissions
            var existingClaims = await _roleManager.GetClaimsAsync(role);
            foreach (var claim in existingClaims.Where(c => c.Type == "permission"))
            {
                var removeResult = await _roleManager.RemoveClaimAsync(role, claim);
                if (!removeResult.Succeeded)
                {
                    foreach (var error in removeResult.Errors)
                    {
                        validationException.Add(new ValidationException(
                            _localizer["FailedToRemovePermission"] + claim.Value + ": " + error.Description,
                            new Exception() { Source = "Model" }));
                    }
                }
            }

            if (validationException.Any()) throw new AggregateException(validationException);

            // Add new permissions
            var (permissionsSuccess, addedPermissions) = await AddPermissionsToRoleAsync(role, roleDtoForUpdate.Permissions);
            if (!permissionsSuccess)
            {
                foreach (var permission in addedPermissions)
                {
                    await _roleManager.RemoveClaimAsync(role, new Claim("permission", permission));
                }
                throw new AggregateException(validationException);
            }
        }

        private async Task<(bool Success, List<string> AddedPermissions)> AddPermissionsToRoleAsync(IdentityRole role, List<string> permissions)
        {
            var addedPermissions = new List<string>();
            var validationExceptions = new List<ValidationException>();

            foreach (var permission in permissions)
            {
                var result = await _roleManager.AddClaimAsync(role, new Claim("permission", permission));
                if (result.Succeeded)
                {
                    addedPermissions.Add(permission);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        validationExceptions.Add(new ValidationException(
                            _localizer["FailedToAddPermission"] + permission + ": " + error.Description,
                            new Exception() { Source = "Model" }));
                    }
                    return (false, addedPermissions);
                }
            }
            return (true, addedPermissions);
        }

        private async Task<string> GetNextRoleIdAsync()
        {
            var roles = await _roleManager.Roles.AsNoTracking().ToListAsync();
            var nextId = roles.Count > 0 ? roles.Max(r => int.Parse(r.Id)) + 1 : 1;
            return nextId.ToString();
        }
    }
}
