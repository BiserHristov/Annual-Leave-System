namespace AnnualLeaveSystem.Services.Users
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using System.Collections.Generic;
    using System.Linq;

    public class UserService : IUserService
    {
        private readonly LeaveSystemDbContext db;

        public UserService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<RegisterDepartamentViewModel> AllDepartments()
        {
            return this.db.Departments
                .OrderBy(d => d.Name)
                .Select(d => new RegisterDepartamentViewModel
                {
                    Id = d.Id,
                    Name = d.Name
                })
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
            var leadId = this.db.Employees
                .Where(e => e.TeamId == teamId)
                .Select(e => e.TeamLeadId)
                .FirstOrDefault();

            return leadId;
        }

        public void AddLeaveTypesToEmployee(string employeeId)
        {
            if (this.db.EmployeesLeaveTypes.Any(el=>el.EmployeeId==employeeId))
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
