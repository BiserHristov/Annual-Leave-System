namespace AnnualLeaveSystem.Services.Employees
{
    using AnnualLeaveSystem.Data;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EmployeeService : IEmployeeService
    {
        private readonly LeaveSystemDbContext db;

        public EmployeeService(LeaveSystemDbContext db)
        {
            this.db = db;
        }
        public string GetTeamLeadId(string employeeId)
        {
            var teamLeadId = this.db.Employees
                     .Where(e => e.Id == employeeId)
                     .Select(e => e.TeamLeadId)
                     .FirstOrDefault();

            return teamLeadId;
        }
    }
}
