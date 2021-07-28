namespace AnnualLeaveSystem.Services.Users
{
    using System.Collections.Generic;

    public interface IUserService
    {
        public IEnumerable<RegisterDepartamentViewModel> AllDepartments();
        public IEnumerable<int> AllTeams();
        public string GetTeamLeadId(int teamId);
        public void AddLeaveTypesToEmployee(string employeeId);
    }
}
