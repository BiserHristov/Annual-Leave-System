namespace AnnualLeaveSystem.Services.Leaves
{
    using AnnualLeaveSystem.Data.Models;
    using AnnualLeaveSystem.Models.Leaves;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public interface ILeaveService
    {
        LeaveQueryServiceModel All(
            Status? status,
            string firstName,
            string lastName,
            LeaveSorting sorting,
            int currentPage,
            int leavesPerPage);

        IEnumerable<LeaveServiceModel> ByEmployee(string employeeId);
    }
}
