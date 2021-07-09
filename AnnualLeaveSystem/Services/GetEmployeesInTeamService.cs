namespace AnnualLeaveSystem.Services
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Models.Leaves;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class GetEmployeesInTeamService : IGetEmployeesInTeamService
    {
        private readonly LeaveSystemDbContext db;

        public GetEmployeesInTeamService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        IEnumerable<ReplacementEmployeeViewModel> IGetEmployeesInTeamService.GetEmployeesInTeam()
        {
            return this.db.Employees
              .Where(e => e.TeamId == 2) // TODO: Take the current user teamId!!!
              .Select(e => new ReplacementEmployeeViewModel
              {
                  Id = e.Id,
                  Name = $"{e.FirstName} {e.MiddleName} {e.LastName}",
              })
              .ToList();
        }
    }
}
