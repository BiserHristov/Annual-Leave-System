using AnnualLeaveSystem.Data;
using AnnualLeaveSystem.Data.Models;
using AnnualLeaveSystem.Infrastructure;
using AnnualLeaveSystem.Services.Emails;
using AnnualLeaveSystem.Services.EmployeeLeaveTypes;
using AnnualLeaveSystem.Services.Employees;
using AnnualLeaveSystem.Services.Leaves;
using AnnualLeaveSystem.Services.LeaveTypes;
using AnnualLeaveSystem.Services.Statistics;
using AnnualLeaveSystem.Services.Teams;
using AnnualLeaveSystem.Services.Users;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public Startup(IConfiguration configuration)
        => this.Configuration = configuration;

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        services
            .AddDbContext<LeaveSystemDbContext>(options => options
                .UseSqlServer(this.Configuration.GetConnectionString("DefaultConnection")));

        services.AddDatabaseDeveloperPageExceptionFilter();

        services
            .AddDefaultIdentity<Employee>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<LeaveSystemDbContext>();

        services
            .AddControllersWithViews();
        services.AddTransient<ICommonInfoService, CommonInfoService>();
        services.AddTransient<ILeaveService, LeaveService>();
        services.AddTransient<ILeaveTypeService, LeaveTypeService>();
        services.AddTransient<IUserService, UserService>();
        services.AddTransient<ITeamService, TeamService>();
        services.AddTransient<IEmployeeService, EmployeeService>();
        services.AddTransient<IEmployeeLeaveTypesService, EmployeeLeaveTypesService>();


        services.AddTransient<IEmailSenderService, EmailSenderService>();




    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.PrepareDatabase();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app
            .UseHttpsRedirection()
            .UseStaticFiles()
            .UseRouting()
            .UseAuthentication()
            .UseAuthorization()
            .UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
    }
}