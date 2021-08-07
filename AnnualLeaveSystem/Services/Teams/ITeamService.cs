namespace AnnualLeaveSystem.Services.Teams
{
    public interface ITeamService
    {
        public bool EmployeeExistInTeam(int? teamId, string employeeId);
    }
}
