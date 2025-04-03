using Entities.Models;
using LudusAppoint.Models;
using LudusAppoint.Models.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System.Security.Claims;

namespace LudusAppoint.Infrastructure.Extensions
{
    public static class ApplicationExtension
    {
        public static void ConfigureAndCheckMigration(this IApplicationBuilder app)
        {
            RepositoryContext context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RepositoryContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }

        public static async void ConfigureDefaultAdminUser(this IApplicationBuilder app)
        {
            const string adminUser = "LaAdmin";
            const string adminPassword = "LaAdminA8539lp7212i,;";

            // UserManager
            UserManager<ApplicationUser> userManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // RoleManager
            RoleManager<IdentityRole> roleManager = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            ApplicationUser user = await userManager.FindByNameAsync(adminUser);
            if (user == null)
            {
                user = new ApplicationUser()
                {
                    UserName = adminUser,
                    Email = "alpayaydinguler@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumber = "+905395166223",
                    PhoneNumberConfirmed = true,
                    Name = "Alpay",
                    Surname = "Aydinguler"
                };
                var result = await userManager.CreateAsync(user, adminPassword);
                if (!result.Succeeded)
                {
                    throw new Exception("Admin user could not be created.");
                }

                var roleResult = await userManager.AddToRolesAsync(user, roleManager.Roles
                                                                                   .Select(r => r.Name)
                                                                                   .ToList());
                if (!roleResult.Succeeded)
                {
                    throw new Exception("Admin user could not be added to roles.");
                }
            }
            var adminRole = roleManager.Roles.FirstOrDefault(r => r.Name == "Admin");
            if (adminRole != null)
            {
                foreach (var permission in Enum.GetValues(typeof(Permissions)))
                {
                    var claims = await roleManager.GetClaimsAsync(adminRole);
                    var claimExists = claims.Any(c => c.Type == "permission" && c.Value == permission.ToString());

                    if (!claimExists)
                    {
                        await roleManager.AddClaimAsync(adminRole, new Claim("permission", permission.ToString()));
                    }
                }
            }
        }
    }
}
