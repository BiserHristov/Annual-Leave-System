namespace AnnualLeaveSystem.Services.EmployeeLeaveTypes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface IEmployeeLeaveTypesService
    {
        public EmployeeLeaveTypesServiceModel GetLeaveType(string employeeId, int leaveTypeId);
    }
}
