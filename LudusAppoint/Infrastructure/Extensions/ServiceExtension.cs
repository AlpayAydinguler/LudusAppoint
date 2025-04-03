using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using Services.Contracts;
using Services;
using LudusAppoint.Dtos;
using Microsoft.AspNetCore.Identity;
using LudusAppoint.Models.Enums;
using Entities.Models;

namespace LudusAppoint.Infrastructure.Extensions
{
    public static class ServiceExtension
    {
        public static void ConfigureDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RepositoryContext>(options =>
            {
                options.UseSqlServer(
                    configuration.GetConnectionString("mssqlconnection"),
                    sqlServerOptions =>
                    {
                        sqlServerOptions
                            .EnableRetryOnFailure(
                                maxRetryCount: 5,
                                maxRetryDelay: TimeSpan.FromSeconds(30),
                                errorNumbersToAdd: null)
                            .MigrationsAssembly("LudusAppoint");
                    });
            });
        }

        public static void ConfigureIdentity(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedPhoneNumber = true;
            }).AddEntityFrameworkStores<RepositoryContext>();

            services.AddAuthorization(options =>
            {
                foreach (Permissions permission in Enum.GetValues(typeof(Permissions)))
                {
                    options.AddPolicy(permission.ToString(), policy =>
                        policy.RequireClaim("permission", permission.ToString()));
                }
            });
        }

        public static void ConfigureSession(this IServiceCollection services)
        {
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromDays(32);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.Name = "LudusAppoint.Session";
            });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<SessionCustomerAppointmentDtoForInsert>();
        }

        public static void ConfigureRepositoryRegistration(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
            services.AddScoped<IAgeGroupRepository, AgeGroupRepository>();
            services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            services.AddScoped<IOfferedServiceRepository, OfferedServiceRepository>();
            services.AddScoped<ICustomerAppointmentRepository, CustomerAppointmentRepository>();
            services.AddScoped<IBranchRepository, BranchRepository>();
            services.AddScoped<IEmployeeLeaveRepository, EmployeeLeaveRepository>();
            services.AddScoped<IApplicationSettingRepository, ApplicationSettingRepository>();
        }

        public static void ConfigureServiceRegistration(this IServiceCollection services)
        {
            services.AddScoped<IServiceManager, ServiceManager>();
            services.AddScoped<IAgeGroupService, AgeGroupManager>();
            services.AddScoped<IEmployeeService, EmployeeManager>();
            services.AddScoped<IOfferedServiceService, OfferedServiceManager>();
            services.AddScoped<ICustomerAppointmentService, CustomerAppointmentManager>();
            services.AddScoped<IBranchService, BranchManager>();
            services.AddScoped<IEmployeeLeaveService, EmployeeLeaveManager>();
            services.AddScoped<IApplicationSettingService, ApplicationSettingManager>();
            services.AddScoped<IAccountService, AccountManager>();
            services.AddScoped<IAuthService, AuthManager>();
            services.AddScoped<IUserService, UserManager>();
        }
    }
}
