namespace AnnualLeaveSystem.Services
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Models.Leaves;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static AnnualLeaveSystem.Data.DataConstants;
    public class GetEmployeesInTeamService : IGetEmployeesInTeamService
    {
        private readonly LeaveSystemDbContext db;

        public GetEmployeesInTeamService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        IEnumerable<SubstituteEmployeeViewModel> IGetEmployeesInTeamService.GetEmployeesInTeam()
        {
            return this.db.Employees
              .Where(e => e.TeamId == _EmployeeTeamId && e.Id != _EmployeeId) // TODO: Take the current user teamId!!!
              .Select(e => new SubstituteEmployeeViewModel
              {
                  Id = e.Id,
                  Name = $"{e.FirstName} {e.MiddleName} {e.LastName}",
              })
              .ToList();
        }
    }
}
