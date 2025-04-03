using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Extensions
{
    public static class UserManagerExtension
    {
        public static async Task<ApplicationUser> FindByPhoneAsync(this UserManager<ApplicationUser> userManager, string phoneNumber)
        {
            return await userManager.Users
                .FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber.NormalizePhoneNumber() &&
                                     u.IsActive == true);
        }

        public static async Task<bool> PhoneNumberExistsAsync(this UserManager<ApplicationUser> userManager, string phoneNumber, string userId = null)
        {
            if (userId != null)
            {
                return await userManager.Users
                    .AnyAsync(u => u.PhoneNumber == phoneNumber.NormalizePhoneNumber() && 
                              u.IsActive == true &&
                              u.Id != userId);
            }
            return await userManager.Users
                .AnyAsync(u => u.PhoneNumber == phoneNumber.NormalizePhoneNumber() &&
                              u.IsActive == true);
        }
        public static async Task<bool> EmailExistsAsync(this UserManager<ApplicationUser> userManager, string email, string userId = null)
        {
            if (userId != null)
            {
                return await userManager.Users
                    .AnyAsync(u => u.Email == email &&
                              u.IsActive == true && 
                              u.Id != userId);
            }
            return await userManager.Users
                .AnyAsync(u => u.Email == email &&
                          u.IsActive == true);
        }
    }
}
