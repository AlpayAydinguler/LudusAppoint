using AutoMapper;
using Entities.Dtos;
using Entities.Models;
using Infrastructure.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Services.Contracts;

namespace Services
{
    public class ApplicationUserManager : IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ITenantService _tenantService; // Added

        public ApplicationUserManager(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            ITenantService tenantService) // Added
        {
            _userManager = userManager;
            _mapper = mapper;
            _tenantService = tenantService;
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync()
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return Enumerable.Empty<UserDto>();
            }

            // Get users for current tenant only
            var users = await _userManager.Users
                .Where(u => u.TenantId == currentTenant.Id) // Tenant filter
                .AsNoTracking()
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserDto>>(users);
        }

        public async Task<UserDtoForUpdate> GetUserForUpdateAsync(string id)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return null;
            }

            // Get user by ID for current tenant only
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Id == id && u.TenantId == currentTenant.Id);

            return _mapper.Map<UserDtoForUpdate>(user);
        }

        public async Task<string> IsUserActiveAsync(string phoneNumber)
        {
            var currentTenant = await _tenantService.GetCurrentTenantAsync();
            if (currentTenant == null)
            {
                return "NoTenant";
            }

            var normalizedPhone = phoneNumber.NormalizePhoneNumber();

            // Find user by phone number for current tenant only
            var user = await _userManager.Users
                .Where(u => u.PhoneNumber == normalizedPhone && u.TenantId == currentTenant.Id)
                .Select(u => new
                {
                    u.PhoneNumber,
                    u.IsActive,
                    u.LastLogin
                })
                .OrderByDescending(u => u.LastLogin)
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return "NotFound";
            }

            return user.IsActive ? "Active" : "Inactive";
        }
    }
}