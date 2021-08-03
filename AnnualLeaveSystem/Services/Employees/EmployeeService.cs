namespace AnnualLeaveSystem.Services.Employees
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
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

        public bool Exist(string employeeId)
        {
            return this.db.Employees.Any(e => e.Id == employeeId);
        }

        public int? TeamId(string employeeId)
        {
            return this.db.Employees
                .Where(e => e.Id == employeeId)
                .Select(e => e.TeamId)
                .FirstOrDefault();
        }

        //public Employee GetEmployee(string employeeId)
        //{
        //    return this.db.Employees
        //             .Where(e => e.Id == employeeId)
        //             .FirstOrDefault();
        //}

        public string TeamLeadId(string employeeId)
        {
            var teamLeadId = this.db.Employees
                     .Where(e => e.Id == employeeId)
                     .Select(e => e.TeamLeadId)
                     .FirstOrDefault();

            return teamLeadId;
        }

        

    }
}
