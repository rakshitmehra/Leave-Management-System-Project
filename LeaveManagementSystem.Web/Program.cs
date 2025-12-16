using LeaveManagementSystem.Application;
using Microsoft.EntityFrameworkCore;
using Serilog;

// This file sets up all the services the app needs, connects to the database,
// Configures identity and roles, registers our custom services,
// Adds AutoMapper, and builds the request pipeline for the web application.

var builder = WebApplication.CreateBuilder(args);

DataServicesRegisteration.AddDataServices(builder.Services, builder.Configuration);

ApplicationServicesRegisteration.AddApplicationServices(builder.Services);

builder.Host.UseSerilog((ctx, config) =>
    config.WriteTo.Console()
    .ReadFrom.Configuration(ctx.Configuration)
);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminSupervisorOnly", policy =>
    {
        policy.RequireRole(Roles.Administrator, Roles.Supervisor);
    });
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
