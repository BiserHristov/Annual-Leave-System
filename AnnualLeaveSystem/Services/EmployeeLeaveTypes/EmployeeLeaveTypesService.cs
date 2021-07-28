namespace AnnualLeaveSystem.Services.EmployeeLeaveTypes
{
    using AnnualLeaveSystem.Data;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EmployeeLeaveTypesService : IEmployeeLeaveTypesService
    {
        private readonly LeaveSystemDbContext db;

        public EmployeeLeaveTypesService(LeaveSystemDbContext db)
        {
            this.db = db;
        }

        public EmployeeLeaveTypesServiceModel GetLeaveType(string employeeId, int leaveTypeId)
        {
            return this.db.EmployeesLeaveTypes
              .Include(x => x.LeaveType)
              .Where(el => el.EmployeeId == employeeId &&
                           el.LeaveTypeId == leaveTypeId)
              .Select(el => new EmployeeLeaveTypesServiceModel
              {
                  UsedDays = el.UsedDays,
                  RemainingDays = el.RemainingDays,
                  PendingApprovalDays = el.PendingApprovalDays,
              })
              .FirstOrDefault();
        }
    }
}
