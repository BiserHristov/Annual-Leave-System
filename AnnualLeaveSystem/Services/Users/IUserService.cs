namespace AnnualLeaveSystem.Services.Users
{
    using System.Collections.Generic;

    public interface IUserService
    {
        public IEnumerable<RegisterDepartamentServiceModel> AllDepartments();

        public IEnumerable<int> AllTeams();

        public string GetTeamLeadId(int? teamId);

        public void AddLeaveTypesToEmployee(string employeeId);

        public bool Exist(string email);
    }
}
