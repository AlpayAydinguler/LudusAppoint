using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Infrastructure.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<ApplicationUser> FindByPhoneAsync(this UserManager<ApplicationUser> userManager,
                                                                   string phoneNumber,
                                                                   Guid tenantId)

        {
            return await userManager.Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber.NormalizePhoneNumber() &&
                                                                    u.TenantId == tenantId &&
                                                                    u.IsActive == true);


        }

        public static async Task<bool> PhoneNumberExistsAsync(this UserManager<ApplicationUser> userManager,
                                                              string phoneNumber,
                                                              Guid tenantId,
                                                              string userId = null)

        {
            var normalizedPhone = phoneNumber.NormalizePhoneNumber();
            var query = userManager.Users
                .Where(u => u.PhoneNumber == normalizedPhone &&
                            u.TenantId == tenantId &&
                            u.IsActive == true);


            if (userId != null)
            {
                query = query.Where(u => u.Id != userId);
            }

            return await query.AnyAsync();
        }

        public static async Task<bool> EmailExistsAsync(this UserManager<ApplicationUser> userManager,
                                                        string email,
                                                        Guid tenantId,
                                                        string userId = null)

        {
            var query = userManager.Users
                .Where(u => u.Email == email &&
                            u.TenantId == tenantId &&
                            u.IsActive == true);


            if (userId != null)
            {
                query = query.Where(u => u.Id != userId);
            }

            return await query.AnyAsync();
        }
    }
}