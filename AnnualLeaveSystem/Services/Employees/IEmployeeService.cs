﻿namespace AnnualLeaveSystem.Services.Employees
{
    using AnnualLeaveSystem.Data.Models;

    public interface IEmployeeService
    {
        public string TeamLeadId(string employeeId);

        public string FullName(string employeeId);

        public int TeamId(string employeeId);

        public bool Exist(string employeeId);

        public bool IsSameTeam(string firstEmployeeId, string secondEmployeeId);

        public Employee Get(string employeeId);
    }
}
