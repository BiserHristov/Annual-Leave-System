namespace AnnualLeaveSystem.Services.Employees
{
    using AnnualLeaveSystem.Data.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEmployeeService
    {
        public string TeamLeadId(string employeeId);

        public int? TeamId(string employeeId);

        public bool Exist(string employeeId);

        
    }
}
