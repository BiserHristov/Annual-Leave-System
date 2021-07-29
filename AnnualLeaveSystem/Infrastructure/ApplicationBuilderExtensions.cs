namespace AnnualLeaveSystem.Infrastructure
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;


    public static class ApplicationBuilderExtensions
    {


        //static ApplicationBuilderExtensions()
        //{

        //}

        public static IApplicationBuilder PrepareDatabase(
            this IApplicationBuilder app)
        {
            using var scopedServices = app.ApplicationServices.CreateScope();

            var data = scopedServices.ServiceProvider.GetService<LeaveSystemDbContext>();

            data.Database.Migrate();

            SeedProjects(data);
            SeedTeams(data);
            SeedLeaveTypes(data);
            SeedDepartaments(data);
            SeedEmployees(data);
            SeedOfficialHolidays(data);
            SeedEmployeesLeaveTypes(data);

            return app;
        }

        private static void SeedProjects(LeaveSystemDbContext data)
        {
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

        private static void SeedTeams(LeaveSystemDbContext data)
        {
            if (data.Teams.Any())
            {
                return;
            }

            data.Teams.AddRange(new[]{
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

        private static void SeedLeaveTypes(LeaveSystemDbContext data)
        {
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

        private static void SeedDepartaments(LeaveSystemDbContext data)
        {
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

        private static void SeedEmployees(LeaveSystemDbContext data)
        {
            if (data.Employees.Any())
            {
                return;
            }

            var firstTeamLead = new Employee
            {
                FirstName = "Boris",
                MiddleName = "Ivanov",
                LastName = "Stoyanov",
                UserName = "boris.stoyanov@abv.bg",
                ImageUrl = "https://thumbs.dreamstime.com/z/artificial-nose-3655155.jpg",
                JobTitle = "Team Lead",
                HireDate = new DateTime(2010, 07, 20).ToUniversalTime().Date,
                DepartmentId = 3,
                TeamId = 1
            };

            var secondTeamLead = new Employee
            {
                FirstName = "Kubrat",
                MiddleName = "Hristov",
                LastName = "Hristoskov",
                UserName = "Kubrat.Hristoskov@abv.bg",
                ImageUrl = "https://www.thesun.co.uk/wp-content/uploads/2019/09/000613.jpg",
                JobTitle = "Team Lead",
                HireDate = new DateTime(2010, 10, 21).ToUniversalTime().Date,
                DepartmentId = 2,
                TeamId = 2
            };


            //await userManager.CreateAsync(firstTeamLead, "111111");
            //await userManager.CreateAsync(secondTeamLead, "111111");

            data.Employees.Add(firstTeamLead);
            data.Employees.Add(secondTeamLead);

            data.Employees.AddRange(new[]{
               new Employee
                   {
                       FirstName="Ivan",
                       MiddleName="Mandzhukov",
                       LastName="Ivanov",
                       UserName="Ivan.Ivanov@abv.bg",
                       PasswordHash="111111",
                       ImageUrl="https://data.whicdn.com/images/356867091/original.jpg",
                       JobTitle= "Specialist",
                       HireDate= new DateTime(1990,10,15).ToUniversalTime().Date,
                       DepartmentId=1,
                       TeamId=1,
                       TeamLeadId=firstTeamLead.Id
                   },

               new Employee
                   {
                       FirstName="Stefani",
                       MiddleName="Sokolova",
                       LastName="Petrova",
                       UserName="Stefani.Petrova@abv.bg",
                       PasswordHash="111111",
                       ImageUrl="https://klohmakeup.files.wordpress.com/2013/03/layers20start20image2.jpg",
                       JobTitle= "Spacialist",
                       HireDate= new DateTime(2020,02,02).ToUniversalTime().Date,
                       DepartmentId=2,
                       TeamId=1,
                       TeamLeadId=firstTeamLead.Id

                   },

               new Employee
                   {
                       FirstName="Stoyan",
                       MiddleName="Dimitrov",
                       LastName="Petkov",
                       UserName="Stoyan.Petkov@abv.bg",
                       PasswordHash="111111",
                       ImageUrl="https://st.depositphotos.com/1807998/3521/i/950/depositphotos_35212277-stock-photo-young-man-in-park.jpg",
                       JobTitle= "Senior Specialist",
                       HireDate= new DateTime(2018,10,05).ToUniversalTime().Date,
                       DepartmentId=3,
                       TeamId=2,
                       TeamLeadId=secondTeamLead.Id

                   },
                new Employee
                   {
                       FirstName="Dimitrichka",
                       MiddleName="Georgieva",
                       LastName="Petkova",
                       UserName="Dimitrichka.Petkova@abv.bg",
                       PasswordHash="111111",
                       ImageUrl="https://data.whicdn.com/images/312637959/original.jpg",
                       JobTitle= "Senior Specialist",
                       HireDate= new DateTime(2010,07,20).ToUniversalTime().Date,
                       DepartmentId=3,
                       TeamId=2,
                       TeamLeadId=secondTeamLead.Id
                   },


            });


            data.SaveChanges();

        }

        private static void SeedOfficialHolidays(LeaveSystemDbContext data)
        {
            if (data.OfficialHolidays.Any())
            {
                return;
            }

            var holidays = new[]  {
            new OfficialHoliday{
                Date= new DateTime(2021, 01, 01).Date,
                Name= "New Year`s Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 03, 03).Date,
                Name= "Liberation Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 04, 30).Date,
                Name= "Good Friday",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 05, 01).Date,
                Name= "International Workers Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 05, 02).Date,
                Name= "Easter",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 05, 03).Date,
                Name= "Easter",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 05, 04).Date,
                Name= "Easter",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 05, 06).Date,
                Name= "St George`s Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 05, 24).Date,
                Name= "Sts Cyril and Methodius Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 09, 06).Date,
                Name= "Unification Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 09, 22).Date,
                Name= "Independence Day",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 12, 24).Date,
                Name= "Christmas",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 12, 25).Date,
                Name= "Christmas",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 12, 26).Date,
                Name= "Christmas",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 12, 27).Date,
                Name= "Christmas",
            },
            new OfficialHoliday{
                Date= new DateTime(2021, 12, 28).Date,
                Name= "Christmas",
            }};


            data.OfficialHolidays.AddRange(holidays);

            data.SaveChanges();

        }

        private static void SeedEmployeesLeaveTypes(LeaveSystemDbContext data)
        {
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


//
//,
//new DateTime,
//new DateTime(2021, ),
//new DateTime(2021, ),
//new DateTime(2021, ),
//new DateTime(2021, ),
//new DateTime(2021, ),
//new DateTime(2021,),
//new DateTime(2021, ),
//new DateTime(2021, ),
//new DateTime(2021, ),
//new DateTime(2021, ),
//new DateTime(2021, 12, 25),
//new DateTime(2021, 12, 26),
//new DateTime(2021, 12, 27),
//new DateTime(2021, 12, 28),