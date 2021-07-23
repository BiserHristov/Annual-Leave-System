namespace AnnualLeaveSystem.Services.Statistics
{
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class StatisticsService : IStatisticsService
    {
        private readonly LeaveSystemDbContext db;

        public StatisticsService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public StatisticsServiceModel Get()
        {
            var employeesCount = this.db.Employees.Count();

            var approvedLeavesCount = this.db
                .Leaves
                .Count(l => l.LeaveStatus == Status.Approved);
            var inProgressLeavesCount = this.db
                .Leaves
                .Count(l => l.LeaveStatus == Status.Pending);
            var allLeavesTotalDays = this.db
                .Leaves
                .Sum(l => l.TotalDays);

            return new StatisticsServiceModel
            {
                EmployeesCount = employeesCount,
                ApprovedLeaveCount = approvedLeavesCount,
                InProgressLeaveCount = inProgressLeavesCount,
                AllLeavesTotalDays = allLeavesTotalDays,
            };

        }
    }
}

//EmployeesCount = this.db.Employees.Count(),
//                ApprovedLeaveCount = this.db.Leaves.Count(l => l.LeaveStatus == Status.Approved),
//                InProgressLeaveCount = this.db.Leaves.Count(l => l.LeaveStatus == Status.Pending),
//                AllLeavesTotalDays = this.db.Leaves.Sum(l => l.TotalDays),