using AnnualLeaveSystem.Areas.Admin;

namespace AnnualLeaveSystem.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;


    using static AdminConstants;


    using static WebConstants;


    public static class ApplicationBuilderExtensions
    {

        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();
            var serviceProvider = scopedServices.ServiceProvider;

            var data = serviceProvider.GetRequiredService<LeaveSystemDbContext>();

            data.Database.Migrate();

            SeedProjects(serviceProvider);
            SeedTeams(serviceProvider);
            SeedLeaveTypes(serviceProvider);
            SeedDepartaments(serviceProvider);
            SeedEmployees(serviceProvider);
            SeedAdministrators(serviceProvider);
            SeedOfficialHolidays(serviceProvider);
            SeedEmployeesLeaveTypes(serviceProvider);

            return app;
        }


        private static void SeedProjects(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<LeaveSystemDbContext>();

            if (data.Projects.Any())
            {
                return;
            }

            data.Projects.AddRange(new[]{
                new Project
                    {
                        Name = "MyHouse" ,
                        Description ="3-floor house with outdoor pool.",
                        StartDate = new DateTime(2020,03,05).Date,
                        EndDate = new DateTime(2020,06,05).Date
                    },
                new Project
                    {
                        Name = "Henry Ford school" ,
                        Description ="School management system",
                        StartDate = new DateTime(2020,06,01),
                        EndDate = new DateTime(2021,02,17)
                    },
                 new Project
                    {
                        Name = "Shoe Shop" ,
                        Description ="Shop fоr casual shoos",
                        StartDate = new DateTime(2021,10,05),
                        EndDate = new DateTime(2021,12,20)
                    },
            });

            data.SaveChanges();
        }

        private static void SeedTeams(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<LeaveSystemDbContext>();

            if (data.Teams.Any())
            {
                return;
            }

            data.Teams.AddRange(new[]{
                new Team
                    {
                        Name="Admins"
                    },
                new Team
                    {
                        ProjectId = 1
                    },
                new Team
                    {
                        ProjectId = 2
                    },
                new Team
                    {
                        ProjectId = 3
                    },

                });

            data.SaveChanges();
        }

        private static void SeedLeaveTypes(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<LeaveSystemDbContext>();

            if (data.LeaveTypes.Any())
            {
                return;
            }

            data.LeaveTypes.AddRange(new[]{
               new LeaveType
                    {
                        Name="Paid",
                        DefaultDays=20
                    },
               new LeaveType
                    {
                        Name="Blood charity",
                        DefaultDays=2
                    },
               new LeaveType
                    {
                        Name="Marriage",
                        DefaultDays=2
                    },
               new LeaveType
                    {
                        Name="Family death",
                        DefaultDays=2
                    },
               new LeaveType
                    {
                        Name="Unpaid",
                        DefaultDays=180
                    },
               new LeaveType
                    {
                        Name="Newborn",
                        DefaultDays=10
                    }

            });

            data.SaveChanges();
        }

        private static void SeedDepartaments(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<LeaveSystemDbContext>();
            if (data.Departments.Any())
            {
                return;
            }

            data.Departments.AddRange(new[]{
               new Department
                    {
                        Name="HR",
                    },
                new Department
                    {
                        Name="IT",
                    },
                 new Department
                    {
                        Name="Finance",
                    },
                  new Department
                    {
                        Name="Logistic",
                    },

            });

            data.SaveChanges();
        }

        private static void SeedEmployees(IServiceProvider serviceProvider)
        {
            //var data = serviceProvider.GetRequiredService<LeaveSystemDbContext>();

            var userManager = serviceProvider.GetRequiredService<UserManager<Employee>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            var firstTeamLead = new Employee
            {
                FirstName = "Boris",
                MiddleName = "Ivanov",
                LastName = "Stoyanov",
                Email = "boris.stoyanov@abv.bg",
                UserName = "boris.stoyanov@abv.bg",
                ImageUrl = "https://thumbs.dreamstime.com/z/artificial-nose-3655155.jpg",
                JobTitle = "Team Lead",
                HireDate = new DateTime(2010, 07, 20).ToUniversalTime().Date,
                DepartmentId = 3,
                TeamId = 2
            };

            var secondTeamLead = new Employee
            {
                FirstName = "Kubrat",
                MiddleName = "Hristov",
                LastName = "Hristoskov",
                Email = "Kubrat.Hristoskov@abv.bg",
                UserName = "Kubrat.Hristoskov@abv.bg",
                ImageUrl = "https://www.thesun.co.uk/wp-content/uploads/2019/09/000613.jpg",
                JobTitle = "Team Lead",
                HireDate = new DateTime(2010, 10, 21).ToUniversalTime().Date,
                DepartmentId = 2,
                TeamId = 3
            };

            var teamLeads = new List<Employee>();
            teamLeads.Add(firstTeamLead);
            teamLeads.Add(secondTeamLead);


            Task
                .Run(async () =>
                {
                    var roleExist = await roleManager.RoleExistsAsync(TeamLeadRoleName);

                    if (!roleExist)
                    {
                        var role = new IdentityRole { Name = TeamLeadRoleName };
                        await roleManager.CreateAsync(role);

                        foreach (var teamLead in teamLeads)
                        {
                            var result = await userManager.CreateAsync(teamLead, "111111");

                            await userManager.AddToRoleAsync(teamLead, role.Name);

                        }

                    }
                })
                .GetAwaiter()
                .GetResult();


            var employees = new List<Employee>(){
               new Employee
                   {
                       FirstName="Ivan",
                       MiddleName="Mandzhukov",
                       LastName="Ivanov",
                       Email="Ivan.Ivanov@abv.bg",
                       UserName="Ivan.Ivanov@abv.bg",
                       ImageUrl="https://data.whicdn.com/images/356867091/original.jpg",
                       JobTitle= "Specialist",
                       HireDate= new DateTime(1990,10,15).ToUniversalTime().Date,
                       DepartmentId=1,
                       TeamId=2,
                       TeamLeadId=firstTeamLead.Id
                   },

               new Employee
                   {
                       FirstName="Stefani",
                       MiddleName="Sokolova",
                       LastName="Petrova",
                       Email="Stefani.Petrova@abv.bg",
                       UserName="Stefani.Petrova@abv.bg",
                       ImageUrl="https://klohmakeup.files.wordpress.com/2013/03/layers20start20image2.jpg",
                       JobTitle= "Spacialist",
                       HireDate= new DateTime(2020,02,02).ToUniversalTime().Date,
                       DepartmentId=2,
                       TeamId=2,
                       TeamLeadId=firstTeamLead.Id

                   },

               new Employee
                   {
                       FirstName="Stoyan",
                       MiddleName="Dimitrov",
                       LastName="Petkov",
                       Email="Stoyan.Petkov@abv.bg",
                       UserName="Stoyan.Petkov@abv.bg",
                       ImageUrl="https://st.depositphotos.com/1807998/3521/i/950/depositphotos_35212277-stock-photo-young-man-in-park.jpg",
                       JobTitle= "Senior Specialist",
                       HireDate= new DateTime(2018,10,05).ToUniversalTime().Date,
                       DepartmentId=3,
                       TeamId=3,
                       TeamLeadId=secondTeamLead.Id

                   },
                new Employee
                   {
                       FirstName="Dimitrichka",
                       MiddleName="Georgieva",
                       LastName="Petkova",
                       Email="Dimitrichka.Petkova@abv.bg",
                       UserName="Dimitrichka.Petkova@abv.bg",
                       ImageUrl="https://data.whicdn.com/images/312637959/original.jpg",
                       JobTitle= "Senior Specialist",
                       HireDate= new DateTime(2010,07,20).ToUniversalTime().Date,
                       DepartmentId=3,
                       TeamId=3,
                       TeamLeadId=secondTeamLead.Id
                   },


            };



            Task
                .Run(async () =>
                {
                    var roleExist = await roleManager.RoleExistsAsync(UserRoleName);

                    if (roleExist)
                    {
                        return;
                    }

                    var role = new IdentityRole { Name = UserRoleName };
                    await roleManager.CreateAsync(role);
                    foreach (var employee in employees)
                    {
                        var result = await userManager.CreateAsync(employee, "111111");
                        await userManager.AddToRoleAsync(employee, role.Name);

                    }


                })
                .GetAwaiter()
                .GetResult();



        }
        private static void SeedAdministrators(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<Employee>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task
                .Run(async () =>
                {
                    if (await roleManager.RoleExistsAsync(AdministratorRoleName))
                    {
                        return;
                    }
                    var role = new IdentityRole { Name = AdministratorRoleName };
                    await roleManager.CreateAsync(role);

                    var admins = new List<Employee>();

                    var adminTeamLead = new Employee
                    {
                        FirstName = "Ivan",
                        MiddleName = "Stamenov",
                        LastName = "Petrov",
                        UserName = "admin.ivan.petrov@gmail.com",
                        Email = "admin.ivan.petrov@gmail.com",
                        ImageUrl = "https://data.whicdn.com/images/312637959/original.jpg",
                        JobTitle = "Team Lead",
                        HireDate = new DateTime(2010, 02, 20).ToUniversalTime().Date,
                        DepartmentId = 1,
                        TeamId=1
                    };

                    var adminEmployee = new Employee
                    {
                        FirstName = "Petkan",
                        MiddleName = "Petkanov",
                        LastName = "Petkanov",
                        UserName = "admin.petkan.petkanov@gmail.com",
                        Email = "admin.petkan.petkanov@gmail.com",
                        ImageUrl = "https://data.whicdn.com/images/312637959/original.jpg",
                        JobTitle = "Senior Specialist",
                        HireDate = new DateTime(2010, 02, 20).ToUniversalTime().Date,
                        DepartmentId = 1,
                        TeamId=1,
                        TeamLeadId = adminTeamLead.Id
                    };

                    admins.Add(adminTeamLead);
                    admins.Add(adminEmployee);

                    foreach (var admin in admins)
                    {
                        var result = await userManager.CreateAsync(admin, "111111");

                        await userManager.AddToRoleAsync(admin, role.Name);
                    }

                })
                .GetAwaiter()
                .GetResult();


        }

        private static void SeedOfficialHolidays(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<LeaveSystemDbContext>();

            if (data.OfficialHolidays.Any())
            {
                return;
            }

            var holidays = new[]  {
            new OfficialHoliday{
                Date= new DateTime(2021, 01, 01),
                Name= "New Year`s Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 03, 03),
                Name= "Liberation Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 04, 30),
                Name= "Good Friday",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 05, 01),
                Name= "International Workers Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 05, 02),
                Name= "Easter",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 05, 03),
                Name= "Easter",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 05, 04),
                Name= "Easter",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 05, 06),
                Name= "St George`s Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 05, 24),
                Name= "Sts Cyril and Methodius Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 09, 06),
                Name= "Unification Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 09, 22),
                Name= "Independence Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 12, 24),
                Name= "Christmas",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 12, 25),
                Name= "Christmas",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 12, 26),
                Name= "Christmas",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 12, 27),
                Name= "Christmas",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 12, 28),
                Name= "Christmas",
            }};


            data.OfficialHolidays.AddRange(holidays);

            data.SaveChanges();

        }

        private static void SeedEmployeesLeaveTypes(IServiceProvider serviceProvider)
        {
            var data = serviceProvider.GetRequiredService<LeaveSystemDbContext>();

            if (data.EmployeesLeaveTypes.Any())
            {
                return;
            }

            var employeeIDs = data.Employees.Select(e => e.Id).ToList();
            var leaveTypeIDs = data.LeaveTypes.Select(lt => lt.Id).ToList();

            foreach (var employeeID in employeeIDs)
            {
                foreach (var leaveTypeID in leaveTypeIDs)
                {
                    var employeeLeaveType = new EmployeeLeaveType
                    {
                        EmployeeId = employeeID,
                        LeaveTypeId = leaveTypeID
                    };

                    data.EmployeesLeaveTypes.Add(employeeLeaveType);
                }

                data.SaveChanges();

            }
        }
    }
}
