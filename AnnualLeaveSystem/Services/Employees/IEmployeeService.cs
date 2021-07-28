namespace AnnualLeaveSystem.Services.Employees
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEmployeeService
    {
        public string GetTeamLeadId(string employeeId);
    }
}
