namespace AnnualLeaveSystem.Services.Teams
{
    using AnnualLeaveSystem.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TeamService : ITeamService
    {
        private readonly LeaveSystemDbContext db;

        public TeamService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public bool EmployeeExistInTeam(int teamId, string employeeId)
        {
            return this.db.Teams
                .Any(t => t.Id == teamId &&
                          t.Employees.Any(e => e.Id == employeeId));
        }
    }
}
