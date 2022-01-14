namespace AnnualLeaveSystem.Services.Employees
{
    using System.Linq;
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;

    public class EmployeeService : IEmployeeService
    {
        private readonly LeaveSystemDbContext db;

        public EmployeeService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public bool Exist(string employeeId)
        {
            return this.db.Employees.Any(e => e.Id == employeeId);
        }

        public string FullName(string employeeId)
            => this.db.Employees
                .Where(e => e.Id == employeeId)
                .Select(e =>
                    e.FirstName + ' ' +
                    (string.IsNullOrEmpty(e.MiddleName) ? string.Empty : e.MiddleName + ' ') + e.LastName)
                .FirstOrDefault();

        public Employee Get(string employeeId)
            => this.db.Employees
                .Where(e => e.Id == employeeId)
                .FirstOrDefault();

        public bool IsSameTeam(string firstEmployeeId, string secondEmployeeId)
        {
            var firstEmployeeTeamId = this.TeamId(firstEmployeeId);
            var secondEmployeeTeamId = this.TeamId(secondEmployeeId);

            return firstEmployeeTeamId == secondEmployeeTeamId;
        }

        public int TeamId(string employeeId)
            => this.db.Employees
                .Where(e => e.Id == employeeId)
                .Select(e => e.TeamId)
                .FirstOrDefault();

        public string TeamLeadId(string employeeId)
            => this.db.Employees
                     .Where(e => e.Id == employeeId)
                     .Select(e => e.TeamLeadId)
                     .FirstOrDefault();
    }
}
