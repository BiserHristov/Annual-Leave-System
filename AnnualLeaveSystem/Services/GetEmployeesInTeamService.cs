namespace AnnualLeaveSystem.Services
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Leaves;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Linq;
    using static AnnualLeaveSystem.Data.DataConstants.User;

    public class GetEmployeesInTeamService : IGetEmployeesInTeamService
    {
        private readonly LeaveSystemDbContext db;

        public GetEmployeesInTeamService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public IEnumerable<SubstituteEmployeeViewModel> GetEmployeesInTeam(string currentEmployeeId)
        {

            //var currentEmployeeTeamId = this.db.Employees
            //    .Where(e=>e.Id== currentEmployeeId)
            //    .Select(e=>e.TeamId)
            //    .FirstOrDefault();

            return this.db.Employees
              .Where(e => e.TeamId == _EmployeeTeamId && e.Id != currentEmployeeId) // TODO: Take the current user teamId!!!
              .Select(e => new SubstituteEmployeeViewModel
              {
                  Id = e.Id,
                  Name = $"{e.FirstName} {e.MiddleName} {e.LastName}",
              })
              .ToList();
        }

       
    }
}
