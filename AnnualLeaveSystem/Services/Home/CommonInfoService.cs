﻿namespace AnnualLeaveSystem.Services.Statistics
{
    using System;
    using System.Linq;
    using AnnualLeaveSystem.Data;
    using AnnualLeaveSystem.Data.Models;

    public class CommonInfoService : ICommonInfoService
    {
        private readonly LeaveSystemDbContext db;

        public CommonInfoService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public CommonInfoServiceModel Get()
        {
            var employeesCount = this.db.Employees.Count();

            var approvedLeavesCount = this.db.Leaves
                .Count(l => l.LeaveStatus == Status.Approved);

            var inProgressLeavesCount = this.db.Leaves
                .Count(l => l.LeaveStatus == Status.Pending);

            var allLeavesTotalDays = this.db.Leaves
                .Sum(l => l.TotalDays);

            var missingEmployees = this.db.Leaves
                .Count(l => l.LeaveStatus == Status.Approved &&
                (l.StartDate <= DateTime.Now.Date && DateTime.Now.Date <= l.EndDate));

            return new CommonInfoServiceModel
            {
                EmployeesCount = employeesCount,
                ApprovedLeaveCount = approvedLeavesCount,
                InProgressLeaveCount = inProgressLeavesCount,
                AllLeavesTotalDays = allLeavesTotalDays,
                MissingEmployees = missingEmployees,
            };
        }
    }
}