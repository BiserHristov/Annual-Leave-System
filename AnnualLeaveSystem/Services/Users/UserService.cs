namespace AnnualLeaveSystem.Services.Users
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly LeaveSystemDbContext db;
        private readonly IConfigurationProvider mapper;

        public UserService(LeaveSystemDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper.ConfigurationProvider;
        }

        public IEnumerable<RegisterDepartamentViewModel> AllDepartments()
        {
            return this.db.Departments
                .OrderBy(d => d.Name)
                .ProjectTo<RegisterDepartamentViewModel>(this.mapper)
                //.Select(d => new RegisterDepartamentViewModel
                //{
                //    Id = d.Id,
                //    Name = d.Name
                //})
                .ToList();
        }

        public IEnumerable<int> AllTeams()
        {
            return this.db.Teams
                .OrderBy(t => t.Id)
                .Select(t => t.Id)
                .ToList();
        }

        public string GetTeamLeadId(int teamId)
        {
            var teamLeadId = this.db.Employees
                .Include(e => e.Team)
                .Where(e => e.TeamId == teamId &&
                            e.TeamLeadId != null)
                .Select(e => e.TeamLeadId)
                .FirstOrDefault();

            return teamLeadId;
        }

        public void AddLeaveTypesToEmployee(string employeeId)
        {
            if (this.db.EmployeesLeaveTypes.Any(el => el.EmployeeId == employeeId))
            {
                return;
            }

            var leaveTypeIDs = this.db.LeaveTypes
                                      .Select(lt => lt.Id)
                                      .ToList();

            foreach (var typeId in leaveTypeIDs)
            {
                var employeeLeaveType = new EmployeeLeaveType
                {
                    EmployeeId = employeeId,
                    LeaveTypeId = typeId
                };

                this.db.EmployeesLeaveTypes.Add(employeeLeaveType);
            }

            this.db.SaveChanges();

        }


    }
}
