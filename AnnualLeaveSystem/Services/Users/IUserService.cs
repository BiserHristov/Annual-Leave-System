namespace AnnualLeaveSystem.Services.Users
{
    using AnnualLeaveSystem.Models.Users;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IUserService
    {
        public IEnumerable<RegisterDepartamentViewModel> AllDepartments();
        public IEnumerable<int> AllTeams();
        public string GetTeamLeadId(int teamId);
        public void AddLeaveTypesToEmployee(string employeeId);
    }
}
