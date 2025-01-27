using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Repositories;
using Repositories.Contracts;
using Services;
using Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.AddMvc()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en-GB", "tr-TR" };
    options.SetDefaultCulture(supportedCultures[1])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

builder.Services.AddDbContext<RepositoryContext>(options =>
{
    /*
    options.UseSqlite(builder.Configuration.GetConnectionString("sqlconnection"),
                      b => b.MigrationsAssembly("LudusAppoint"));
    */
    options.UseSqlServer(builder.Configuration.GetConnectionString("mssqlconnection"),
                      b => b.MigrationsAssembly("LudusAppoint"));
});

builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IAgeGroupRepository, AgeGroupRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IOfferedServiceRepository, OfferedServiceRepository>();
builder.Services.AddScoped<ICustomerAppointmentRepository, CustomerAppointmentRepository>();
builder.Services.AddScoped<IBranchRepository, BranchRepository>();
//builder.Services.AddScoped<IShopSettingsRepository, ShopSettingsRepository>();
builder.Services.AddScoped<IEmployeeLeaveRepository, EmployeeLeaveRepository>();

builder.Services.AddScoped<IServiceManager, ServiceManager>();
builder.Services.AddScoped<IAgeGroupService, AgeGroupManager>();
builder.Services.AddScoped<IEmployeeService, EmployeeManager>();
builder.Services.AddScoped<IOfferedServiceService, OfferedServiceManager>();
builder.Services.AddScoped<ICustomerAppointmentService, CustomerAppointmentManager>();
builder.Services.AddScoped<IBranchService, BranchManager>();
//builder.Services.AddScoped<IShopSettingsService, ShopSettingsManager>();
builder.Services.AddScoped<IEmployeeLeaveService, EmployeeLeaveManager>();

builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.UseRequestLocalization();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();
app.UseRequestLocalization();

app.MapAreaControllerRoute(
    name: "Admin",
    areaName: "Admin",
    pattern: "Admin/{controller=Dashboard}/{action=Index}/{id?}"
);

app.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");

app.Run();
