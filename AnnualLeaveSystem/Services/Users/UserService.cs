namespace AnnualLeaveSystem.Services.Users
{
    using AnnualLeaveSystem.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

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
                .OrderBy(d=>d.Name)
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
                .OrderBy(t=>t.Id)
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
    }
}
