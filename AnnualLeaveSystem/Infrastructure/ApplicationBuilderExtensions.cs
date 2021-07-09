namespace AnnualLeaveSystem.Infrastructure
{
    using System;
    using System.Linq;
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;


    public static class ApplicationBuilderExtensions
    {

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
                        StartDate = new DateTime(2020,03,05),
                        EndDate = new DateTime(2020,06,05)
                    },
                new Project
                    {
                        Name = "Henry Ford school" ,
                        Description ="School management system",
                        StartDate = new DateTime(2020,06,01),
                        EndDate = new DateTime(2021,02,17)
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
                        ProjectId = 5
                    },
                new Team
                    {
                        ProjectId = 6
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
                        Name="BloodCharity",
                        DefaultDays=3
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

            data.Employees.AddRange(new[]{
               new Employee
                   {
                       FirstName="Ivan",
                       MiddleName="Mandzhukov",
                       LastName="Ivanov",
                       ImageUrl="https://data.whicdn.com/images/356867091/original.jpg",
                       JobTitle= "Specialist",
                       DepartmentId=1,
                       TeamId=2

                   },

               new Employee
                   {
                       FirstName="Stefani",
                       MiddleName="Sokolova",
                       LastName="Petrova",
                       ImageUrl="https://klohmakeup.files.wordpress.com/2013/03/layers20start20image2.jpg",
                       JobTitle= "Spacialist",
                       DepartmentId=2,
                       TeamId=2

                   },

               new Employee
                   {
                       FirstName="Stoyan",
                       MiddleName="Dimitrov",
                       LastName="Petkov",
                       ImageUrl="https://st.depositphotos.com/1807998/3521/i/950/depositphotos_35212277-stock-photo-young-man-in-park.jpg",
                       JobTitle= "Senior Specialist",
                       DepartmentId=3,
                       TeamId=3

                   },
                new Employee
                   {
                       FirstName="Dimtrichka",
                       MiddleName="Georgieva",
                       LastName="Petkova",
                       ImageUrl="https://data.whicdn.com/images/312637959/original.jpg",
                       JobTitle= "Senior Specialist",
                       DepartmentId=3,
                       TeamId=3

                   },


            });

            data.SaveChanges();
        }


    }
}
