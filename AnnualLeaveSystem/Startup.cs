using AnnualLeaveSystem.Data;
using AnnualLeaveSystem.Data.Models;
using AnnualLeaveSystem.Infrastructure;
using AnnualLeaveSystem.Services;
using AnnualLeaveSystem.Services.Leaves;
using AnnualLeaveSystem.Services.Statistics;
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
            .AddEntityFrameworkStores<LeaveSystemDbContext>();

        services
            .AddControllersWithViews();
        services.AddTransient<IGetLeaveTypesService, GetLeaveTypesService>();
        services.AddTransient<IGetEmployeesInTeamService, GetEmployeesInTeamService>();
        services.AddTransient<IGetOfficialHolidaysService, GetOfficialHolidaysService>();
        services.AddTransient<IStatisticsService, StatisticsService>();
        services.AddTransient<ILeaveService, LeaveService>();



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