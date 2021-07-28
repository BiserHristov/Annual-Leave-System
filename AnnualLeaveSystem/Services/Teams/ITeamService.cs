namespace AnnualLeaveSystem.Services.Teams
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ITeamService
    {
        public bool EmployeeExistInTeam(int teamId, string employeeId);
    }
}
